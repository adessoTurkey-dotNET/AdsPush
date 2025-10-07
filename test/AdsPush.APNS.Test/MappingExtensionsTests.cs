using AdsPush.Abstraction;
using AdsPush.Abstraction.APNS;
using AdsPush.APNS.Extensions;

namespace AdsPush.APNS.Test;

public class MappingExtensionsTests
{
    [Fact]
    public void CreateRequestMapsPayloadProperties()
    {
        var payload = new AdsPushBasicSendPayload
        {
            PushType = AdsPushType.Background,
            Badge = 5,
            Sound = "sound.caf",
            GroupId = "thread",
            Title = AdsPushText.CreateUsingString("title"),
            Detail = AdsPushText.CreateUsingString("body"),
            Parameters = new Dictionary<string, object> { ["traceId"] = 123, ["flag"] = true }
        };

        var request = payload.CreateRequest();

        Assert.Equal(payload.Parameters, request.AdditionalParameters);
        Assert.Equal(payload.Badge, request.ApnsPayload.Badge);
        Assert.Equal(payload.Sound, request.ApnsPayload.Sound);
        Assert.Equal(payload.GroupId, request.ApnsPayload.ThreadId);
        Assert.True(request.ApnsPayload.MutableContent);
        Assert.True(request.ApnsPayload.ContentAvailable);
        Assert.Equal("background", request.ApnsPayload.PushType);
        Assert.Equal("title", request.ApnsPayload.Alert.Title);
        Assert.Equal("body", request.ApnsPayload.Alert.Body);
    }

    public static TheoryData<APNSErrorReasonCode, AdsPushErrorType> ErrorMappings => new()
    {
        { APNSErrorReasonCode.BadDeviceToken, AdsPushErrorType.InvalidToken },
        { APNSErrorReasonCode.Unregistered, AdsPushErrorType.InvalidToken },
        { APNSErrorReasonCode.BadCollapseId, AdsPushErrorType.InvalidArgument },
        { APNSErrorReasonCode.BadExpirationDate, AdsPushErrorType.InvalidArgument },
        { APNSErrorReasonCode.BadMessageId, AdsPushErrorType.InvalidArgument },
        { APNSErrorReasonCode.BadPriority, AdsPushErrorType.InvalidArgument },
        { APNSErrorReasonCode.BadTopic, AdsPushErrorType.InvalidArgument },
        { APNSErrorReasonCode.DeviceTokenNotForTopic, AdsPushErrorType.InvalidArgument },
        { APNSErrorReasonCode.MissingDeviceToken, AdsPushErrorType.InvalidArgument },
        { APNSErrorReasonCode.MissingTopic, AdsPushErrorType.InvalidArgument },
        { APNSErrorReasonCode.PayloadEmpty, AdsPushErrorType.InvalidArgument },
        { APNSErrorReasonCode.TopicDisallowed, AdsPushErrorType.InvalidArgument },
        { APNSErrorReasonCode.BadCertificate, AdsPushErrorType.InvalidAuthConfiguration },
        { APNSErrorReasonCode.BadCertificateEnvironment, AdsPushErrorType.InvalidAuthConfiguration },
        { APNSErrorReasonCode.Forbidden, AdsPushErrorType.InvalidAuthConfiguration },
        { APNSErrorReasonCode.MethodNotAllowed, AdsPushErrorType.InvalidAuthConfiguration },
        { APNSErrorReasonCode.InternalServerError, AdsPushErrorType.ServiceUnavailable },
        { APNSErrorReasonCode.ServiceUnavailable, AdsPushErrorType.ServiceUnavailable },
        { APNSErrorReasonCode.Shutdown, AdsPushErrorType.ServiceUnavailable },
        { APNSErrorReasonCode.ExpiredProviderToken, AdsPushErrorType.ServiceUnavailable },
        { APNSErrorReasonCode.InvalidProviderToken, AdsPushErrorType.ServiceUnavailable },
        { APNSErrorReasonCode.MissingProviderToken, AdsPushErrorType.ServiceUnavailable },
        { APNSErrorReasonCode.BadPath, AdsPushErrorType.ServiceUnavailable },
        { APNSErrorReasonCode.PayloadTooLarge, AdsPushErrorType.ServiceUnavailable },
        { APNSErrorReasonCode.TooManyProviderTokenUpdates, AdsPushErrorType.ServiceUnavailable },
        { APNSErrorReasonCode.TooManyRequests, AdsPushErrorType.ServiceUnavailable },
        { APNSErrorReasonCode.DuplicateHeaders, AdsPushErrorType.ServiceUnavailable },
        { APNSErrorReasonCode.IdleTimeout, AdsPushErrorType.ServiceUnavailable }
    };

    [Theory]
    [MemberData(nameof(ErrorMappings))]
    public void CreateExceptionMapsErrorReason(
        APNSErrorReasonCode reason,
        AdsPushErrorType expectedErrorType)
    {
        using var response = new HttpResponseMessage();
        var error = new APNSError { Reason = reason, HttpResponse = response };

        var exception = error.CreateException();

        Assert.Equal(expectedErrorType, exception.ErrorType);
        Assert.Same(response, exception.HttpResponse);
    }

    [Fact]
    public void CreateExceptionUnknownReasonThrows()
    {
        using var response = new HttpResponseMessage();
        var error = new APNSError { Reason = (APNSErrorReasonCode)999, HttpResponse = response };

        Assert.Throws<System.ArgumentOutOfRangeException>(() => error.CreateException());
    }

    [Fact]
    public void CreateRequestInitializesParametersWhenNull()
    {
        var payload = new AdsPushBasicSendPayload
        {
            Parameters = null,
            Title = AdsPushText.CreateUsingString("Title"),
            Detail = AdsPushText.CreateUsingString("Body")
        };

        var request = payload.CreateRequest();

        Assert.NotNull(request.AdditionalParameters);
        Assert.Empty(request.AdditionalParameters);
    }
}
