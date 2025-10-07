using System.Net;
using AdsPush.Abstraction;
using AdsPush.Abstraction.Vapid;
using AdsPush.Vapid.Extensions;

namespace AdsPush.Vapid.Test;

public class MappingExtensionsTests
{
    [Fact]
    public void CreateRequestMapsAllFields()
    {
        var payload = new AdsPushBasicSendPayload
        {
            PushType = AdsPushType.Background,
            Title = AdsPushText.CreateUsingString("title"),
            Detail = AdsPushText.CreateUsingString("detail"),
            Sound = "sound.mp3",
            GroupId = "group",
            Badge = 2,
            Ttl = System.TimeSpan.FromMinutes(5)
        };

        payload.Parameters["number"] = 5;
        payload.Parameters["state"] = "ok";

        var request = payload.CreateRequest();

        Assert.Equal("title", request.Title);
        Assert.Equal("detail", request.Message);
        Assert.Equal("group", request.Tag);
        Assert.Equal("sound.mp3", request.Sound);
        Assert.True(request.Silent);
        Assert.Equal(300, request.TTL);
        Assert.Equal("5", request.Data["number"]);
        Assert.Equal("ok", request.Data["state"]);
    }

    public static IEnumerable<object[]> ExceptionMappings()
    {
        yield return new object[] { VapidErrorReasonCode.UnknownError, AdsPushErrorType.Unknown };
        yield return new object[] { VapidErrorReasonCode.InvalidToken, AdsPushErrorType.InvalidToken };
        yield return new object[] { VapidErrorReasonCode.ServiceUnavailable, AdsPushErrorType.ServiceUnavailable };
        yield return new object[] { VapidErrorReasonCode.InvalidArgument, AdsPushErrorType.InvalidArgument };
        yield return new object[]
        {
            VapidErrorReasonCode.InvalidAuthConfiguration, AdsPushErrorType.InvalidAuthConfiguration
        };
        yield return new object[] { (VapidErrorReasonCode)999, AdsPushErrorType.Unknown };
    }

    [Theory]
    [MemberData(nameof(ExceptionMappings))]
    public void CreateExceptionMapsErrorType(
        VapidErrorReasonCode reason,
        AdsPushErrorType expectedErrorType)
    {
        using var response = new HttpResponseMessage(HttpStatusCode.BadRequest);
        var vapidError = new VapidError(reason, response);
        var exception = vapidError.CreateException();

        Assert.Equal(expectedErrorType, exception.ErrorType);
        Assert.Same(response, exception.HttpResponse);
    }
}
