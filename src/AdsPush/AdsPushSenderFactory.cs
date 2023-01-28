using System.Collections.Concurrent;
using AdsPush.Abstraction;
using AdsPush.APNS;
using AdsPush.Firebase;

namespace AdsPush
{
    public class AdsPushSenderFactory : IAdsPushSenderFactory
    {
        private readonly ConcurrentDictionary<string, IAdsPushSender> _senders;
        private readonly IAdsPushConfigurationProvider _adsPushConfigurationProvider;
        private readonly IApplePushNotificationSenderFactory _applePushNotificationSenderFactory;
        private readonly IFirebasePushNotificationSenderFactory _firebasePushNotificationSenderFactory;
       
        public AdsPushSenderFactory(
            IAdsPushConfigurationProvider adsPushConfigurationProvider,
            IApplePushNotificationSenderFactory applePushNotificationSenderFactory,
            IFirebasePushNotificationSenderFactory firebasePushNotificationSenderFactory)
        {
            this._senders = new ConcurrentDictionary<string, IAdsPushSender>();
            this._adsPushConfigurationProvider = adsPushConfigurationProvider;
            this._applePushNotificationSenderFactory = applePushNotificationSenderFactory;
            this._firebasePushNotificationSenderFactory = firebasePushNotificationSenderFactory;
        }

        /// <inheritdoc />
        public IAdsPushSender GetSender(
            string appName)
        {
            return this._senders.GetOrAdd(
                appName,
                new AdsPushSender(
                    appName, this._adsPushConfigurationProvider,
                    this._firebasePushNotificationSenderFactory,
                    this._applePushNotificationSenderFactory));
        }
    }
}
