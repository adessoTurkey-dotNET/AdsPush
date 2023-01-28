using AdsPush.Abstraction.Settings;
using AdsPush.APNS.Settings;

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
