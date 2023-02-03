using System;
using System.Threading;
using System.Threading.Tasks;
using AdsPush.Abstraction;
using AdsPush.APNS;
using AdsPush.Firebase;

namespace AdsPush
{
    public class AdsPushSender : IAdsPushSender
    {
        private readonly string _appName;
        private readonly IAdsPushConfigurationProvider _adsPushConfigurationProvider;
        private readonly IFirebasePushNotificationSenderFactory _firebasePushNotificationSenderFactory;
        private readonly IApplePushNotificationSenderFactory _applePushNotificationSenderFactory;

        public AdsPushSender(
            string appName,
            IAdsPushConfigurationProvider adsPushConfigurationProvider,
            IFirebasePushNotificationSenderFactory firebasePushNotificationSenderFactory,
            IApplePushNotificationSenderFactory applePushNotificationSenderFactory)
        {
            this._appName = appName;
            this._adsPushConfigurationProvider = adsPushConfigurationProvider;
            this._firebasePushNotificationSenderFactory = firebasePushNotificationSenderFactory;
            this._applePushNotificationSenderFactory = applePushNotificationSenderFactory;
        }


        /// <inheritdoc />
        public async Task BasicSendAsync(
            AdsPushTarget target,
            string pushToken,
            AdsPushBasicSendPayload payload,
            CancellationToken cancellationToken = default)
        {
            var settings = await this._adsPushConfigurationProvider.GetSettingsAsync(this._appName, cancellationToken);
            if (!settings.TargetMappings.ContainsKey(target))
            {
                throw new AdsPushException(
                    $"Settings are not configured for target platform {target}",
                    AdsPushErrorType.InvalidAuthConfiguration,
                    null);
            }

            var provider = settings.TargetMappings[target];
            switch (provider)
            {
                case AdsPushProvider.Apns:

                    if (target is not AdsPushTarget.Ios)
                    {
                        throw new AdsPushException(
                            $"{target} target don't support sending wia APNS. ",
                            AdsPushErrorType.InvalidArgument,
                            null);
                    }

                    if (settings.Apns is null)
                    {
                        throw new AdsPushException(
                            $"Settings are not configured for target platform {target}. Configure APNS to be able to proceed.",
                            AdsPushErrorType.InvalidAuthConfiguration,
                            null);
                    }

                    await this._applePushNotificationSenderFactory
                        .GetSender(this._appName, settings.Apns)
                        .SendAsync(pushToken, payload, cancellationToken);
                    break;
                case AdsPushProvider.Firebase:

                    if (settings.Firebase is null)
                    {
                        throw new AdsPushException(
                            $"Settings are not configured for target platform {target}. Configure FCM to be able to proceed.",
                            AdsPushErrorType.InvalidAuthConfiguration,
                            null);
                    }

                    await this._firebasePushNotificationSenderFactory
                        .GetSender(this._appName, settings.Firebase)
                        .SendAsync(
                            target,
                            pushToken,
                            payload,
                            cancellationToken);

                    break;
                default:
                    throw new NotSupportedException($"Target {target} is not supported by Framework");
            }
        }

        public IApplePushNotificationSender GetApnsSender()
        {
            return _applePushNotificationSenderFactory.GetSender(this._appName);
        }

        public IFirebasePushNotificationSender GetFirebaseSender()
        {
            return this._firebasePushNotificationSenderFactory.GetSender(this._appName);
        }
    }
}
