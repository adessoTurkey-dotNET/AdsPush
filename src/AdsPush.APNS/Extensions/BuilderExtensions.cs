using System;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using AdsPush.Abstraction.Settings;
using AdsPush.APNS.Settings;

namespace AdsPush.APNS.Extensions
{
    public static class BuilderExtensions
    {
  
        /// <summary>
        /// Configures <see cref="IApplePushNotificationSenderFactory"/> to be able to creates <see cref="IApplePushNotificationSender"/> instance.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="apnsSettingsSectionsAction"></param>
        /// <param name="httpClient"></param>
        /// <returns></returns>
        public static IServiceCollection AddAppleNotificationServiceFactory(
            this IServiceCollection services,
            Action<APNSSettingsSection> apnsSettingsSectionsAction = null,
            HttpClient httpClient = null)
        {
            var apnsSettingsSection = new APNSSettingsSection();
            apnsSettingsSectionsAction?.Invoke(apnsSettingsSection);

            services.AddSingleton<IApplePushNotificationSenderFactory>(serviceProvider =>
            {
                apnsSettingsSection = apnsSettingsSectionsAction is null
                    ? serviceProvider.GetService<IOptions<APNSSettingsSection>>()?.Value
                    : apnsSettingsSection;

                return new ApplePushNotificationSenderFactory(apnsSettingsSection, httpClient ?? new HttpClient());
            });

            return services;
        }
    }
}
