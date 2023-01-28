using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using AdsPush.Abstraction.Settings;
using AdsPush.Firebase.Settings;

namespace AdsPush.Firebase.Extensions
{
    public static class BuilderExtension
    {
        /// <summary>
        /// Registers <see cref="IFirebasePushNotificationSender"/> to be able send notification wia FCM.
        /// <seealso cref="AdsPushFirebaseSettings"/>
        /// </summary>
        /// <param name="services"></param>
        /// <param name="settingsAction"></param>
        /// <returns></returns>
        public static IServiceCollection AddFirebaseCloudMessagingService(
            this IServiceCollection services,
            Action<AdsPushFirebaseSettings> settingsAction)
        {
            var settings = new AdsPushFirebaseSettings();
            settingsAction?.Invoke(settings);

            services.AddSingleton<IFirebasePushNotificationSender>(provider =>
            {
                settings = settingsAction is null
                    ? provider.GetRequiredService<IOptions<AdsPushFirebaseSettings>>().Value
                    : settings;

                return new FirebasePushNotificationSender(settings);
            });

            return services;
        }

        /// <summary>
        /// Configures <see cref="IFirebasePushNotificationSenderFactory"/> to be able yo creates <see cref="IFirebasePushNotificationSender"/> instance.
        /// For settings: <seealso cref="FirebaseAppSettingsSection"/> amd <seealso cref="AdsPushFirebaseSettings"/>
        /// </summary>
        /// <param name="services"></param>
        /// <param name="settingsSectionAction"></param>
        /// <returns></returns>
        public static IServiceCollection AddFirebaseCloudMessagingServiceFactory(
            this IServiceCollection services,
            Action<FirebaseAppSettingsSection> settingsSectionAction = null)
        {
            var settingsSection = new FirebaseAppSettingsSection();
            settingsSectionAction?.Invoke(settingsSection);

            services.AddSingleton<IFirebasePushNotificationSenderFactory>(provider =>
            {
                settingsSection = settingsSectionAction is null
                    ? provider.GetService<IOptions<FirebaseAppSettingsSection>>()?.Value
                    : settingsSection;

                return new FirebasePushNotificationSenderFactory(settingsSection);
            });

            return services;
        }
    }
}
