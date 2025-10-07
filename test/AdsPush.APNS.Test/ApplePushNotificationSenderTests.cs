using System.Net;
using AdsPush.Abstraction;
using AdsPush.Abstraction.APNS;
using Moq;
using Moq.Protected;

namespace AdsPush.APNS.Test;

public class ApplePushNotificationSenderTests
{
    [Fact]
    public void ConstructorThrowsWhenSettingsNull()
    {
        var httpClient = new HttpClient(new HttpClientHandler());

        Assert.Throws<ArgumentNullException>(() => new ApplePushNotificationSender(null!, httpClient));
    }

    [Fact]
    public void ConstructorThrowsWhenHttpClientNull()
    {
        var settings = ApnsTestHelper.CreateSettings();

        Assert.Throws<ArgumentNullException>(() => new ApplePushNotificationSender(settings, null!));
    }

    [Fact]
    public async Task SendAsyncWithJsonPayloadPopulatesRequestHeaders()
    {
        ApnsTestHelper.ClearJwtTokenCache();
        var settings = ApnsTestHelper.CreateSettings();
        var handlerMock = new Mock<HttpMessageHandler>();
        HttpRequestMessage? capturedRequest = null;

        handlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .Callback<HttpRequestMessage, CancellationToken>((request, _) => capturedRequest = request)
            .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent("null") });

        using var httpClient = new HttpClient(handlerMock.Object);
        var sender = new ApplePushNotificationSender(settings, httpClient);

        var apnsId = Guid.NewGuid();
        var expiration = APNSExpiration.FromTimeSpan(TimeSpan.FromMinutes(10));
        var response = await sender.SendAsync(
            "{\"aps\":{\"alert\":\"hi\"}}",
            "device-token",
            apnsId,
            expiration,
            apnsPriority: 5,
            isBackground: true);

        Assert.True(response.IsSuccess);
        Assert.Null(response.Error);
        var request = Assert.IsType<HttpRequestMessage>(capturedRequest);
        Assert.Equal(new Version(2, 0), request.Version);
        Assert.Equal("/3/device/device-token", request.RequestUri!.PathAndQuery);
        Assert.Equal("POST", request.Method.Method);
        Assert.Equal("device-token", request.RequestUri.AbsolutePath.Split('/').Last());
        Assert.Equal("bearer", request.Headers.Authorization!.Scheme, ignoreCase: true);
        Assert.False(string.IsNullOrEmpty(request.Headers.Authorization.Parameter));
        Assert.Equal(settings.AppBundleIdentifier, request.Headers.GetValues("apns-topic").Single());
        Assert.Equal(expiration.ApnsExpirationValue.ToString(), request.Headers.GetValues("apns-expiration").Single());
        Assert.Equal("5", request.Headers.GetValues("apns-priority").Single());
        Assert.Equal("background", request.Headers.GetValues("apns-push-type").Single());
        Assert.Equal(apnsId.ToString(), request.Headers.GetValues("apns-id").Single());
        Assert.NotNull(request.Content);
        Assert.Equal("{\"aps\":{\"alert\":\"hi\"}}", await request.Content!.ReadAsStringAsync());
    }

    [Fact]
    public async Task SendAsyncWhenServerReturnsErrorReturnsResponseWithError()
    {
        ApnsTestHelper.ClearJwtTokenCache();
        var settings = ApnsTestHelper.CreateSettings();
        var handlerMock = new Mock<HttpMessageHandler>();

        handlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                Content = new StringContent("{\"reason\":\"BadDeviceToken\"}")
            });

        using var httpClient = new HttpClient(handlerMock.Object);
        var sender = new ApplePushNotificationSender(settings, httpClient);

        var response = await sender.SendAsync(
            "{\"aps\":{\"alert\":\"hi\"}}",
            "device-token",
            Guid.NewGuid());

        Assert.False(response.IsSuccess);
        Assert.NotNull(response.Error);
        Assert.Equal(APNSErrorReasonCode.BadDeviceToken, response.Error!.Reason);
        Assert.Equal(HttpStatusCode.BadRequest, response.Error.HttpResponse!.StatusCode);
    }

    [Fact]
    public async Task SendAsyncWithBasicPayloadFailureThrowsAdsPushException()
    {
        ApnsTestHelper.ClearJwtTokenCache();
        var settings = ApnsTestHelper.CreateSettings();
        var handlerMock = new Mock<HttpMessageHandler>();

        handlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                Content = new StringContent("{\"reason\":\"BadDeviceToken\"}")
            });

        using var httpClient = new HttpClient(handlerMock.Object);
        var sender = new ApplePushNotificationSender(settings, httpClient);

        var payload = new AdsPushBasicSendPayload
        {
            Title = AdsPushText.CreateUsingString("Title"),
            Detail = AdsPushText.CreateUsingString("Body"),
            PushType = AdsPushType.Alert
        };
        payload.Parameters["foo"] = "bar";

        var ex = await Assert.ThrowsAsync<AdsPushException>(() => sender.SendAsync("device-token", payload));
        Assert.Equal(AdsPushErrorType.InvalidToken, ex.ErrorType);
    }

    [Fact]
    public async Task SendAsyncWithBasicPayloadSuccessDoesNotThrow()
    {
        ApnsTestHelper.ClearJwtTokenCache();
        var settings = ApnsTestHelper.CreateSettings();
        var handlerMock = new Mock<HttpMessageHandler>();

        handlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent("null") });

        using var httpClient = new HttpClient(handlerMock.Object);
        var sender = new ApplePushNotificationSender(settings, httpClient);

        var payload = new AdsPushBasicSendPayload
        {
            Title = AdsPushText.CreateUsingString("Title"),
            Detail = AdsPushText.CreateUsingString("Body"),
            PushType = AdsPushType.Alert
        };

        await sender.SendAsync("device-token", payload);
    }

    [Fact]
    public async Task SendAsyncWithApnsRequestIncludesAdditionalParameters()
    {
        ApnsTestHelper.ClearJwtTokenCache();
        var settings = ApnsTestHelper.CreateSettings();
        var handlerMock = new Mock<HttpMessageHandler>();
        HttpRequestMessage? capturedRequest = null;

        handlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .Callback<HttpRequestMessage, CancellationToken>((request, _) => capturedRequest = request)
            .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent("null") });

        using var httpClient = new HttpClient(handlerMock.Object);
        var sender = new ApplePushNotificationSender(settings, httpClient);

        var apnsRequest = new APNSRequest
        {
            ApnsPayload = new APNSPayload { Alert = new APNSAlert { Title = "title" } },
            AdditionalParameters = new Dictionary<string, object> { ["number"] = 5 }
        };

        await sender.SendAsync(apnsRequest, "device", Guid.NewGuid());

        Assert.NotNull(capturedRequest);
        var body = await capturedRequest!.Content!.ReadAsStringAsync();
        Assert.Contains("\"number\":5", body);
    }

    [Fact]
    public async Task SendAsyncWithObjectPayloadSerializesAnonymousType()
    {
        ApnsTestHelper.ClearJwtTokenCache();
        var settings = ApnsTestHelper.CreateSettings();
        var handlerMock = new Mock<HttpMessageHandler>();
        HttpRequestMessage? capturedRequest = null;

        handlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .Callback<HttpRequestMessage, CancellationToken>((request, _) => capturedRequest = request)
            .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent("null") });

        using var httpClient = new HttpClient(handlerMock.Object);
        var sender = new ApplePushNotificationSender(settings, httpClient);

        await sender.SendAsync(new { foo = "bar" }, "device", Guid.NewGuid());

        Assert.NotNull(capturedRequest);
        var body = await capturedRequest!.Content!.ReadAsStringAsync();
        Assert.Contains("\"foo\":\"bar\"", body);
    }

    [Fact]
    public async Task GetJwtTokenRemovesExpiredCacheEntry()
    {
        ApnsTestHelper.ClearJwtTokenCache();
        var settings = ApnsTestHelper.CreateSettings(appBundleIdentifier: "com.example.expired");
        ApnsTestHelper.SetCachedToken(settings.AppBundleIdentifier, "expired-token", DateTime.UtcNow.AddMinutes(-60));

        var handlerMock = new Mock<HttpMessageHandler>();
        HttpRequestMessage? capturedRequest = null;
        handlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .Callback<HttpRequestMessage, CancellationToken>((request, _) => capturedRequest = request)
            .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent("null") });

        using var httpClient = new HttpClient(handlerMock.Object);
        var sender = new ApplePushNotificationSender(settings, httpClient);

        await sender.SendAsync("{}", "device", Guid.NewGuid());

        Assert.NotNull(capturedRequest);
        Assert.NotEqual("expired-token", capturedRequest!.Headers.Authorization!.Parameter);
    }

    [Fact]
    public void CleanP8KeyRemovesPemHeaders()
    {
        var original = "-----BEGIN PRIVATE KEY-----\nABCDEF\n-----END PRIVATE KEY-----";
        var method = typeof(ApplePushNotificationSender).GetMethod("CleanP8Key",
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
        var cleaned = (string)method!.Invoke(null, new object[] { original })!;

        Assert.Equal("ABCDEF", cleaned);
    }

    [Fact]
    public void CleanP8KeyReturnsOriginalWhenEmpty()
    {
        var method = typeof(ApplePushNotificationSender).GetMethod("CleanP8Key",
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
        var cleaned = (string)method!.Invoke(null, new object[] { string.Empty })!;

        Assert.Equal(string.Empty, cleaned);
    }

    [Fact]
    public void CleanP8KeyKeepsContentWithoutHeaders()
    {
        var method = typeof(ApplePushNotificationSender).GetMethod("CleanP8Key",
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
        var input = "LINE1\nLINE2";
        var cleaned = (string)method!.Invoke(null, new object[] { input })!;

        Assert.Equal("LINE1LINE2", cleaned);
    }

    [Fact]
    public void CleanP8KeyRemovesHeaderWithoutBody()
    {
        var method = typeof(ApplePushNotificationSender).GetMethod("CleanP8Key",
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
        var input = "-----BEGIN PRIVATE KEY-----";
        var cleaned = (string)method!.Invoke(null, new object[] { input })!;

        Assert.Equal(string.Empty, cleaned);
    }
}
