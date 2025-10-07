using AdsPush.Abstraction;
using AdsPush.Abstraction.Settings;
using AdsPush.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace AdsPush.Test;

public class BuilderExtensionTests
{
    [Fact]
    public void AddAdsPushWithSettingsRegistersDependencies()
    {
        var services = new ServiceCollection();
        services.AddAdsPush(options =>
        {
            options["app"] = new AdsPushAppSettings();
        });

        using var provider = services.BuildServiceProvider();
        Assert.IsType<AdsPushSenderFactory>(provider.GetRequiredService<IAdsPushSenderFactory>());
        Assert.IsType<DefaultAdsPushConfigurationProvider>(provider
            .GetRequiredService<IAdsPushConfigurationProvider>());
    }

    [Fact]
    public void AddAdsPushWithConfigurationRegistersDependencies()
    {
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?> { ["AdsPush:app:Firebase:ProjectId"] = "project" })
            .Build();

        var services = new ServiceCollection();
        services.AddAdsPush(configuration);

        using var provider = services.BuildServiceProvider();
        Assert.IsType<AdsPushSenderFactory>(provider.GetRequiredService<IAdsPushSenderFactory>());
        Assert.IsType<DefaultAdsPushConfigurationProvider>(provider
            .GetRequiredService<IAdsPushConfigurationProvider>());
        Assert.Equal("project",
            provider.GetRequiredService<IOptions<AdsPushSettings>>().Value["app"].Firebase.ProjectId);
    }

    [Fact]
    public void AddAdsPushWithCustomProviderRegistersType()
    {
        var services = new ServiceCollection();
        services.AddAdsPush<FakeProvider>();

        using var provider = services.BuildServiceProvider();
        Assert.IsType<AdsPushSenderFactory>(provider.GetRequiredService<IAdsPushSenderFactory>());
        Assert.IsType<FakeProvider>(provider.GetRequiredService<IAdsPushConfigurationProvider>());
    }

    private sealed class FakeProvider : IAdsPushConfigurationProvider
    {
        public Task<AdsPushAppSettings> GetSettingsAsync(string appName, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(new AdsPushAppSettings());
        }
    }
}
