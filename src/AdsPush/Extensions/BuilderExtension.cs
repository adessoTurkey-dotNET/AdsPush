using System;
using Microsoft.Extensions.DependencyInjection;
using AdsPush.Abstraction;
using AdsPush.Abstraction.Settings;
using AdsPush.APNS.Extensions;
using AdsPush.Firebase.Extensions;
using Microsoft.Extensions.Configuration;

namespace AdsPush.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public static class BuilderExtension
    {
        /// <summary>
        /// Configures AdsPush service by passing the required settings.
        /// <seealso cref="AdsPushAppSettings"/>
        /// </summary>
        /// <param name="services"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static IServiceCollection AddAdsPush(
            this IServiceCollection services,
            Action<AdsPushSettings> settings)
        {
            services.AddFirebaseCloudMessagingServiceFactory();
            services.AddAppleNotificationServiceFactory();
            services.AddSingleton<IAdsPushSenderFactory, AdsPushSenderFactory>();
            services.AddSingleton<IAdsPushConfigurationProvider, DefaultAdsPushConfigurationProvider>();
            services.Configure(settings);

            return services;
        }

        /// <summary>
        /// Configure AdsPush using default Microsoft Options Pattern configuration.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddAdsPush(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddFirebaseCloudMessagingServiceFactory();
            services.AddAppleNotificationServiceFactory();
            services.AddSingleton<IAdsPushSenderFactory, AdsPushSenderFactory>();
            services.AddSingleton<IAdsPushConfigurationProvider, DefaultAdsPushConfigurationProvider>();
            services.Configure<AdsPushSettings>(configuration.GetSection("AdsPush"));

            return services;
        }

        /// <summary>
        /// Configures AdsPush services by using custom settings provider.
        /// </summary>
        /// <param name="services"></param>
        /// <typeparam name="TProvider">The provider that provides required settings. <see cref="IAdsPushConfigurationProvider"/></typeparam>
        /// <returns></returns>
        public static IServiceCollection AddAdsPush<TProvider>(
            this IServiceCollection services) where TProvider : class, IAdsPushConfigurationProvider
        {
            services.AddFirebaseCloudMessagingServiceFactory();
            services.AddAppleNotificationServiceFactory();
            services.AddSingleton<IAdsPushSenderFactory, AdsPushSenderFactory>();
            services.AddSingleton<IAdsPushConfigurationProvider, TProvider>();

            return services;
        }
    }
}
