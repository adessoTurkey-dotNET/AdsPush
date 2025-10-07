using System.Threading;
using System.Threading.Tasks;
using AdsPush.Abstraction;
using AdsPush.Abstraction.Settings;
using Microsoft.Extensions.Options;

namespace AdsPush
{
    /// <summary>
    /// Use to configure Microsoft default.
    /// </summary>
    public class DefaultAdsPushConfigurationProvider : IAdsPushConfigurationProvider
    {
        private readonly IOptionsMonitor<AdsPushSettings> _options;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="options"></param>
        public DefaultAdsPushConfigurationProvider(
            IOptionsMonitor<AdsPushSettings> options)
        {
            this._options = options;
        }

        /// <inheritdoc />
        public Task<AdsPushAppSettings> GetSettingsAsync(
            string appName,
            CancellationToken cancellationToken = default)
        {
            return Task.FromResult(this._options.CurrentValue[appName]);
        }
    }
}
