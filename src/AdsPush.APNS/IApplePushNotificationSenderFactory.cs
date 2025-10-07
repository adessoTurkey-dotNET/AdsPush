using AdsPush.Abstraction.Settings;

namespace AdsPush.APNS
{
    public interface IApplePushNotificationSenderFactory
    {
        IApplePushNotificationSender GetSender(
            string appName);

        IApplePushNotificationSender GetSender(
            string appName,
            AdsPushAPNSSettings apnsSettings);
    }
}
