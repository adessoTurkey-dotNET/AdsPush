using System;
using System.Collections.Concurrent;
using System.Net.Http;
using AdsPush.Abstraction.Settings;
using AdsPush.Vapid.Settings;

namespace AdsPush.Vapid
{
    public class VapidPushNotificationSenderFactory:  IVapidPushNotificationSenderFactory
    {
        public VapidPushNotificationSenderFactory(
            VapidSettingsSection vapidSettingsSection,
            HttpClient httpClient)
        {
            this._vapidPushNotificationSenders = new ConcurrentDictionary<string, VapidPushNotificationSender>();
            this._settings = vapidSettingsSection ?? new VapidSettingsSection();
            this._httpClient = httpClient;
        }

        public VapidPushNotificationSenderFactory()
        {
            this._vapidPushNotificationSenders = new ConcurrentDictionary<string, VapidPushNotificationSender>();
        }

        private readonly HttpClient _httpClient;
        private readonly ConcurrentDictionary<string, VapidPushNotificationSender> _vapidPushNotificationSenders;
        private readonly VapidSettingsSection _settings;

        /// <inheritdoc/>
        public IVapidPushNotificationSender GetSender(
            string appName)
        {
            return this._vapidPushNotificationSenders.GetOrAdd(appName, this.ValueFactory);
        }

        private VapidPushNotificationSender ValueFactory(
            string arg)
        {
            if (!this._settings.ContainsKey(arg))
            {
                throw new ArgumentException($"{arg} is not defined in settings!");
            }

            var settings = this._settings[arg];
            return new VapidPushNotificationSender(settings, this._httpClient);
        }

        /// <inheritdoc/>
        public IVapidPushNotificationSender GetSender(
            string appName,
            AdsPushVapidSettings vapidSettings)
        {
            return this._vapidPushNotificationSenders.GetOrAdd(appName, new VapidPushNotificationSender(vapidSettings, this._httpClient));
        }
    }
}
