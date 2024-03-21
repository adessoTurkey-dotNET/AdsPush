using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net.Http;
using System;
using AdsPush.Abstraction.Settings;
using AdsPush.Abstraction.HMS;
using System.Threading;
using Newtonsoft.Json;
using System.Threading.Tasks;
using AdsPush.Abstraction;
using System.Net.Http.Headers;
using AdsPush.HMS.Helpers;
using Newtonsoft.Json.Linq;
using AdsPush.HMS.Extensions;

namespace AdsPush.HMS
{
    public class HMSPushNotificationSender : IHMSPushNotificationSender
    {
        private static readonly ConcurrentDictionary<string, Tuple<string, DateTime>> Tokens
            = new ConcurrentDictionary<string, Tuple<string, DateTime>>();
        private static readonly Dictionary<HMSBaseUrl, string> Servers
            = new Dictionary<HMSBaseUrl, string> { { HMSBaseUrl.Auth, "https://oauth-login.cloud.huawei.com" },
                                                      { HMSBaseUrl.Api, "https://push-api.cloud.huawei.com/v1/" } };

        private readonly AdsPushHMSSettings _settings;
        private readonly HttpClient _http;
        private readonly HttpClient _authHttp;


        /// <summary>
        /// Apple push notification sender constructor
        /// </summary>
        /// <param name="settings">HMS Push Notification settings</param>
        /// <param name="http">HTTP client instance</param>
        public HMSPushNotificationSender(AdsPushHMSSettings settings, HttpClient http, HttpClient authHttp)
        {
            this._settings = settings ?? throw new ArgumentNullException(nameof(settings));
            this._http = http ?? throw new ArgumentNullException(nameof(http));
            this._authHttp = authHttp ?? throw new ArgumentNullException(nameof(authHttp));

            if (http.BaseAddress == null)
            {
                http.BaseAddress = new Uri(Servers[HMSBaseUrl.Api]);
            }
            if (authHttp.BaseAddress == null)
            {
                authHttp.BaseAddress = new Uri(Servers[HMSBaseUrl.Auth]);
            }
        }

        private async Task<string> CreateJwtToken(CancellationToken cancellationToken)
        {
            var path = "/oauth2/v3/token";

            var payload = new Dictionary<string, string>
            {
                { "grant_type", "client_credentials" },
                { "client_id", this._settings.ClientId },
                { "client_secret", this._settings.ClientSecret }
            };

            var request = new HttpRequestMessage(HttpMethod.Post, path);
            request.Version = new Version(2, 0);
            request.Content = new FormUrlEncodedContent(payload);
            request.Headers.TryAddWithoutValidation(":method", "POST");
            request.Headers.TryAddWithoutValidation(":path", path);
            request.Headers.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
            request.Headers.TryAddWithoutValidation("cache-control", "no-store");

            var response = await this._authHttp.SendAsync(request, cancellationToken);

            var succeed = response.IsSuccessStatusCode;
            var content = await response.Content.ReadAsStringAsync();
            HMSOAuthResponse res;
            res = JsonConvert.DeserializeObject<HMSOAuthResponse>(content);

            //var error = JsonConvert.DeserializeObject<HMSError>(content);
            //if (error != null)
            //{
            //    //error.HttpResponse = response;
            //}

            Console.WriteLine(content);
            return res.Access_Token;
        }

        public async Task<HMSResponse> SendAsync(
            HMSRequest hmsRequest,
            string deviceToken,
            Guid applicationId,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var jsonObject = new JObject()
            {
                ["validate_only"] = false,
                ["message"] = JObject.FromObject(hmsRequest)
            };

            var jsonPayload = JsonHelper.Serialize(jsonObject);
            return await this.SendAsync(
                jsonPayload,
                deviceToken,
                applicationId,
                cancellationToken);
        }

        public async Task<HMSResponse> SendAsync(
            string dataPayload,
            string deviceToken,
            Guid applicationId,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var path = $"/v1/{applicationId}/messages:send";
            var request = new HttpRequestMessage(HttpMethod.Post, path);
            request.Content = new StringContent(dataPayload);
            var hmsPushToken = await this.CreateJwtToken(cancellationToken);
            request.Headers.Authorization = new AuthenticationHeaderValue("bearer", hmsPushToken);

            var response = await this._http.SendAsync(request, cancellationToken);
            var succeed = response.IsSuccessStatusCode;
            var content = await response.Content.ReadAsStringAsync();
            var error = JsonHelper.Deserialize<HMSError>(content);
            if (error != null)
            {
                error.HttpResponse = response;
            }

            return new HMSResponse { IsSuccess = succeed, Error = error };
        }

        public async Task SendAsync(
            string deviceToken,
            AdsPushBasicSendPayload payload,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var response = await this.SendAsync(
                payload.CreateRequest(),
                deviceToken,
                Guid.Parse(payload.Id),
                cancellationToken: cancellationToken);

            if (!response.IsSuccess)
            {
                throw response.Error.CreateException();
            }
        }


    }
}
