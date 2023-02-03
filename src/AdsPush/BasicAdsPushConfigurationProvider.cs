using System.Threading;
using System.Threading.Tasks;
using AdsPush.Abstraction;
using AdsPush.Abstraction.Settings;

namespace AdsPush;

internal class BasicAdsPushConfigurationProvider : IAdsPushConfigurationProvider
{
    private readonly AdsPushAppSettings _adsPushAppSettings;

    public BasicAdsPushConfigurationProvider(AdsPushAppSettings adsPushAppSettings)
    {
        _adsPushAppSettings = adsPushAppSettings;
    }
    
    public Task<AdsPushAppSettings> GetSettingsAsync(
        string appName,
        CancellationToken cancellationToken = default)
    {
        return Task.FromResult(this._adsPushAppSettings);
    }
}