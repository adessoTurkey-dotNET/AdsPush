using System;
using System.Net.Http;
using AdsPush.Abstraction;
using AdsPush.Abstraction.Settings;
using AdsPush.APNS;
using AdsPush.APNS.Settings;
using AdsPush.Firebase;
using AdsPush.Firebase.Settings;
using AdsPush.HMS;
using AdsPush.Vapid;
using AdsPush.Vapid.Settings;

namespace AdsPush
{
    /// <summary>
    /// Use to create <see cref="IAdsPushSender"/> instance.
    /// </summary>
    public class AdsPushSenderBuilder
    {
        private readonly AdsPushAppSettings _adsPushAppSettings;
        private HttpClient _apnsHttpClient;
        private HttpClient _vapidHttpClient;
        private HttpClient _hmsHttpClient;
        private HttpClient _hmsAuthHttpClient;

        /// <summary>
        ///
        /// </summary>
        public AdsPushSenderBuilder()
        {
            this._adsPushAppSettings = new AdsPushAppSettings();
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
            this._adsPushAppSettings.Apns = settings;
            this._adsPushAppSettings.TargetMappings.Add(AdsPushTarget.Ios, AdsPushProvider.Apns);
            this._apnsHttpClient = httpClient ?? new HttpClient();

            return this;
        }

        public AdsPushSenderBuilder ConfigureVapid(
            AdsPushVapidSettings settings,
            HttpClient httpClient = null)
        {
            this._adsPushAppSettings.Vapid = settings;
            this._adsPushAppSettings.TargetMappings.Add(AdsPushTarget.BrowserAndPwa, AdsPushProvider.VapidWebPush);
            this._vapidHttpClient = httpClient ?? new HttpClient();

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
            this._adsPushAppSettings.Firebase = settings;
            foreach (var target in targets)
            {
                this._adsPushAppSettings.TargetMappings[target] = AdsPushProvider.Firebase;
            }

            return this;
        }


        /// <summary>
        /// Use to configure Huawei Messaging System for sender.
        /// </summary>
        /// <param name="settings"></param>
        /// <param name="targets"></param>
        /// <returns></returns>
        public AdsPushSenderBuilder ConfigureHMS(
            AdsPushHMSSettings settings,
            HttpClient httpClient = null,
            HttpClient authHttpClient = null,
            params AdsPushTarget[] targets)
        {
            // Can be used as a provider for different type of target os like ios, android, huawei
            this._adsPushAppSettings.HMS = settings;
            foreach (var target in targets)
            {
                this._adsPushAppSettings.TargetMappings[target] = AdsPushProvider.HMS;
            }
            this._hmsHttpClient = httpClient ?? new HttpClient();
            this._hmsAuthHttpClient = authHttpClient ?? new HttpClient();

            return this;
        }

        public AdsPushSenderBuilder ConfigureHMS(
           AdsPushHMSSettings settings,
           params AdsPushTarget[] targets)
        {
            // Can be used as a provider for different type of target os like ios, android, huawei
            this._adsPushAppSettings.HMS = settings;
            foreach (var target in targets)
            {
                this._adsPushAppSettings.TargetMappings[target] = AdsPushProvider.HMS;
            }
            this._hmsHttpClient = new HttpClient();
            this._hmsAuthHttpClient = new HttpClient();

            return this;
        }

        /// <summary>
        /// Build the configured sender.
        /// </summary>
        /// <returns></returns>
        public IAdsPushSender BuildSender()
        {
            var appName = Guid.NewGuid().ToString();
            var provider = new BasicAdsPushConfigurationProvider(this._adsPushAppSettings);

            var apnsFactory = this._adsPushAppSettings.Apns != null
                ? new ApplePushNotificationSenderFactory(new APNSSettingsSection
                {
                    {
                        appName, this._adsPushAppSettings.Apns
                    }
                }, this._apnsHttpClient)
                : null;

            var firebaseFactory = this._adsPushAppSettings.Firebase != null
                ? new FirebasePushNotificationSenderFactory(new FirebaseAppSettingsSection
                {
                    {
                        appName, this._adsPushAppSettings.Firebase
                    }
                })
                : null;

            var vapidFactory = this._adsPushAppSettings.Vapid != null
                ? new VapidPushNotificationSenderFactory(new VapidSettingsSection()
                {
                    {
                        appName, this._adsPushAppSettings.Vapid
                    }
                }, this._vapidHttpClient)
                : null;

            var hmsFactory = this._adsPushAppSettings.HMS != null
               ? new HMSPushNotificationSenderFactory(new HMS.Settings.HMSSettingsSection
               {
                    {
                        appName, this._adsPushAppSettings.HMS
                    }
               }, this._hmsHttpClient, this._hmsAuthHttpClient)
               : null;

            return new AdsPushSender(
                appName,
                provider,
                firebaseFactory,
                apnsFactory,
                vapidFactory,
                hmsFactory);
        }
    }
}
