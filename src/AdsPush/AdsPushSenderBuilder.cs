using System;
using System.Net.Http;
using AdsPush.Abstraction;
using AdsPush.Abstraction.Settings;
using AdsPush.APNS;
using AdsPush.APNS.Settings;
using AdsPush.Firebase;
using AdsPush.Firebase.Settings;

namespace AdsPush
{
    /// <summary>
    /// Use to create <see cref="IAdsPushSender"/> instance.
    /// </summary>
    public class AdsPushSenderBuilder
    {
        private readonly AdsPushAppSettings _adsPushAppSettings;
        private HttpClient _apnsHttpClient;
    
        /// <summary>
        /// 
        /// </summary>
        public AdsPushSenderBuilder()
        {
            _adsPushAppSettings = new AdsPushAppSettings();
        }
    
        /// <summary>
        /// Use to configure APNS for sender.
        /// </summary>
        /// <param name="settings"></param>
        /// <param name="httpClient"></param>
        /// <returns></returns>
        public AdsPushSenderBuilder ConfigureApns(
            AdsPushAPNSSettings settings,
            HttpClient httpClient = null)
        {
            _adsPushAppSettings.Apns = settings;
            _adsPushAppSettings.TargetMappings.Add(AdsPushTarget.Ios, AdsPushProvider.Apns);
            _apnsHttpClient = httpClient ?? new HttpClient();
    
            return this;
        }
    
        /// <summary>
        /// Use to configure Firebase Cloud Messaging for sender.
        /// </summary>
        /// <param name="settings"></param>
        /// <param name="targets"></param>
        /// <returns></returns>
        public AdsPushSenderBuilder ConfigureFirebase(
            AdsPushFirebaseSettings settings,
            params AdsPushTarget[] targets)
        {
            _adsPushAppSettings.Firebase = settings;
            foreach (var target in targets)
            {
                _adsPushAppSettings.TargetMappings[target] = AdsPushProvider.Firebase;
            }
            
            return this;
        }
    
        /// <summary>
        /// Build the configured sender.
        /// </summary>
        /// <returns></returns>
        public IAdsPushSender BuildSender()
        {
            var appName = Guid.NewGuid().ToString();
            var provider = new BasicAdsPushConfigurationProvider(_adsPushAppSettings);
    
            var apnsFactory = _adsPushAppSettings.Apns != null
                ? new ApplePushNotificationSenderFactory(new APNSSettingsSection
                {
                    {
                        appName, _adsPushAppSettings.Apns
                    }
                }, _apnsHttpClient)
                : null;
    
            var firebaseFactory = _adsPushAppSettings.Firebase != null
                ? new FirebasePushNotificationSenderFactory(new FirebaseAppSettingsSection
                {
                    {
                        appName, _adsPushAppSettings.Firebase
                    }
                })
                : null;
    
            return new AdsPushSender(
                appName,
                provider,
                firebaseFactory,
                apnsFactory);
        }
    }
}