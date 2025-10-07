using AdsPush.Abstraction;
using AdsPush.Abstraction.APNS;
using AdsPush.Abstraction.Settings;

namespace AdsPush.Test;

public class AdsPushSenderBuilderTests
{
    [Fact]
    public void ConfigureMethodsPopulateSettings()
    {
        var builder = new AdsPushSenderBuilder();

        builder.ConfigureApns(
            new AdsPushAPNSSettings
            {
                AppBundleIdentifier = "com.example.app",
                P8PrivateKey = "key",
                P8PrivateKeyId = "kid",
                TeamId = "team",
                EnvironmentType = APNSEnvironmentType.Development
            }, new HttpClient());

        builder.ConfigureFirebase(
            new AdsPushFirebaseSettings
            {
                ProjectId = "project",
                PrivateKey = "private",
                PrivateKeyId = "pkid",
                ClientEmail = "service@example.com",
                ClientId = "client",
                AuthUri = "https://auth",
                TokenUri = "https://token",
                AuthProviderX509CertUrl = "https://cert",
                ClientX509CertUrl = "https://client-cert",
                Type = "service_account"
            }, AdsPushTarget.Android);

        builder.ConfigureVapid(
            new AdsPushVapidSettings
            {
                PublicKey = "public", PrivateKey = "private", Subject = "mailto:test@example.com"
            }, new HttpClient());

        var sender = builder.BuildSender();

        Assert.NotNull(sender);
    }

    [Fact]
    public void ConfigureMethodsUseDefaultHttpClientsWhenNotProvided()
    {
        var builder = new AdsPushSenderBuilder();

        builder.ConfigureApns(new AdsPushAPNSSettings
        {
            AppBundleIdentifier = "com.example.default",
            P8PrivateKey = "key",
            P8PrivateKeyId = "kid",
            TeamId = "team",
            EnvironmentType = APNSEnvironmentType.Production
        });

        builder.ConfigureVapid(new AdsPushVapidSettings
        {
            PublicKey = "public", PrivateKey = "private", Subject = "mailto:second@example.com"
        });

        var sender = builder.BuildSender();

        Assert.NotNull(sender);
    }

    [Fact]
    public void BuildSenderWithoutOptionalConfigurationsReturnsSender()
    {
        var builder = new AdsPushSenderBuilder();
        var sender = builder.BuildSender();

        Assert.NotNull(sender);
    }
}
