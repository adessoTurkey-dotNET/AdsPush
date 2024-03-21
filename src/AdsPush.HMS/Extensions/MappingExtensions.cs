using System;
using AdsPush.Abstraction;
using AdsPush.Abstraction.HMS;

namespace AdsPush.HMS.Extensions
{
    public static class MappingExtensions
    {
        public static HMSRequest CreateRequest(
            this AdsPushBasicSendPayload payload)
        {
            return new HMSRequest()
            {
                HMSPayload = new HMSPayload
                {
                    PushToken = payload.PushToken,
                    Notification = new HMSNotification
                    {
                        Title = payload.Title.Text,
                        Body = payload.Detail.Text
                    }
                }
            };
        }

        // BU EXCECPTION NEYE GÖRE KARAR VERILECEK?
        public static AdsPushException CreateException(
           this HMSError error)
        {
            switch (error.Reason)
            {
                case HMSErrorReasonCode.FailedIllegalTokens:
                    return new AdsPushException(
                       error.Reason.ToString(),
                       AdsPushErrorType.InvalidArgument,
                       error.HttpResponse);
                case HMSErrorReasonCode.UnsupportedRequestParameters:
                case HMSErrorReasonCode.BadMessageStructure:
                case HMSErrorReasonCode.BadExpiryTime:
                case HMSErrorReasonCode.BadCollapseKey:
                    return new AdsPushException(
                       error.Reason.ToString(),
                       AdsPushErrorType.InvalidArgument,
                       error.HttpResponse);
                case HMSErrorReasonCode.TopicOverload:
                case HMSErrorReasonCode.FailedOAuth:
                    return new AdsPushException(
                       error.Reason.ToString(),
                       AdsPushErrorType.InvalidToken,
                       error.HttpResponse);
                case HMSErrorReasonCode.ExpiredOAuthToken:
                    return new AdsPushException(
                       error.Reason.ToString(),
                       AdsPushErrorType.InvalidToken,
                       error.HttpResponse);
                case HMSErrorReasonCode.NotPermitted:
                    return new AdsPushException(
                       error.Reason.ToString(),
                       AdsPushErrorType.InvalidToken,
                       error.HttpResponse);
                case HMSErrorReasonCode.FailedAllTokens:
                case HMSErrorReasonCode.BodyOverlaod:
                case HMSErrorReasonCode.TokenCountOverload:
                case HMSErrorReasonCode.InvalidReceiptUrl:
                    return new AdsPushException(
                       error.Reason.ToString(),
                       AdsPushErrorType.InvalidArgument,
                       error.HttpResponse);
                case HMSErrorReasonCode.FailedOAuthRequest:
                case HMSErrorReasonCode.InternalError:

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
