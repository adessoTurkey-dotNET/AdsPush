using AdsPush.Abstraction.Settings;

namespace AdsPush.Vapid
{
    public interface IVapidPushNotificationSenderFactory
    {
        IVapidPushNotificationSender GetSender(
            string appName);

        IVapidPushNotificationSender GetSender(
            string appName,
            AdsPushVapidSettings vapidSettings);
    }
}
