using AdsPush.Abstraction.Settings;
using AdsPush.Firebase.Extensions;
using AdsPush.Firebase.Settings;
using FirebaseAdmin;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace AdsPush.Firebase.Test;

public class BuilderExtensionTests
{
    [Fact]
    public void AddFirebaseCloudMessagingServiceRegistersSenderUsingAction()
    {
        var services = new ServiceCollection();
        var settings = FirebaseTestHelper.CreateSettings();

        services.AddFirebaseCloudMessagingService(config =>
        {
            config.Type = settings.Type;
            config.ProjectId = settings.ProjectId;
            config.PrivateKeyId = settings.PrivateKeyId;
            config.PrivateKey = settings.PrivateKey;
            config.ClientEmail = settings.ClientEmail;
            config.ClientId = settings.ClientId;
            config.AuthUri = settings.AuthUri;
            config.TokenUri = settings.TokenUri;
            config.AuthProviderX509CertUrl = settings.AuthProviderX509CertUrl;
            config.ClientX509CertUrl = settings.ClientX509CertUrl;
        });

        using var provider = services.BuildServiceProvider();
        var sender = provider.GetRequiredService<IFirebasePushNotificationSender>();

        Assert.NotNull(sender);
        FirebaseApp.GetInstance(settings.ProjectId).Delete();
    }

    [Fact]
    public void AddFirebaseCloudMessagingServiceReadsSettingsFromOptions()
    {
        var services = new ServiceCollection();
        var settings = FirebaseTestHelper.CreateSettings();
        services.AddSingleton<IOptions<AdsPushFirebaseSettings>>(new OptionsWrapper<AdsPushFirebaseSettings>(settings));

        services.AddFirebaseCloudMessagingService(null);

        using var provider = services.BuildServiceProvider();
        var sender = provider.GetRequiredService<IFirebasePushNotificationSender>();

        Assert.NotNull(sender);
        FirebaseApp.GetInstance(settings.ProjectId).Delete();
    }

    [Fact]
    public void AddFirebaseCloudMessagingServiceFactoryRegistersFactoryUsingAction()
    {
        var services = new ServiceCollection();
        var settings = FirebaseTestHelper.CreateSettings();

        services.AddFirebaseCloudMessagingServiceFactory(section =>
        {
            section["app"] = settings;
        });

        using var provider = services.BuildServiceProvider();
        var factory = provider.GetRequiredService<IFirebasePushNotificationSenderFactory>();
        var sender = factory.GetSender("app");

        Assert.NotNull(sender);
        FirebaseApp.GetInstance(settings.ProjectId).Delete();
    }

    [Fact]
    public void AddFirebaseCloudMessagingServiceFactoryReadsFromOptions()
    {
        var services = new ServiceCollection();
        var settings = FirebaseTestHelper.CreateSettings();
        var section = new FirebaseAppSettingsSection { ["configured"] = settings };

        services.AddSingleton<IOptions<FirebaseAppSettingsSection>>(
            new OptionsWrapper<FirebaseAppSettingsSection>(section));
        services.AddFirebaseCloudMessagingServiceFactory();

        using var provider = services.BuildServiceProvider();
        var factory = provider.GetRequiredService<IFirebasePushNotificationSenderFactory>();
        var sender = factory.GetSender("configured");

        Assert.NotNull(sender);
        FirebaseApp.GetInstance(settings.ProjectId).Delete();
    }

    [Fact]
    public void AddFirebaseCloudMessagingServiceFactoryHandlesMissingOptions()
    {
        var services = new ServiceCollection();
        services.AddFirebaseCloudMessagingServiceFactory();

        using var provider = services.BuildServiceProvider();
        var factory = provider.GetRequiredService<IFirebasePushNotificationSenderFactory>();
        var explicitSettings = FirebaseTestHelper.CreateSettings();
        var sender = factory.GetSender("custom", explicitSettings);

        Assert.NotNull(sender);
        FirebaseApp.GetInstance(explicitSettings.ProjectId).Delete();
    }
}
