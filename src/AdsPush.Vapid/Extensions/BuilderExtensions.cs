using System;
using System.Net.Http;
using AdsPush.Vapid.Settings;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace AdsPush.Vapid.Extensions
{
    public static class BuilderExtensions
    {

        /// <summary>
        /// Configures <see cref="IVapidPushNotificationSenderFactory"/> to be able to creates <see cref="IVapidPushNotificationSender"/> instance.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="vapidSettingsSectionsAction"></param>
        /// <param name="httpClient"></param>
        /// <returns></returns>
        public static IServiceCollection AddVapidNotificationServiceFactory(
            this IServiceCollection services,
            Action<VapidSettingsSection> vapidSettingsSectionsAction = null,
            HttpClient httpClient = null)
        {
            var vapidSettingsSection = new VapidSettingsSection();
            vapidSettingsSectionsAction?.Invoke(vapidSettingsSection);

            services.AddSingleton<IVapidPushNotificationSenderFactory>(serviceProvider =>
            {
                vapidSettingsSection = vapidSettingsSectionsAction is null
                    ? serviceProvider.GetService<IOptions<VapidSettingsSection>>()?.Value
                    : vapidSettingsSection;

                return new VapidPushNotificationSenderFactory(vapidSettingsSection, httpClient ?? new HttpClient());
            });

            return services;
        }
    }
}
