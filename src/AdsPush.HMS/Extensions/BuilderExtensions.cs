using System;
using System.Net.Http;
using AdsPush.HMS.Settings;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace AdsPush.HMS.Extensions
{
    public static class BuilderExtensions
    {
        /// <summary>
        /// Configures <see cref="IHMSPushNotificationSenderFactory"/> to be able to creates <see cref="IApplePushNotificationSender"/> instance.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="hmsSettingsSectionsAction"></param>
        /// <param name="httpClient"></param>
        /// <returns></returns>
        ///
        public static IServiceCollection AddHuaweiNotificationServiceFactory(
            this IServiceCollection services,
            Action<HMSSettingsSection> hmsSettingsSectionsAction = null,
            HttpClient httpClient = null,
            HttpClient authHttpClient = null)
        {
            var hmsSettingsSection = new HMSSettingsSection();
            hmsSettingsSectionsAction?.Invoke(hmsSettingsSection);

            services.AddSingleton<IHMSPushNotificationSenderFactory>(serviceProvider =>
            {
                hmsSettingsSection = hmsSettingsSectionsAction is null
                ? serviceProvider.GetService<IOptions<HMSSettingsSection>>()?.Value
                : hmsSettingsSection;

                return new HMSPushNotificationSenderFactory(hmsSettingsSection, httpClient ?? new HttpClient(), authHttpClient ?? new HttpClient());
            });
            return services;
        }



    }
}
