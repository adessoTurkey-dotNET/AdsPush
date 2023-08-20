using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using AdsPush.Abstraction;
using AdsPush.Abstraction.Settings;
using AdsPush.Abstraction.Vapid;
using AdsPush.Vapid.Extensions;
using AdsPush.Vapid.Util;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace AdsPush.Vapid
{
    public class VapidPushNotificationSender : IVapidPushNotificationSender
    {
        private const long DefaultTtl = 43200;
        private readonly HttpClient _client;
        private readonly AdsPushVapidSettings _adsPushVapidSettings;

        private readonly JsonSerializerSettings _jsonSerializerSettings = new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
            NullValueHandling = NullValueHandling.Ignore,
            DefaultValueHandling = DefaultValueHandling.Ignore
        };

        public VapidPushNotificationSender(
            AdsPushVapidSettings adsPushVapidSettings,
            HttpClient client)
        {
            this._client = client;
            this._adsPushVapidSettings = adsPushVapidSettings;
        }

        /// <inheritdoc />
        public Task<VapidResponse> SendAsync(
            VapidSubscription subscription,
            string payload,
            CancellationToken cancellationToken = default)
        {
            return this.SendBaseAsync(
                subscription,
                payload,
                cancellationToken);
        }

        /// <inheritdoc />
        public Task<VapidResponse> SendAsync(
            VapidSubscription subscription,
            VapidRequest payload,
            CancellationToken cancellationToken = default)
        {
            var jsonPayload = JsonConvert.SerializeObject(payload, this._jsonSerializerSettings);
            return this.SendBaseAsync(
                subscription,
                jsonPayload,
                cancellationToken);
        }

        /// <inheritdoc />
        public async Task SendAsync(
            string subscriptionJson,
            AdsPushBasicSendPayload payload,
            CancellationToken cancellationToken = default)
        {
            var subscription = VapidSubscription.FromSubscriptionJson(subscriptionJson);
            var vapidRequest = payload.CreateRequest();
            var jsonPayload = JsonConvert.SerializeObject(vapidRequest, this._jsonSerializerSettings);
            var result = await this.SendBaseAsync(
                subscription,
                jsonPayload,
                cancellationToken);

            if (!result.IsSuccess)
            {
                throw result.Error.CreateException();
            }
        }

        private async Task<HttpResponseMessage> SendHttpRequestAsync(
            VapidSubscription subscription,
            string payload,
            CancellationToken cancellationToken)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, subscription.Endpoint);
            var encryptedPayload = Encryptor.Encrypt(
                subscription.P256dh,
                subscription.Auth,
                payload);

            request.Content = new ByteArrayContent(encryptedPayload.Payload);
            request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            request.Content.Headers.ContentLength = encryptedPayload.Payload.Length;
            request.Content.Headers.ContentEncoding.Add("aesgcm");

            var uri = new Uri(subscription.Endpoint);
            var audience = uri.Scheme + @"://" + uri.Host;
            var vapidHeaders = VapidHelper.GetVapidHeaders(audience,
                this._adsPushVapidSettings.Subject,
                this._adsPushVapidSettings.PublicKey,
                this._adsPushVapidSettings.PrivateKey);

            var cryptoKeyHeader = @"dh=" + encryptedPayload.Base64EncodePublicKey() + @";" + vapidHeaders["Crypto-Key"];
            request.Headers.Add("Crypto-Key", cryptoKeyHeader);
            request.Headers.Add("Encryption", "salt=" + encryptedPayload.Base64EncodeSalt());
            request.Headers.Add("Authorization", vapidHeaders["Authorization"]);
            request.Headers.Add("TTL", this.GetTtl(payload).ToString());

            return await this._client.SendAsync(request, cancellationToken);
        }

        private async Task<VapidResponse> SendBaseAsync(
            VapidSubscription subscription,
            string jsonPayload,
            CancellationToken cancellationToken)
        {
            if (!this.ValidateSubscription(subscription))
            {
                return new VapidResponse(false, new VapidError(VapidErrorReasonCode.InvalidToken, null));
            }

            var response = await this.SendHttpRequestAsync(
                subscription,
                jsonPayload,
                cancellationToken);

            if (response.IsSuccessStatusCode)
            {
                return new VapidResponse(true, null);
            }

            var reasonCode = this.GetVapidErrorReasonCode(response);
            return new VapidResponse(false, new VapidError(reasonCode, response));
        }

        private long GetTtl(
            string jsonPayload)
        {
            var ttlString = JObject.Parse(jsonPayload)["TTL"]?.ToString();
            if (!string.IsNullOrEmpty(ttlString) && long.TryParse(ttlString, out var ttlLong) && ttlLong > 0)
            {
                return ttlLong;
            }

            return DefaultTtl;
        }

        private bool ValidateSubscription(
            VapidSubscription subscription)
        {
            return Uri.IsWellFormedUriString(subscription.Endpoint, UriKind.Absolute)
                   && !string.IsNullOrEmpty(subscription.P256dh)
                   && !string.IsNullOrEmpty(subscription.Auth);
        }

        private VapidErrorReasonCode GetVapidErrorReasonCode(
            HttpResponseMessage response)
        {
            switch (response.StatusCode)
            {
                case HttpStatusCode.BadRequest:
                    return VapidErrorReasonCode.InvalidArgument;
                case HttpStatusCode.Unauthorized:
                case HttpStatusCode.Forbidden:
                    return VapidErrorReasonCode.InvalidAuthConfiguration;

                case HttpStatusCode.NotFound:
                    return VapidErrorReasonCode.InvalidToken;

                case HttpStatusCode.ServiceUnavailable:
                    return VapidErrorReasonCode.ServiceUnavailable;
                default:
                    return VapidErrorReasonCode.UnknownError;
            }
        }
    }
}
