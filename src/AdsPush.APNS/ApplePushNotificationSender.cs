using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using AdsPush.Abstraction;
using AdsPush.Abstraction.APNS;
using AdsPush.Abstraction.Settings;
using AdsPush.APNS.Helpers;
using AdsPush.APNS.Extensions;

namespace AdsPush.APNS
{
    /// <summary>
    /// HTTP2 Apple Push Notification sender
    /// </summary>
    internal class ApplePushNotificationSender : IApplePushNotificationSender
    {
        private static readonly ConcurrentDictionary<string, Tuple<string, DateTime>> Tokens = new ConcurrentDictionary<string, Tuple<string, DateTime>>();
        private static readonly Dictionary<APNSEnvironmentType, string> Servers = new Dictionary<APNSEnvironmentType, string> { { APNSEnvironmentType.Development, "https://api.development.push.apple.com:443" }, { APNSEnvironmentType.Production, "https://api.push.apple.com:443" } };

        private const string ApnIdHeader = "apns-id";
        private const int TokenExpiresMinutes = 50;

        private readonly AdsPushAPNSSettings _settings;
        private readonly HttpClient _http;

        /// <summary>
        /// Apple push notification sender constructor
        /// </summary>
        /// <param name="settings">Apple Push Notification settings</param>
        /// <param name="http">HTTP client instance</param>
        public ApplePushNotificationSender(
            AdsPushAPNSSettings settings,
            HttpClient http)
        {
            this._settings = settings ?? throw new ArgumentNullException(nameof(settings));
            this._http = http ?? throw new ArgumentNullException(nameof(http));

            if (http.BaseAddress == null)
            {
                http.BaseAddress = new Uri(Servers[settings.EnvironmentType]);
            }
        }

        /// <inheritdoc />
        public Task<APNSResponse> SendAsync(
            APNSRequest apnsRequest,
            string deviceToken,
            Guid apnsId,
            APNSExpiration apnsExpiration = null,
            int apnsPriority = 10,
            bool isBackground = false,
            CancellationToken cancellationToken = default)
        {
            var jsonObject = new JObject()
            {
                ["aps"] = JObject.FromObject(apnsRequest.ApnsPayload)
            };

            foreach (var item in apnsRequest.AdditionalParameters)
            {
                jsonObject[item.Key] = JToken.FromObject(item.Value);
            }

            var json = JsonHelper.Serialize(jsonObject);

            return this.SendAsync(
                json,
                deviceToken,
                apnsId,
                apnsExpiration,
                apnsPriority,
                isBackground,
                cancellationToken);
        }

        /// <inheritdoc />
        public Task<APNSResponse> SendAsync(
            object objectPayload,
            string deviceToken,
            Guid apnsId,
            APNSExpiration apnsExpiration = null,
            int apnsPriority = 10,
            bool isBackground = false,
            CancellationToken cancellationToken = default)
        {
            var json = JsonHelper.Serialize(objectPayload);

            return this.SendAsync(
                json,
                deviceToken,
                apnsId,
                apnsExpiration,
                apnsPriority,
                isBackground,
                cancellationToken);
        }

        /// <inheritdoc />
        public async Task<APNSResponse> SendAsync(
            string jsonPayload,
            string deviceToken,
            Guid apnsId,
            APNSExpiration apnsExpiration = null,
            int apnsPriority = 10,
            bool isBackground = false,
            CancellationToken cancellationToken = default)
        {
            var path = $"/3/device/{deviceToken}";

            var message = new HttpRequestMessage(HttpMethod.Post, path);
            message.Version = new Version(2, 0);
            message.Content = new StringContent(jsonPayload);

            message.Headers.Authorization = new AuthenticationHeaderValue("bearer", this.GetJwtToken());
            message.Headers.TryAddWithoutValidation(":method", "POST");
            message.Headers.TryAddWithoutValidation(":path", path);
            message.Headers.Add("apns-topic", this._settings.AppBundleIdentifier);
            message.Headers.Add("apns-expiration", (apnsExpiration ?? APNSExpiration.SingeTry()).ApnsExpirationValue.ToString());
            message.Headers.Add("apns-priority", apnsPriority.ToString());
            message.Headers.Add("apns-push-type", isBackground ? "background" : "alert"); // required for iOS 13+
            message.Headers.Add(ApnIdHeader, apnsId.ToString());

            var response = await this._http.SendAsync(message, cancellationToken);
            var succeed = response.IsSuccessStatusCode;
            var content = await response.Content.ReadAsStringAsync();
            var error = JsonHelper.Deserialize<APNSError>(content);
            if (error != null)
            {
                error.HttpResponse = response;
            }

            return new APNSResponse { IsSuccess = succeed, Error = error };
        }

        public async Task SendAsync(
            string deviceToken,
            AdsPushBasicSendPayload payload,
            CancellationToken cancellationToken = default)
        {
            var response = await this.SendAsync(
                payload.CreateRequest(),
                deviceToken,
                Guid.Parse(payload.Id),
                isBackground: payload.PushType == AdsPushType.Background,
                cancellationToken: cancellationToken);

            if (!response.IsSuccess)
            {
                throw response.Error.CreateException();
            }
        }

        private string GetJwtToken()
        {
            var (token, date) = Tokens.GetOrAdd(this._settings.AppBundleIdentifier, _ => new Tuple<string, DateTime>(this.CreateJwtToken(), DateTime.UtcNow));
            if (date >= DateTime.UtcNow.AddMinutes(-TokenExpiresMinutes))
            {
                return token;
            }

            Tokens.TryRemove(this._settings.AppBundleIdentifier, out _);
            return this.GetJwtToken();
        }

        private string CreateJwtToken()
        {
            var header = JsonHelper.Serialize(new { alg = "ES256", kid = CleanP8Key(this._settings.P8PrivateKeyId) });
            var payload = JsonHelper.Serialize(new { iss = this._settings.TeamId, iat = EpochTime() });
            var headerBase64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(header));
            var payloadBase64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(payload));
            var unsignedJwtData = $"{headerBase64}.{payloadBase64}";
            var unsignedJwtBytes = Encoding.UTF8.GetBytes(unsignedJwtData);

            var dsa = AppleCryptoHelper.GetEllipticCurveAlgorithm(CleanP8Key(this._settings.P8PrivateKey));
            var signature = dsa.SignData(unsignedJwtBytes, 0, unsignedJwtBytes.Length, HashAlgorithmName.SHA256);
            return $"{unsignedJwtData}.{Convert.ToBase64String(signature)}";
        }

        private static int EpochTime()
        {
            var span = DateTime.UtcNow - new DateTime(1970, 1, 1);
            return Convert.ToInt32(span.TotalSeconds);
        }

        private static string CleanP8Key(
            string p8Key)
        {
            // If we have an empty p8Key, then don't bother doing any tasks.
            if (string.IsNullOrEmpty(p8Key))
            {
                return p8Key;
            }

            var lines = p8Key.Split('\n').ToList();

            if (0 != lines.Count && lines[0].StartsWith("-----BEGIN PRIVATE KEY-----"))
            {
                lines.RemoveAt(0);
            }

            if (0 != lines.Count && lines[lines.Count -1].StartsWith("-----END PRIVATE KEY-----"))
            {
                lines.RemoveAt(lines.Count - 1);
            }

            var result = string.Join(string.Empty, lines);

            return result;
        }
    }
}
