using System.Threading;
using System.Threading.Tasks;
using AdsPush.Abstraction.Settings;

namespace AdsPush.Abstraction
{
    /// <summary>
    /// Providers a interface to be able customize configuration provider source.
    /// </summary>
    public interface IAdsPushConfigurationProvider
    {
        /// <summary>
        /// Get settings for passing <paramref name="appName"/> 
        /// </summary>
        /// <param name="appName"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<AdsPushAppSettings> GetSettingsAsync(
            string appName,
            CancellationToken cancellationToken = default);
    }
}
