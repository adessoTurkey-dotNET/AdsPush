using System;
using System.Net.Http;
using AdsPush.Abstraction;
using AdsPush.Abstraction.Settings;
using AdsPush.APNS;
using AdsPush.Firebase;

namespace AdsPush;

public class AdsPushSenderBuilder
{
    private readonly AdsPushAppSettings _adsPushAppSettings;
    private HttpClient _apnsHttpClient = null;

    public AdsPushSenderBuilder()
    {
        this._adsPushAppSettings = new AdsPushAppSettings();
    }

    public AdsPushSenderBuilder ConfigureApns(
        AdsPushAPNSSettings settings,
        HttpClient httpClient = null)
    {
        this._adsPushAppSettings.Apns = settings;
        this._adsPushAppSettings.TargetMappings.Add(AdsPushTarget.Ios, AdsPushProvider.Apns);
        this._apnsHttpClient = httpClient ?? new HttpClient();

        return this;
    }

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

    public IAdsPushSender BuildSender()
    {
        var appName = Guid.NewGuid().ToString();
        var provider = new BasicAdsPushConfigurationProvider(this._adsPushAppSettings);

        var apnsFactory = _adsPushAppSettings.Apns != null
            ? new ApplePushNotificationSenderFactory(new()
            {
                {
                    appName, _adsPushAppSettings.Apns
                }
            }, _apnsHttpClient)
            : null;

        var firebaseFactory = _adsPushAppSettings.Firebase != null
            ? new FirebasePushNotificationSenderFactory(new()
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