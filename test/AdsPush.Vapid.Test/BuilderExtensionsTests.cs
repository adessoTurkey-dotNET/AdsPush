using AdsPush.Abstraction.Settings;
using AdsPush.Vapid.Extensions;
using AdsPush.Vapid.Settings;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace AdsPush.Vapid.Test;

public class BuilderExtensionsTests
{
    [Fact]
    public void AddVapidNotificationServiceFactoryRegistersFactoryUsingAction()
    {
        var keys = VapidHelper.GenerateVapidKeys();
        var services = new ServiceCollection();
        using var httpClient = new HttpClient(new HttpClientHandler());

        services.AddVapidNotificationServiceFactory(section =>
        {
            section["app"] = new AdsPushVapidSettings
            {
                PublicKey = keys.PublicLey, PrivateKey = keys.PrivateKey, Subject = "mailto:test@example.com"
            };
        }, httpClient);

        using var provider = services.BuildServiceProvider();
        var factory = provider.GetRequiredService<IVapidPushNotificationSenderFactory>();
        var sender = factory.GetSender("app");

        Assert.NotNull(sender);
    }

    [Fact]
    public void AddVapidNotificationServiceFactoryReadsSettingsFromOptions()
    {
        var keys = VapidHelper.GenerateVapidKeys();
        var services = new ServiceCollection();
        var section = new VapidSettingsSection
        {
            ["configured-app"] = new AdsPushVapidSettings
            {
                PublicKey = keys.PublicLey, PrivateKey = keys.PrivateKey, Subject = "mailto:test@example.com"
            }
        };

        services.AddSingleton<IOptions<VapidSettingsSection>>(new OptionsWrapper<VapidSettingsSection>(section));
        services.AddVapidNotificationServiceFactory();

        using var provider = services.BuildServiceProvider();
        var factory = provider.GetRequiredService<IVapidPushNotificationSenderFactory>();
        var sender = factory.GetSender("configured-app");

        Assert.NotNull(sender);
    }

    [Fact]
    public void AddVapidNotificationServiceFactoryWithoutOptionsKeepsDefaultSection()
    {
        var services = new ServiceCollection();
        services.AddVapidNotificationServiceFactory();

        using var provider = services.BuildServiceProvider();
        var factory = provider.GetRequiredService<IVapidPushNotificationSenderFactory>();

        Assert.Throws<ArgumentException>(() => factory.GetSender("missing-app"));
    }
}
