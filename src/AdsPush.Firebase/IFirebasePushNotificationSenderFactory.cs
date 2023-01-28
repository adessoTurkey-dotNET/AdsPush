using AdsPush.Abstraction.Settings;
using AdsPush.Firebase.Settings;

namespace AdsPush.Firebase
{
    public interface IFirebasePushNotificationSenderFactory
    {
        /// <summary>
        /// Get instance of <see cref="IFirebasePushNotificationSender"/> by sing the pre-configured settings.
        /// </summary>
        /// <param name="appName"></param>
        /// <returns></returns>
        IFirebasePushNotificationSender GetSender(
            string appName);

        /// <summary>
        /// Get instance of <see cref="IFirebasePushNotificationSender"/> by providing the request settings;
        /// </summary>
        /// <param name="appName"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        IFirebasePushNotificationSender GetSender(
            string appName,
            AdsPushFirebaseSettings settings);
    }
}
