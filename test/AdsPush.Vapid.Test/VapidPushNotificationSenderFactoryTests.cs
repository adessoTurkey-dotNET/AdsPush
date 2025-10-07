using AdsPush.Abstraction.Settings;
using AdsPush.Vapid.Settings;

namespace AdsPush.Vapid.Test;

public class VapidPushNotificationSenderFactoryTests
{
    [Fact]
    public void ConstructorWithoutParametersInitializes()
    {
        var factory = new VapidPushNotificationSenderFactory();
        Assert.NotNull(factory);
    }

    [Fact]
    public void GetSenderReturnsCachedInstanceForKnownApp()
    {
        var keys = VapidHelper.GenerateVapidKeys();
        var settingsSection = new VapidSettingsSection
        {
            ["ads-app"] = new AdsPushVapidSettings
            {
                PublicKey = keys.PublicLey, PrivateKey = keys.PrivateKey, Subject = "mailto:test@example.com"
            }
        };

        using var client = new HttpClient(new HttpClientHandler());
        var factory = new VapidPushNotificationSenderFactory(settingsSection, client);

        var first = factory.GetSender("ads-app");
        var second = factory.GetSender("ads-app");

        Assert.Same(first, second);
    }

    [Fact]
    public void GetSenderThrowsWhenSettingsMissing()
    {
        var settingsSection = new VapidSettingsSection();
        using var client = new HttpClient(new HttpClientHandler());
        var factory = new VapidPushNotificationSenderFactory(settingsSection, client);

        Assert.Throws<ArgumentException>(() => factory.GetSender("missing-app"));
    }

    [Fact]
    public void GetSenderWithExplicitSettingsAddsInstance()
    {
        var keys = VapidHelper.GenerateVapidKeys();
        var settingsSection = new VapidSettingsSection();

        using var client = new HttpClient(new HttpClientHandler());
        var factory = new VapidPushNotificationSenderFactory(settingsSection, client);

        var explicitSettings = new AdsPushVapidSettings
        {
            PublicKey = keys.PublicLey, PrivateKey = keys.PrivateKey, Subject = "mailto:test@example.com"
        };

        var created = factory.GetSender("custom-app", explicitSettings);
        var retrieved = factory.GetSender("custom-app");

        Assert.Same(created, retrieved);
    }

    [Fact]
    public void ConstructorCreatesDefaultSettingsWhenSectionNull()
    {
        using var client = new HttpClient(new HttpClientHandler());
        var factory = new VapidPushNotificationSenderFactory(null, client);
        var keys = VapidHelper.GenerateVapidKeys();

        var sender = factory.GetSender("generated-app",
            new AdsPushVapidSettings
            {
                PublicKey = keys.PublicLey, PrivateKey = keys.PrivateKey, Subject = "mailto:test@example.com"
            });

        Assert.NotNull(sender);
    }
}
