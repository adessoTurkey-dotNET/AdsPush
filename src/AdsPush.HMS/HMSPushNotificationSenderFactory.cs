using System;
using System.Collections.Concurrent;
using System.Net.Http;
using AdsPush.Abstraction.Settings;
using AdsPush.HMS.Settings;

namespace AdsPush.HMS
{
    public class HMSPushNotificationSenderFactory : IHMSPushNotificationSenderFactory
    {
        private readonly HttpClient _httpClient;
        private readonly HttpClient _authHttpClient;
        private readonly ConcurrentDictionary<string, HMSPushNotificationSender> _hmsPushNotificationSenders;
        private readonly HMSSettingsSection _settings;

        public HMSPushNotificationSenderFactory(
            HMSSettingsSection hmsSettingsSection,
            HttpClient httpClient,
            HttpClient _authHttpClient)
        {
            this._hmsPushNotificationSenders = new ConcurrentDictionary<string, HMSPushNotificationSender>();
            this._settings = hmsSettingsSection ?? new HMSSettingsSection();
            this._httpClient = httpClient;
            this._authHttpClient = _authHttpClient;
        }

        public HMSPushNotificationSenderFactory()
        {
            this._hmsPushNotificationSenders = new ConcurrentDictionary<string, HMSPushNotificationSender>();
        }

        public IHMSPushNotificationSender GetSender(string appName)
        {
            return this._hmsPushNotificationSenders.GetOrAdd(appName, this.ValueFactory);
        }

        private HMSPushNotificationSender ValueFactory(
           string arg)
        {
            if (!this._settings.ContainsKey(arg))
            {
                throw new ArgumentException($"{arg} is not defined in settings!");
            }

            var settings = this._settings[arg];
            return new HMSPushNotificationSender(settings, this._httpClient, this._authHttpClient);
        }

        public IHMSPushNotificationSender GetSender(string appName, AdsPushHMSSettings hmsSettings)
        {
            return this._hmsPushNotificationSenders.GetOrAdd(appName, new HMSPushNotificationSender(hmsSettings, this._httpClient, this._authHttpClient));
        }
    }
}
