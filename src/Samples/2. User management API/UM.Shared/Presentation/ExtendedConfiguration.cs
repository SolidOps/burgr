using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SolidOps.UM.Shared.Contracts.Endpoints;
using SolidOps.UM.Shared.Contracts.Results;
using SolidOps.UM.Shared.Domain.Configuration;

namespace SolidOps.UM.Shared.Presentation;

public class ExtendedConfiguration : IExtendedConfiguration
{
    private readonly IConfiguration configuration;
    private BurgrConfiguration burgrConfiguration = null;

    public ExtendedConfiguration(IConfiguration configuration)
    {
        this.configuration = configuration;
        this.Subscriptions = new List<string>();
    }

    public string this[string key]
    {
        get
        {
            return this.configuration[key];
        }
        set
        {
            this.configuration[key] = value;
        }
    }

    public BurgrConfiguration BurgrConfiguration
    {
        get
        {
            if (this.burgrConfiguration == null)
                this.burgrConfiguration = new BurgrConfiguration(this.configuration);
            return burgrConfiguration;
        }
        set
        {
            this.burgrConfiguration = value;
        }
    }

    public IConfiguration InnerConfiguration => configuration;

    public IConfigurationSection GetSection(string key)
    {
        var val = this.configuration.GetSection(key);
        return val;
    }

    public List<string> Subscriptions { get; set; }

    public async Task<IOpsResult> Reload(IServiceProvider serviceProvider)
    {
        if (this.configuration is IConfigurationRoot root)
        {
            root.Reload();
        }

        if (this["Configuration:Remote"] == "True")
        {
            var remoteConfigurationService = serviceProvider.GetService<IRemoteConfigurationService>();
            var token = serviceProvider.GetRequiredService<IApplicationToken>().Token;

            if (remoteConfigurationService != null)
            {
                var result = await remoteConfigurationService.GetRemoteConfiguration(this["Application"], this["Environment"], token);
                if(result.HasError) return result;
                foreach (var kvp in result.Data)
                {
                    this[kvp.Key] = kvp.Value;
                }
            }
        }
        if (Override != null)
        {
            foreach (var kvp in Override)
            {
                this[kvp.Key] = kvp.Value;
            }
        }

        this.burgrConfiguration = null;

        return IOpsResult.Ok();
    }

    public Dictionary<string, string> TechnicalUsers
    {
        get
        {
            return configuration.GetChildren()
                .SingleOrDefault(c => c.Key == "TechUsers")
                ?.GetChildren().ToDictionary(x => x.Key, x => x.Value);
        }
    }

    public Dictionary<string, string> Override { get; set; }

    public T GetValue<T>(string key)
    {
        return configuration.GetValue<T>(key);
    }
}
