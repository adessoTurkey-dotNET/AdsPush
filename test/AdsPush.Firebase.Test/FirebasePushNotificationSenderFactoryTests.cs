using AdsPush.Firebase.Settings;
using FirebaseAdmin;

namespace AdsPush.Firebase.Test;

public class FirebasePushNotificationSenderFactoryTests
{
    [Fact]
    public void GetSenderReturnsCachedInstance()
    {
        var settings = FirebaseTestHelper.CreateSettings();
        var section = new FirebaseAppSettingsSection { ["app"] = settings };

        var factory = new FirebasePushNotificationSenderFactory(section);
        var first = factory.GetSender("app");
        var second = factory.GetSender("app");

        Assert.Same(first, second);
        FirebaseApp.GetInstance(settings.ProjectId).Delete();
    }

    [Fact]
    public void GetSenderWhenSettingsMissingThrows()
    {
        var section = new FirebaseAppSettingsSection();
        var factory = new FirebasePushNotificationSenderFactory(section);

        Assert.Throws<ArgumentException>(() => factory.GetSender("missing"));
    }

    [Fact]
    public void GetSenderWithExplicitSettingsRegistersSender()
    {
        var section = new FirebaseAppSettingsSection();
        var factory = new FirebasePushNotificationSenderFactory(section);
        var explicitSettings = FirebaseTestHelper.CreateSettings();

        var sender = factory.GetSender("custom", explicitSettings);
        Assert.NotNull(sender);
        Assert.Same(sender, factory.GetSender("custom"));
        FirebaseApp.GetInstance(explicitSettings.ProjectId).Delete();
    }

    [Fact]
    public void ConstructorWithNullSettingsCreatesEmptySection()
    {
        var factory = new FirebasePushNotificationSenderFactory(null);
        var explicitSettings = FirebaseTestHelper.CreateSettings();

        var sender = factory.GetSender("null-app", explicitSettings);
        Assert.NotNull(sender);
        FirebaseApp.GetInstance(explicitSettings.ProjectId).Delete();
    }
}
