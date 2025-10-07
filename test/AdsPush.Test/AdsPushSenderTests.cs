using AdsPush.Abstraction;
using AdsPush.Abstraction.Settings;
using AdsPush.APNS;
using AdsPush.Firebase;
using AdsPush.Vapid;
using Moq;

namespace AdsPush.Test;

public class AdsPushSenderTests
{
    private static AdsPushBasicSendPayload CreatePayload() =>
        new AdsPushBasicSendPayload
        {
            Title = AdsPushText.CreateUsingString("title"), Detail = AdsPushText.CreateUsingString("body")
        };

    [Fact]
    public async Task BasicSendAsyncThrowsWhenTargetNotConfigured()
    {
        var settings = new AdsPushAppSettings();
        var configurationProvider = new Mock<IAdsPushConfigurationProvider>();
        configurationProvider.Setup(p => p.GetSettingsAsync("app", It.IsAny<CancellationToken>()))
            .ReturnsAsync(settings);

        var sender = new AdsPushSender(
            "app",
            configurationProvider.Object,
            Mock.Of<IFirebasePushNotificationSenderFactory>(),
            Mock.Of<IApplePushNotificationSenderFactory>(),
            Mock.Of<IVapidPushNotificationSenderFactory>());

        var exception = await Assert.ThrowsAsync<AdsPushException>(() =>
            sender.BasicSendAsync(AdsPushTarget.Ios, "token", CreatePayload()));

        Assert.Equal(AdsPushErrorType.InvalidAuthConfiguration, exception.ErrorType);
    }

    [Fact]
    public async Task BasicSendAsyncThrowsWhenApnsTargetInvalid()
    {
        var settings = new AdsPushAppSettings();
        settings.TargetMappings[AdsPushTarget.Android] = AdsPushProvider.Apns;
        settings.Apns = new AdsPushAPNSSettings();

        var configurationProvider = new Mock<IAdsPushConfigurationProvider>();
        configurationProvider.Setup(p => p.GetSettingsAsync("app", It.IsAny<CancellationToken>()))
            .ReturnsAsync(settings);

        var sender = new AdsPushSender(
            "app",
            configurationProvider.Object,
            Mock.Of<IFirebasePushNotificationSenderFactory>(),
            Mock.Of<IApplePushNotificationSenderFactory>(),
            Mock.Of<IVapidPushNotificationSenderFactory>());

        var exception = await Assert.ThrowsAsync<AdsPushException>(() =>
            sender.BasicSendAsync(AdsPushTarget.Android, "token", CreatePayload()));

        Assert.Equal(AdsPushErrorType.InvalidArgument, exception.ErrorType);
    }

    [Fact]
    public async Task BasicSendAsyncThrowsWhenApnsSettingsMissing()
    {
        var settings = new AdsPushAppSettings();
        settings.TargetMappings[AdsPushTarget.Ios] = AdsPushProvider.Apns;

        var configurationProvider = new Mock<IAdsPushConfigurationProvider>();
        configurationProvider.Setup(p => p.GetSettingsAsync("app", It.IsAny<CancellationToken>()))
            .ReturnsAsync(settings);

        var sender = new AdsPushSender(
            "app",
            configurationProvider.Object,
            Mock.Of<IFirebasePushNotificationSenderFactory>(),
            Mock.Of<IApplePushNotificationSenderFactory>(),
            Mock.Of<IVapidPushNotificationSenderFactory>());

        var exception = await Assert.ThrowsAsync<AdsPushException>(() =>
            sender.BasicSendAsync(AdsPushTarget.Ios, "token", CreatePayload()));

        Assert.Equal(AdsPushErrorType.InvalidAuthConfiguration, exception.ErrorType);
    }

    [Fact]
    public async Task BasicSendAsyncThrowsWhenFirebaseSettingsMissing()
    {
        var settings = new AdsPushAppSettings();
        settings.TargetMappings[AdsPushTarget.Android] = AdsPushProvider.Firebase;

        var configurationProvider = new Mock<IAdsPushConfigurationProvider>();
        configurationProvider.Setup(p => p.GetSettingsAsync("app", It.IsAny<CancellationToken>()))
            .ReturnsAsync(settings);

        var sender = new AdsPushSender(
            "app",
            configurationProvider.Object,
            Mock.Of<IFirebasePushNotificationSenderFactory>(),
            Mock.Of<IApplePushNotificationSenderFactory>(),
            Mock.Of<IVapidPushNotificationSenderFactory>());

        var exception = await Assert.ThrowsAsync<AdsPushException>(() =>
            sender.BasicSendAsync(AdsPushTarget.Android, "token", CreatePayload()));

        Assert.Equal(AdsPushErrorType.InvalidAuthConfiguration, exception.ErrorType);
    }

