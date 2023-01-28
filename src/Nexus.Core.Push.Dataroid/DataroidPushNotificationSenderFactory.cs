using System;
using System.Collections.Concurrent;
using System.Net.Http;
using Nexus.Core.Push.Abstraction.Settings;
using Nexus.Core.Push.Dataroid.Settings;

namespace Nexus.Core.Push.Dataroid
{
    public class DataroidPushNotificationSenderFactory : IDataroidPushNotificationSenderFactory
    {
        public DataroidPushNotificationSenderFactory(
            DataroidSettingsSection apnsSettingsSection,
            HttpClient httpClient)
        {
            this._dataroidPushNotificationSenders = new ConcurrentDictionary<string, DataroidPushNotificationSender>();
            this._settings = apnsSettingsSection ?? new DataroidSettingsSection();
            this._httpClient = httpClient;
        }

        private readonly HttpClient _httpClient;
        private readonly ConcurrentDictionary<string, DataroidPushNotificationSender> _dataroidPushNotificationSenders;
        private readonly DataroidSettingsSection _settings;


        /// <inheritdoc/>
        public IDataroidPushNotificationSender GetSender(
            string appName)
        {
            return this._dataroidPushNotificationSenders.GetOrAdd(appName, this.ValueFactory);
        }

        private DataroidPushNotificationSender ValueFactory(
            string arg)
        {
            if (!this._settings.ContainsKey(arg))
            {
                throw new ArgumentException($"{arg} is not defined in settings!");
            }

            var settings = this._settings[arg];
            return new DataroidPushNotificationSender(settings, this._httpClient);
        }

        /// <inheritdoc/>
        public IDataroidPushNotificationSender GetSender(
            string appName,
            NexusPushDataroidSettings dataroidSettings)
        {
            return this._dataroidPushNotificationSenders.GetOrAdd(appName, new DataroidPushNotificationSender(dataroidSettings, this._httpClient));
        }
    }
}
