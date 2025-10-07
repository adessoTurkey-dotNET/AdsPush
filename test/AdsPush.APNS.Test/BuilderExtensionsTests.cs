using AdsPush.APNS.Extensions;
using AdsPush.APNS.Settings;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace AdsPush.APNS.Test;

public class BuilderExtensionsTests
{
    [Fact]
    public void AddAppleNotificationServiceFactoryWithActionRegistersFactory()
    {
        var services = new ServiceCollection();
        using var httpClient = new HttpClient(new HttpClientHandler());
        var settings = ApnsTestHelper.CreateSettings(appBundleIdentifier: "com.example.action");

        services.AddAppleNotificationServiceFactory(section =>
        {
            section["app"] = settings;
        }, httpClient);

        using var provider = services.BuildServiceProvider();
        var factory = provider.GetRequiredService<IApplePushNotificationSenderFactory>();
        var sender = factory.GetSender("app");

        Assert.NotNull(sender);
    }

    [Fact]
    public void AddAppleNotificationServiceFactoryWithOptionsUsesConfiguredSection()
    {
        var services = new ServiceCollection();
        var settings = ApnsTestHelper.CreateSettings(appBundleIdentifier: "com.example.options");
        var section = new APNSSettingsSection { ["configured"] = settings };

        services.AddSingleton<IOptions<APNSSettingsSection>>(new OptionsWrapper<APNSSettingsSection>(section));
        services.AddAppleNotificationServiceFactory();

        using var provider = services.BuildServiceProvider();
        var factory = provider.GetRequiredService<IApplePushNotificationSenderFactory>();
        var sender = factory.GetSender("configured");

        Assert.NotNull(sender);
    }

    [Fact]
    public void AddAppleNotificationServiceFactoryHandlesMissingOptions()
    {
        var services = new ServiceCollection();
        services.AddAppleNotificationServiceFactory();

        using var provider = services.BuildServiceProvider();
        var factory = provider.GetRequiredService<IApplePushNotificationSenderFactory>();
        var settings = ApnsTestHelper.CreateSettings(appBundleIdentifier: "com.example.missing");
        var sender = factory.GetSender("missing", settings);

        Assert.NotNull(sender);
    }
}
