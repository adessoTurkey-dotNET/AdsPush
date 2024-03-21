using AdsPush.Abstraction.Settings;

namespace AdsPush.HMS
{
    public interface IHMSPushNotificationSenderFactory
    {
        IHMSPushNotificationSender GetSender(
         string appName);

        IHMSPushNotificationSender GetSender(
            string appName,
            AdsPushHMSSettings hmsSettings);
    }
}

