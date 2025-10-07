using AdsPush.Abstraction.Settings;
using Microsoft.Extensions.Options;
using Moq;

namespace AdsPush.Test;

public class ConfigurationProviderTests
{
    [Fact]
    public async Task BasicAdsPushConfigurationProviderReturnsSettings()
    {
        var settings = new AdsPushAppSettings();
        var provider = new BasicAdsPushConfigurationProvider(settings);

        var result = await provider.GetSettingsAsync("any");

        Assert.Same(settings, result);
    }

    [Fact]
    public async Task DefaultAdsPushConfigurationProviderReturnsSettingsFromOptions()
    {
        var settings = new AdsPushAppSettings();
        var adsPushSettings = new AdsPushSettings { ["app"] = settings };

        var optionsMonitor = new Mock<IOptionsMonitor<AdsPushSettings>>();
        optionsMonitor.Setup(o => o.CurrentValue).Returns(adsPushSettings);

        var provider = new DefaultAdsPushConfigurationProvider(optionsMonitor.Object);

        var result = await provider.GetSettingsAsync("app");

        Assert.Same(settings, result);
    }
}
