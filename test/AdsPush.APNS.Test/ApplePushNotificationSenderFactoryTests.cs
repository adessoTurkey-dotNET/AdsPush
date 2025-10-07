using AdsPush.APNS.Settings;

namespace AdsPush.APNS.Test;

public class ApplePushNotificationSenderFactoryTests
{
    [Fact]
    public void ConstructorWithoutParametersCreatesInstance()
    {
        var factory = new ApplePushNotificationSenderFactory();
        Assert.NotNull(factory);
    }

    [Fact]
    public void GetSenderReturnsCachedInstance()
    {
        var settings = ApnsTestHelper.CreateSettings(appBundleIdentifier: "com.example.cached");
        var section = new APNSSettingsSection { ["app"] = settings };

        using var client = new HttpClient(new HttpClientHandler());
        var factory = new ApplePushNotificationSenderFactory(section, client);

        var first = factory.GetSender("app");
        var second = factory.GetSender("app");

        Assert.Same(first, second);
    }

    [Fact]
    public void GetSenderWhenSettingsMissingThrows()
    {
        var section = new APNSSettingsSection();
        using var client = new HttpClient(new HttpClientHandler());
        var factory = new ApplePushNotificationSenderFactory(section, client);

        Assert.Throws<ArgumentException>(() => factory.GetSender("missing"));
    }

    [Fact]
    public void GetSenderWithExplicitSettingsRegistersSender()
    {
        var section = new APNSSettingsSection();
        using var client = new HttpClient(new HttpClientHandler());
        var factory = new ApplePushNotificationSenderFactory(section, client);
        var explicitSettings = ApnsTestHelper.CreateSettings(appBundleIdentifier: "com.example.explicit");

        var sender = factory.GetSender("app", explicitSettings);
        Assert.NotNull(sender);
        Assert.Same(sender, factory.GetSender("app"));
    }

    [Fact]
    public void ConstructorWithNullSectionCreatesEmptySettings()
    {
        using var client = new HttpClient(new HttpClientHandler());
        var factory = new ApplePushNotificationSenderFactory(null, client);
        var settings = ApnsTestHelper.CreateSettings(appBundleIdentifier: "com.example.null");

        var sender = factory.GetSender("null-app", settings);

        Assert.NotNull(sender);
    }
}
