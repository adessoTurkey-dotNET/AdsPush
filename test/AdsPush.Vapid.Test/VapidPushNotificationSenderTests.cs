using System.Net;
using AdsPush.Abstraction;
using AdsPush.Abstraction.Settings;
using AdsPush.Abstraction.Vapid;
using Moq;
using Moq.Protected;

namespace AdsPush.Vapid.Test;

public class VapidPushNotificationSenderTests
{
    private static AdsPushVapidSettings CreateSettings()
    {
        var keys = VapidHelper.GenerateVapidKeys();
        return new AdsPushVapidSettings
        {
            PublicKey = keys.PublicLey, PrivateKey = keys.PrivateKey, Subject = "mailto:test@example.com"
        };
    }

    private static VapidSubscription CreateValidSubscription(
        string endpoint)
    {
        var keys = VapidHelper.GenerateVapidKeys();
        var authBytes = new byte[]
        {
            0x41, 0x64, 0x73, 0x50, 0x75, 0x73, 0x68, 0x54, 0x65, 0x73, 0x74, 0x53, 0x65, 0x63, 0x72, 0x65
        };

        var auth = Convert.ToBase64String(authBytes)
            .Replace('+', '-')
            .Replace('/', '_')
            .TrimEnd('=');

        return VapidSubscription.FromParameters(endpoint, keys.PublicLey, auth);
    }

