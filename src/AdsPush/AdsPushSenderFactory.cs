using System.Collections.Concurrent;
using AdsPush.Abstraction;
using AdsPush.APNS;
using AdsPush.Firebase;
using AdsPush.HMS;
using AdsPush.Vapid;

namespace AdsPush
{
    /// <summary>
    /// Implementation of <see cref="IAdsPushSenderFactory"/>
    /// </summary>
    public class AdsPushSenderFactory : IAdsPushSenderFactory
    {
        private readonly ConcurrentDictionary<string, IAdsPushSender> _senders;
        private readonly IAdsPushConfigurationProvider _adsPushConfigurationProvider;
        private readonly IApplePushNotificationSenderFactory _applePushNotificationSenderFactory;
        private readonly IFirebasePushNotificationSenderFactory _firebasePushNotificationSenderFactory;
        private readonly IVapidPushNotificationSenderFactory _vapidPushNotificationSenderFactory;
        private readonly IHMSPushNotificationSenderFactory _hmsPushNotificationSenderFactory;

        /// <summary>"
        ///
        /// </summary>
        /// <param name="adsPushConfigurationProvider"></param>
        /// <param name="applePushNotificationSenderFactory"></param>
        /// <param name="firebasePushNotificationSenderFactory"></param>
        /// <param name="vapidPushNotificationSenderFactory"></param>
        /// <param name="hmsPushNotificationSenderFactory"></param>
        public AdsPushSenderFactory(
            IAdsPushConfigurationProvider adsPushConfigurationProvider,
            IApplePushNotificationSenderFactory applePushNotificationSenderFactory,
            IFirebasePushNotificationSenderFactory firebasePushNotificationSenderFactory,
            IVapidPushNotificationSenderFactory vapidPushNotificationSenderFactory,
            IHMSPushNotificationSenderFactory hmsPushNotificationSenderFactory)
        {
            this._senders = new ConcurrentDictionary<string, IAdsPushSender>();
            this._adsPushConfigurationProvider = adsPushConfigurationProvider;
            this._applePushNotificationSenderFactory = applePushNotificationSenderFactory;
            this._firebasePushNotificationSenderFactory = firebasePushNotificationSenderFactory;
            this._vapidPushNotificationSenderFactory = vapidPushNotificationSenderFactory;
            this._hmsPushNotificationSenderFactory = hmsPushNotificationSenderFactory;
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
                    this._applePushNotificationSenderFactory,
                    this._vapidPushNotificationSenderFactory,
                    this._hmsPushNotificationSenderFactory));
        }
    }
}
