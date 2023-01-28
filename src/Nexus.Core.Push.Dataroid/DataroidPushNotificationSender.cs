using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Nexus.Core.Push.Abstraction;
using Nexus.Core.Push.Abstraction.Dataroid;
using Nexus.Core.Push.Abstraction.Settings;
using Nexus.Core.Push.Dataroid.Extensions;

namespace Nexus.Core.Push.Dataroid
{
    public class DataroidPushNotificationSender : IDataroidPushNotificationSender
    {
        private readonly NexusPushDataroidSettings _dataroidSettings;
        private readonly HttpClient _httpClient;

        public DataroidPushNotificationSender(
            NexusPushDataroidSettings dataroidSettings,
            HttpClient httpClient)
        {
            this._dataroidSettings = dataroidSettings;
            this._httpClient = httpClient;
        }

        public async Task SendAsync(
            NexusPushTarget target,
            string customerId,
            NexusPushPayload payload,
            CancellationToken cancellationToken = default)
        {
            using var message = new HttpRequestMessage(HttpMethod.Post, this._dataroidSettings.SendServerUrl);
            var jsonContent = JsonConvert.SerializeObject(payload.CreateRequest(target,customerId));
            message.Content = new StringContent(jsonContent,Encoding.Default,"application/json");
            message.Headers.Add("x-appconnect-api-key", this._dataroidSettings.ApiKey);
            message.Headers.Add("x-appconnect-app-id", this._dataroidSettings.AppId);


            var response = await this._httpClient.SendAsync(message, cancellationToken);
            var responseModel = JsonConvert.DeserializeObject<DataroidSendPushResponse>(await response.Content.ReadAsStringAsync(cancellationToken));
            if (responseModel is null)
            {
                throw new NexusPushException(
                    response.StatusCode.ToString(),
                    NexusPushErrorType.Unknown,
                    response);
            }

            if (!responseModel.Success)
            {
                throw responseModel.Error.CreateException(response);
            }
        }
    }
}
