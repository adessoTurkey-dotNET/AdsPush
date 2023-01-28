using System;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Nexus.Core.Push.Abstraction.Settings;
using Nexus.Core.Push.Dataroid.Settings;

namespace Nexus.Core.Push.Dataroid.Extensions
{
    public static class BuilderExtensions
    {
        /// <summary>
        /// Registers <see cref="IDataroidPushNotificationSender"/> to be able send notification to the Dataroid.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="dataroidSettingsAction"></param>
        /// <param name="httpClient"></param>
        /// <returns></returns>
        public static IServiceCollection AddDataroidNotificationService(
            this IServiceCollection services,
            Action<NexusPushDataroidSettings> dataroidSettingsAction = null,
            HttpClient httpClient = null)
        {
            var dataroidSettings = new NexusPushDataroidSettings();
            dataroidSettingsAction?.Invoke(dataroidSettings);

            services.AddSingleton<IDataroidPushNotificationSender>(sender =>
            {
                dataroidSettings = dataroidSettings ?? sender.GetRequiredService<IOptions<NexusPushDataroidSettings>>().Value;

                return new DataroidPushNotificationSender(dataroidSettings, httpClient ?? new HttpClient());
            });

            return services;
        }

        /// <summary>
        /// Configures <see cref="IDataroidPushNotificationSenderFactory"/> to be able to creates <see cref="IDataroidPushNotificationSender"/> instance.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="dataroidSettingsSectionsAction"></param>
        /// <param name="httpClient"></param>
        /// <returns></returns>
        public static IServiceCollection AddDataroidNotificationServiceFactory(
            this IServiceCollection services,
            Action<DataroidSettingsSection> dataroidSettingsSectionsAction = null,
            HttpClient httpClient = null)
        {
            var dataroidSettingsSection = new DataroidSettingsSection();
            dataroidSettingsSectionsAction?.Invoke(dataroidSettingsSection);

            services.AddSingleton<IDataroidPushNotificationSenderFactory>(serviceProvider =>
            {
                dataroidSettingsSection = dataroidSettingsSectionsAction is null
                    ? serviceProvider.GetService<IOptions<DataroidSettingsSection>>()?.Value
                    : dataroidSettingsSection;

                return new DataroidPushNotificationSenderFactory(dataroidSettingsSection, httpClient ?? new HttpClient());
            });

            return services;
        }
    }
}