    [Fact]
    public async Task SendAsyncValidSubscriptionJsonPayloadUsesDefaultTtl()
    {
        var settings = CreateSettings();
        var subscription = CreateValidSubscription("https://example.com/push");
        var handlerMock = new Mock<HttpMessageHandler>();
        HttpRequestMessage? capturedRequest = null;

        handlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .Callback<HttpRequestMessage, CancellationToken>((request, _) => capturedRequest = request)
            .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK));

        using var httpClient = new HttpClient(handlerMock.Object);
        var sender = new VapidPushNotificationSender(settings, httpClient);

        var response = await sender.SendAsync(subscription, "{\"message\":\"hello\"}");

        Assert.True(response.IsSuccess);
        Assert.NotNull(capturedRequest);
        Assert.Equal("43200", capturedRequest!.Headers.GetValues("TTL").Single());

        handlerMock.Protected()
            .Verify(
                "SendAsync",
                Times.Once(),
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>());
    }

    [Fact]
    public async Task SendAsyncCustomTtlInPayloadUsesProvidedValue()
    {
        var settings = CreateSettings();
        var subscription = CreateValidSubscription("https://example.com/push");
        var handlerMock = new Mock<HttpMessageHandler>();
        HttpRequestMessage? capturedRequest = null;

        handlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .Callback<HttpRequestMessage, CancellationToken>((request, _) => capturedRequest = request)
            .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK));

        using var httpClient = new HttpClient(handlerMock.Object);
        var sender = new VapidPushNotificationSender(settings, httpClient);

        var response = await sender.SendAsync(subscription, "{\"TTL\":60,\"message\":\"hello\"}");

        Assert.True(response.IsSuccess);
        Assert.NotNull(capturedRequest);
        Assert.Equal("60", capturedRequest!.Headers.GetValues("TTL").Single());

        handlerMock.Protected()
            .Verify(
                "SendAsync",
                Times.Once(),
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>());
    }

    [Fact]
    public async Task SendAsyncInvalidSubscriptionReturnsInvalidTokenError()
    {
        var settings = CreateSettings();
        var handlerMock = new Mock<HttpMessageHandler>();

        using var httpClient = new HttpClient(handlerMock.Object);
        var sender = new VapidPushNotificationSender(settings, httpClient);
        var invalidSubscription = VapidSubscription.FromParameters("invalid-endpoint", string.Empty, string.Empty);

        var response = await sender.SendAsync(invalidSubscription, "{}");

        Assert.False(response.IsSuccess);
        Assert.NotNull(response.Error);
        Assert.Equal(VapidErrorReasonCode.InvalidToken, response.Error.ReasonCode);

        handlerMock.Protected()
            .Verify(
                "SendAsync",
                Times.Never(),
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>());
    }

    [Fact]
    public async Task SendAsyncSubscriptionJsonFailureThrowsAdsPushException()
    {
        var settings = CreateSettings();
        var subscription = CreateValidSubscription("https://example.com/failure");
        var subscriptionJson = subscription.ToAdsPushToken();

        var handlerMock = new Mock<HttpMessageHandler>();
        handlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.Unauthorized));

        using var httpClient = new HttpClient(handlerMock.Object);
        var sender = new VapidPushNotificationSender(settings, httpClient);

        var payload = new AdsPushBasicSendPayload
        {
            Title = AdsPushText.CreateUsingString("title"), Detail = AdsPushText.CreateUsingString("detail")
        };
        payload.Parameters["traceId"] = "abc123";

        var exception = await Assert.ThrowsAsync<AdsPushException>(() => sender.SendAsync(subscriptionJson, payload));

        Assert.Equal(AdsPushErrorType.InvalidAuthConfiguration, exception.ErrorType);

        handlerMock.Protected()
            .Verify(
                "SendAsync",
                Times.Once(),
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>());
    }

    [Fact]
    public async Task SendAsyncSubscriptionJsonSuccessCompletesWithoutException()
    {
        var settings = CreateSettings();
        var subscription = CreateValidSubscription("https://example.com/success");
        var subscriptionJson = subscription.ToAdsPushToken();

        var handlerMock = new Mock<HttpMessageHandler>();
        handlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK));

        using var httpClient = new HttpClient(handlerMock.Object);
        var sender = new VapidPushNotificationSender(settings, httpClient);

        var payload = new AdsPushBasicSendPayload
        {
            Title = AdsPushText.CreateUsingString("title"), Detail = AdsPushText.CreateUsingString("detail")
        };

        await sender.SendAsync(subscriptionJson, payload);

        handlerMock.Protected()
            .Verify(
                "SendAsync",
                Times.Once(),
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>());
    }

    [Fact]
    public async Task SendAsyncZeroTtlFallsBackToDefault()
    {
        var settings = CreateSettings();
        var subscription = CreateValidSubscription("https://example.com/push");
        var handlerMock = new Mock<HttpMessageHandler>();
        HttpRequestMessage? capturedRequest = null;

        handlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .Callback<HttpRequestMessage, CancellationToken>((request, _) => capturedRequest = request)
            .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK));

        using var httpClient = new HttpClient(handlerMock.Object);
        var sender = new VapidPushNotificationSender(settings, httpClient);

        var response = await sender.SendAsync(subscription, "{\"TTL\":0,\"message\":\"hello\"}");

        Assert.True(response.IsSuccess);
        Assert.NotNull(capturedRequest);
        Assert.Equal("43200", capturedRequest!.Headers.GetValues("TTL").Single());
    }

    [Fact]
    public async Task SendAsyncWithRequestPayloadAppliesTtl()
    {
        var settings = CreateSettings();
        var subscription = CreateValidSubscription("https://example.com/push");
        var handlerMock = new Mock<HttpMessageHandler>();
        HttpRequestMessage? capturedRequest = null;

        handlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .Callback<HttpRequestMessage, CancellationToken>((request, _) => capturedRequest = request)
            .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK));

        using var httpClient = new HttpClient(handlerMock.Object);
        var sender = new VapidPushNotificationSender(settings, httpClient);

        var request = new VapidRequest { Title = "Sample", Message = "Body", TTL = 75 };

        var response = await sender.SendAsync(subscription, request);

        Assert.True(response.IsSuccess);
        Assert.NotNull(capturedRequest);
        Assert.Equal("43200", capturedRequest!.Headers.GetValues("TTL").Single());
    }

    public static TheoryData<HttpStatusCode, VapidErrorReasonCode> FailureReasonMappings()
    {
        return new TheoryData<HttpStatusCode, VapidErrorReasonCode>
        {
            { HttpStatusCode.BadRequest, VapidErrorReasonCode.InvalidArgument },
            { HttpStatusCode.Unauthorized, VapidErrorReasonCode.InvalidAuthConfiguration },
            { HttpStatusCode.Forbidden, VapidErrorReasonCode.InvalidAuthConfiguration },
            { HttpStatusCode.NotFound, VapidErrorReasonCode.InvalidToken },
            { HttpStatusCode.ServiceUnavailable, VapidErrorReasonCode.ServiceUnavailable },
            { HttpStatusCode.InternalServerError, VapidErrorReasonCode.UnknownError }
        };
    }

    [Theory]
    [MemberData(nameof(FailureReasonMappings))]
    public async Task SendAsyncMapsHttpStatusToVapidError(
        HttpStatusCode statusCode,
        VapidErrorReasonCode expectedReason)
    {
        var settings = CreateSettings();
        var subscription = CreateValidSubscription("https://example.com/push");
        var handlerMock = new Mock<HttpMessageHandler>();

        handlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage(statusCode));

        using var httpClient = new HttpClient(handlerMock.Object);
        var sender = new VapidPushNotificationSender(settings, httpClient);

        var response = await sender.SendAsync(subscription, "{}");

        Assert.False(response.IsSuccess);
        Assert.Equal(expectedReason, response.Error!.ReasonCode);
    }
}
