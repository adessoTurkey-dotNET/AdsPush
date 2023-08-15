using System.Linq;
using AdsPush.Abstraction;
using AdsPush.Abstraction.Vapid;

namespace AdsPush.Vapid.Extensions
{
    public static class MappingExtensions
    {
        public static AdsPushException CreateException(
            this VapidError error)
        {
            switch (error.ReasonCode)
            {
                case VapidErrorReasonCode.UnknownError:
                    return new AdsPushException(
                        error.ReasonCode.ToString(),
                        AdsPushErrorType.Unknown,
                        error.HttpResponse);
                case VapidErrorReasonCode.InvalidToken:
                    return new AdsPushException(
                        error.ReasonCode.ToString(),
                        AdsPushErrorType.InvalidToken,
                        error.HttpResponse);
                case VapidErrorReasonCode.ServiceUnavailable:
                    return new AdsPushException(
                        error.ReasonCode.ToString(),
                        AdsPushErrorType.ServiceUnavailable,
                        error.HttpResponse);
                case VapidErrorReasonCode.InvalidArgument:
                    return new AdsPushException(
                        error.ReasonCode.ToString(),
                        AdsPushErrorType.InvalidArgument,
                        error.HttpResponse);
                case VapidErrorReasonCode.InvalidAuthConfiguration:
                    return new AdsPushException(
                        error.ReasonCode.ToString(),
                        AdsPushErrorType.InvalidAuthConfiguration,
                        error.HttpResponse);
                default:
                    return new AdsPushException(
                        error.ReasonCode.ToString(),
                        AdsPushErrorType.Unknown,
                        error.HttpResponse);
            }
        }

        public static VapidRequest CreateRequest(
            this AdsPushBasicSendPayload payload)
        {
            return new VapidRequest()
            {
                Title = payload.Title.Text,
                Message = payload.Detail.Text,
                Tag = payload.GroupId,
                Sound = payload.Sound,
                Data = payload.Parameters.ToDictionary(x=>x.Key, x=>x.Value.ToString()),
            };
        }
    }
}
