using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using Nexus.Core.Push.Abstraction;
using Nexus.Core.Push.Abstraction.Dataroid;
using Nexus.Modules.Proxy.Abstraction.Enums;

namespace Nexus.Core.Push.Dataroid.Extensions
{
    internal static class MappingExtensions
    {
        public static DataroidSendPushRequest CreateRequest(
            this NexusPushPayload payload,
            NexusPushTarget target,
            string customerId)
        {
            var request = new DataroidSendPushRequest()
            {
                Title = payload.Title.Text,
                Parameters = payload.Parameters.ToDictionary(x => x.Key, x => x.Value.ToString()),
                Text = payload.Detail.Text,
                ClientId = customerId,
                MediaUrl = payload.ImageUrl,
                ActionType = payload.ActionType switch
                {
                    NexusPushActionType.OpenApp => DataoridPushNotificationActionType.OPEN_APP,
                    NexusPushActionType.GoToUrl => DataoridPushNotificationActionType.GO_TO_URL,
                    NexusPushActionType.GoToDeepLink => DataoridPushNotificationActionType.GO_TO_DEEPLINK,
                    _ => throw new ArgumentOutOfRangeException()
                },
                ActionTargetUrl = payload.ActionUrl,
            };

            if (!string.IsNullOrEmpty(payload.Sound))
            {
                request.PlatformMap = new JObject()
                {
                    [target.ToString().ToUpper()] = new JObject() { ["sound"] = payload.Sound },
                };
            }

            return request;
        }

        public static NexusPushException CreateException(
            this DataroidSendPushErrorDetail errorDetail,
            HttpResponseMessage httpResponseMessage)
        {
            var errorType = NexusPushErrorType.Unknown;
            if (errorDetail.ErrorMessage.Contains("token"))
            {
                errorType = NexusPushErrorType.InvalidToken;
            }
            else if (httpResponseMessage.StatusCode is HttpStatusCode.InternalServerError or HttpStatusCode.ServiceUnavailable)
            {
                errorType = NexusPushErrorType.ServiceUnavailable;
            }
            else if (httpResponseMessage.StatusCode is HttpStatusCode.Unauthorized or HttpStatusCode.Forbidden)
            {
                errorType = NexusPushErrorType.InvalidAuthConfiguration;
            }

            return new NexusPushException(
                errorDetail.ErrorMessage,
                errorType,
                httpResponseMessage);
        }
    }
}
