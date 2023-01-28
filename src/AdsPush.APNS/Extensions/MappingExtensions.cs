using System;
using System.Collections.Generic;
using AdsPush.Abstraction;
using AdsPush.Abstraction.APNS;

namespace AdsPush.APNS.Extensions
{
    public static class MappingExtensions
    {
        public static APNSRequest CreateRequest(
            this AdsPushPayload payload)
        {
            payload.Parameters ??= new Dictionary<string, object>();
          
            return new APNSRequest()
            {
                AdditionalParameters = payload.Parameters,
                ApnsPayload = new APNSPayload()
                {
                    Badge = payload.Badge,
                    Sound = payload.Sound,
                    ThreadId = payload.GroupId,
                    MutableContent = true,
                    ContentAvailable = true,
                    Alert = new APNSAlert()
                    {
                        Title = payload.Title.Text,
                        TitleLocationKey = payload.Title.LocalizationKey,
                        TitleLocationKeyArgs = payload.Title.LocalizationArgs,
                        Body = payload.Detail.Text,
                        BodyLocationKey = payload.Detail.LocalizationKey,
                        BodyLocationKeyArgs = payload.Detail.LocalizationArgs,
                    },
                    PushType = payload.PushType.ToString().ToLower(),
                }
            };
        }

        public static AdsPushException CreateException(
            this APNSError error)
        {
            switch (error.Reason)
                {
                    case APNSErrorReasonCode.BadDeviceToken:
                    case APNSErrorReasonCode.Unregistered:
                        return new AdsPushException(
                            error.Reason.ToString(),
                            AdsPushErrorType.InvalidToken,
                            error.HttpResponse);

                    case APNSErrorReasonCode.BadCollapseId:
                    case APNSErrorReasonCode.BadExpirationDate:
                    case APNSErrorReasonCode.BadMessageId:
                    case APNSErrorReasonCode.BadPriority:
                    case APNSErrorReasonCode.BadTopic:
                    case APNSErrorReasonCode.DeviceTokenNotForTopic:
                    case APNSErrorReasonCode.MissingDeviceToken:
                    case APNSErrorReasonCode.MissingTopic:
                    case APNSErrorReasonCode.PayloadEmpty:
                    case APNSErrorReasonCode.TopicDisallowed:
                        return new AdsPushException(
                            error.Reason.ToString(),
                            AdsPushErrorType.InvalidArgument,
                            error.HttpResponse);

                    case APNSErrorReasonCode.BadCertificate:
                    case APNSErrorReasonCode.BadCertificateEnvironment:
                    case APNSErrorReasonCode.Forbidden:
                    case APNSErrorReasonCode.MethodNotAllowed:
                        return new AdsPushException(
                            error.Reason.ToString(),
                            AdsPushErrorType.InvalidAuthConfiguration,
                            error.HttpResponse);

                    case APNSErrorReasonCode.InternalServerError:
                    case APNSErrorReasonCode.ServiceUnavailable:
                    case APNSErrorReasonCode.Shutdown:
                    case APNSErrorReasonCode.ExpiredProviderToken:
                    case APNSErrorReasonCode.InvalidProviderToken:
                    case APNSErrorReasonCode.MissingProviderToken:
                    case APNSErrorReasonCode.BadPath:
                    case APNSErrorReasonCode.PayloadTooLarge:
                    case APNSErrorReasonCode.TooManyProviderTokenUpdates:
                    case APNSErrorReasonCode.TooManyRequests:
                    case APNSErrorReasonCode.DuplicateHeaders:
                    case APNSErrorReasonCode.IdleTimeout:
                        return new AdsPushException(
                            error.Reason.ToString(),
                            AdsPushErrorType.ServiceUnavailable,
                            error.HttpResponse);

                    default:
                        throw new ArgumentOutOfRangeException();
                }
        }
    }
}