    [Fact]
    public async Task BasicSendAsyncThrowsWhenVapidSettingsMissing()
    {
        var settings = new AdsPushAppSettings();
        settings.TargetMappings[AdsPushTarget.BrowserAndPwa] = AdsPushProvider.VapidWebPush;

        var configurationProvider = new Mock<IAdsPushConfigurationProvider>();
        configurationProvider.Setup(p => p.GetSettingsAsync("app", It.IsAny<CancellationToken>()))
            .ReturnsAsync(settings);

        var sender = new AdsPushSender(
            "app",
            configurationProvider.Object,
            Mock.Of<IFirebasePushNotificationSenderFactory>(),
            Mock.Of<IApplePushNotificationSenderFactory>(),
            Mock.Of<IVapidPushNotificationSenderFactory>());

        var exception = await Assert.ThrowsAsync<AdsPushException>(() =>
            sender.BasicSendAsync(AdsPushTarget.BrowserAndPwa, "token", CreatePayload()));

        Assert.Equal(AdsPushErrorType.InvalidAuthConfiguration, exception.ErrorType);
    }

    [Fact]
    public async Task BasicSendAsyncInvokesApnsSender()
    {
        var settings = new AdsPushAppSettings { Apns = new AdsPushAPNSSettings() };
        settings.TargetMappings[AdsPushTarget.Ios] = AdsPushProvider.Apns;

        var configurationProvider = new Mock<IAdsPushConfigurationProvider>();
        configurationProvider.Setup(p => p.GetSettingsAsync("app", It.IsAny<CancellationToken>()))
            .ReturnsAsync(settings);

        var apnsSender = new Mock<IApplePushNotificationSender>();
        var apnsFactory = new Mock<IApplePushNotificationSenderFactory>();
        apnsFactory.Setup(f => f.GetSender("app", settings.Apns))
            .Returns(apnsSender.Object);

        var sender = new AdsPushSender(
            "app",
            configurationProvider.Object,
            Mock.Of<IFirebasePushNotificationSenderFactory>(),
            apnsFactory.Object,
            Mock.Of<IVapidPushNotificationSenderFactory>());

        await sender.BasicSendAsync(AdsPushTarget.Ios, "token", CreatePayload());

        apnsSender.Verify(s => s.SendAsync("token", It.IsAny<AdsPushBasicSendPayload>(), It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task BasicSendAsyncInvokesFirebaseSender()
    {
        var firebaseSettings = new AdsPushFirebaseSettings();
        var settings = new AdsPushAppSettings { Firebase = firebaseSettings };
        settings.TargetMappings[AdsPushTarget.Android] = AdsPushProvider.Firebase;

        var configurationProvider = new Mock<IAdsPushConfigurationProvider>();
        configurationProvider.Setup(p => p.GetSettingsAsync("app", It.IsAny<CancellationToken>()))
            .ReturnsAsync(settings);

        var firebaseSender = new Mock<IFirebasePushNotificationSender>();
        var firebaseFactory = new Mock<IFirebasePushNotificationSenderFactory>();
        firebaseFactory.Setup(f => f.GetSender("app", firebaseSettings))
            .Returns(firebaseSender.Object);

        var sender = new AdsPushSender(
            "app",
            configurationProvider.Object,
            firebaseFactory.Object,
            Mock.Of<IApplePushNotificationSenderFactory>(),
            Mock.Of<IVapidPushNotificationSenderFactory>());

        await sender.BasicSendAsync(AdsPushTarget.Android, "token", CreatePayload());

        firebaseSender.Verify(
            s => s.SendAsync(AdsPushTarget.Android, "token", It.IsAny<AdsPushBasicSendPayload>(),
                It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task BasicSendAsyncInvokesVapidSender()
    {
        var vapidSettings = new AdsPushVapidSettings
        {
            PublicKey = "public", PrivateKey = "private", Subject = "mailto:test@example.com"
        };
        var settings = new AdsPushAppSettings { Vapid = vapidSettings };
        settings.TargetMappings[AdsPushTarget.BrowserAndPwa] = AdsPushProvider.VapidWebPush;

        var configurationProvider = new Mock<IAdsPushConfigurationProvider>();
        configurationProvider.Setup(p => p.GetSettingsAsync("app", It.IsAny<CancellationToken>()))
            .ReturnsAsync(settings);

        var vapidSender = new Mock<IVapidPushNotificationSender>();
        var vapidFactory = new Mock<IVapidPushNotificationSenderFactory>();
        vapidFactory.Setup(f => f.GetSender("app", vapidSettings))
            .Returns(vapidSender.Object);

        var sender = new AdsPushSender(
            "app",
            configurationProvider.Object,
            Mock.Of<IFirebasePushNotificationSenderFactory>(),
            Mock.Of<IApplePushNotificationSenderFactory>(),
            vapidFactory.Object);

        await sender.BasicSendAsync(AdsPushTarget.BrowserAndPwa, "token", CreatePayload());

        vapidSender.Verify(
            s => s.SendAsync("token", It.IsAny<AdsPushBasicSendPayload>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task BasicSendAsyncThrowsForUnsupportedProvider()
    {
        var settings = new AdsPushAppSettings();
        settings.TargetMappings[AdsPushTarget.Android] = (AdsPushProvider)999;

        var configurationProvider = new Mock<IAdsPushConfigurationProvider>();
        configurationProvider.Setup(p => p.GetSettingsAsync("app", It.IsAny<CancellationToken>()))
            .ReturnsAsync(settings);

        var sender = new AdsPushSender(
            "app",
            configurationProvider.Object,
            Mock.Of<IFirebasePushNotificationSenderFactory>(),
            Mock.Of<IApplePushNotificationSenderFactory>(),
            Mock.Of<IVapidPushNotificationSenderFactory>());

        await Assert.ThrowsAsync<NotSupportedException>(() =>
            sender.BasicSendAsync(AdsPushTarget.Android, "token", CreatePayload()));
    }

    [Fact]
    public void GetApnsSenderReturnsFactoryInstance()
    {
        var apnsFactory = new Mock<IApplePushNotificationSenderFactory>();
        var apnsSender = Mock.Of<IApplePushNotificationSender>();
        apnsFactory.Setup(f => f.GetSender("app")).Returns(apnsSender);

        var sender = new AdsPushSender(
            "app",
            Mock.Of<IAdsPushConfigurationProvider>(),
            Mock.Of<IFirebasePushNotificationSenderFactory>(),
            apnsFactory.Object,
            Mock.Of<IVapidPushNotificationSenderFactory>());

        Assert.Same(apnsSender, sender.GetApnsSender());
    }

    [Fact]
    public void GetFirebaseSenderReturnsFactoryInstance()
    {
        var firebaseFactory = new Mock<IFirebasePushNotificationSenderFactory>();
        var firebaseSender = Mock.Of<IFirebasePushNotificationSender>();
        firebaseFactory.Setup(f => f.GetSender("app")).Returns(firebaseSender);

        var sender = new AdsPushSender(
            "app",
            Mock.Of<IAdsPushConfigurationProvider>(),
            firebaseFactory.Object,
            Mock.Of<IApplePushNotificationSenderFactory>(),
            Mock.Of<IVapidPushNotificationSenderFactory>());

        Assert.Same(firebaseSender, sender.GetFirebaseSender());
    }

    [Fact]
    public void GetVapidSenderReturnsFactoryInstance()
    {
        var vapidFactory = new Mock<IVapidPushNotificationSenderFactory>();
        var vapidSender = Mock.Of<IVapidPushNotificationSender>();
        vapidFactory.Setup(f => f.GetSender("app")).Returns(vapidSender);

        var sender = new AdsPushSender(
            "app",
            Mock.Of<IAdsPushConfigurationProvider>(),
            Mock.Of<IFirebasePushNotificationSenderFactory>(),
            Mock.Of<IApplePushNotificationSenderFactory>(),
            vapidFactory.Object);

        Assert.Same(vapidSender, sender.GetVapidSender());
    }
}
