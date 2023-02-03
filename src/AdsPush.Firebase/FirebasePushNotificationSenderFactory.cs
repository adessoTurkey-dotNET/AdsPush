using System;
using System.Collections.Concurrent;
using AdsPush.Abstraction.Settings;
using AdsPush.Firebase.Settings;

namespace AdsPush.Firebase
{
    public class FirebasePushNotificationSenderFactory : IFirebasePushNotificationSenderFactory
    {
        public FirebasePushNotificationSenderFactory(FirebaseAppSettingsSection firebaseAppSettingsSection)
        {
            this._firebasePushNotificationSenders = new ConcurrentDictionary<string, FirebasePushNotificationSender>();
            this._settings = firebaseAppSettingsSection ?? new FirebaseAppSettingsSection();
        }

        private readonly ConcurrentDictionary<string, FirebasePushNotificationSender> _firebasePushNotificationSenders;
        private readonly FirebaseAppSettingsSection _settings;

        /// <inheritdoc/>
        public IFirebasePushNotificationSender GetSender(
            string appName)
        {
            return this._firebasePushNotificationSenders.GetOrAdd(appName, this.ValueFactory);
        }

        private FirebasePushNotificationSender ValueFactory(
            string arg)
        {
            if (!this._settings.ContainsKey(arg))
            {
                throw new ArgumentException($"{arg} is not defined in settings!");
            }

            var settings = this._settings[arg];
            return new FirebasePushNotificationSender(settings);
        }

        /// <inheritdoc/>
        public IFirebasePushNotificationSender GetSender(
            string appName,
            AdsPushFirebaseSettings settings)
        {
            return this._firebasePushNotificationSenders.GetOrAdd(appName, new FirebasePushNotificationSender(settings));
        }
    }
}
