using System;
using System.Collections.Concurrent;
using System.Net.Http;
using AdsPush.Abstraction.Settings;
using AdsPush.APNS.Settings;

namespace AdsPush.APNS
{
    public class ApplePushNotificationSenderFactory:  IApplePushNotificationSenderFactory
    {
        public ApplePushNotificationSenderFactory(
            APNSSettingsSection apnsSettingsSection,
            HttpClient httpClient)
        {
            this._apnsPushNotificationSenders = new ConcurrentDictionary<string, ApplePushNotificationSender>();
            this._settings = apnsSettingsSection ?? new APNSSettingsSection();
            this._httpClient = httpClient;
        }

        public ApplePushNotificationSenderFactory()
        {
            this._apnsPushNotificationSenders = new ConcurrentDictionary<string, ApplePushNotificationSender>();
        }

        private readonly HttpClient _httpClient;
        private readonly ConcurrentDictionary<string, ApplePushNotificationSender> _apnsPushNotificationSenders;
        private readonly APNSSettingsSection _settings;

        /// <inheritdoc/>
        public IApplePushNotificationSender GetSender(
            string appName)
        {
            return this._apnsPushNotificationSenders.GetOrAdd(appName, this.ValueFactory);
        }

        private ApplePushNotificationSender ValueFactory(
            string arg)
        {
            if (!this._settings.ContainsKey(arg))
            {
                throw new ArgumentException($"{arg} is not defined in settings!");
            }

            var settings = this._settings[arg];
            return new ApplePushNotificationSender(settings, this._httpClient);
        }

        /// <inheritdoc/>
        public IApplePushNotificationSender GetSender(
            string appName,
            AdsPushAPNSSettings apnsSettings)
        {
            return this._apnsPushNotificationSenders.GetOrAdd(appName, new ApplePushNotificationSender(apnsSettings, this._httpClient));
        }
    }
}
