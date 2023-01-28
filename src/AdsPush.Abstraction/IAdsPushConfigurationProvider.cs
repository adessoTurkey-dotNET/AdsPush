using System.Threading;
using System.Threading.Tasks;
using AdsPush.Abstraction.Settings;

namespace AdsPush.Abstraction
{
    public interface IAdsPushConfigurationProvider
    {
        Task<AdsPushAppSettings> GetSettingsAsync(
            string appName,
            CancellationToken cancellationToken = default);
    }
}
