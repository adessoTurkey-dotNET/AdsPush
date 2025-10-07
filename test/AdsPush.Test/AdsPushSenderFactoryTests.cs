using AdsPush.Abstraction;
using AdsPush.Abstraction.Settings;
using AdsPush.APNS;
using AdsPush.Firebase;
using AdsPush.Vapid;
using Moq;

namespace AdsPush.Test;

public class AdsPushSenderFactoryTests
{
    [Fact]
    public void GetSenderReturnsCachedInstance()
    {
        var configuration = Mock.Of<IAdsPushConfigurationProvider>();
        var factory = new AdsPushSenderFactory(
            configuration,
            Mock.Of<IApplePushNotificationSenderFactory>(),
            Mock.Of<IFirebasePushNotificationSenderFactory>(),
            Mock.Of<IVapidPushNotificationSenderFactory>());

        var first = factory.GetSender("app");
        var second = factory.GetSender("app");

        Assert.Same(first, second);
    }

    [Fact]
    public async Task SenderFromFactoryUsesProvidedFactories()
    {
        var settings = new AdsPushAppSettings();
        settings.Firebase = new AdsPushFirebaseSettings();
        settings.TargetMappings[AdsPushTarget.Android] = AdsPushProvider.Firebase;

        var configuration = new Mock<IAdsPushConfigurationProvider>();
        configuration.Setup(p => p.GetSettingsAsync("app", It.IsAny<CancellationToken>()))
            .ReturnsAsync(settings);

        var firebaseSender = new Mock<IFirebasePushNotificationSender>();
        var firebaseFactory = new Mock<IFirebasePushNotificationSenderFactory>();
        firebaseFactory.Setup(f => f.GetSender("app", settings.Firebase))
            .Returns(firebaseSender.Object);
        firebaseFactory.Setup(f => f.GetSender("app"))
            .Returns(firebaseSender.Object);

        var senderFactory = new AdsPushSenderFactory(
            configuration.Object,
            Mock.Of<IApplePushNotificationSenderFactory>(),
            firebaseFactory.Object,
            Mock.Of<IVapidPushNotificationSenderFactory>());

        var sender = senderFactory.GetSender("app");

        Assert.Same(firebaseSender.Object, sender.GetFirebaseSender());

        await sender.BasicSendAsync(AdsPushTarget.Android, "token",
            new AdsPushBasicSendPayload
            {
                Title = AdsPushText.CreateUsingString("title"), Detail = AdsPushText.CreateUsingString("body")
            });

        firebaseSender.Verify(
            s => s.SendAsync(AdsPushTarget.Android, "token", It.IsAny<AdsPushBasicSendPayload>(),
                It.IsAny<CancellationToken>()), Times.Once);
    }
}
