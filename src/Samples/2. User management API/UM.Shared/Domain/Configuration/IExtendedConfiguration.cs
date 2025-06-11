using Microsoft.Extensions.Configuration;
using SolidOps.UM.Shared.Contracts.Results;

namespace SolidOps.UM.Shared.Domain.Configuration;

public interface IExtendedConfiguration
{
    string this[string key] { get; set; }
    IConfigurationSection GetSection(string key);
    T GetValue<T>(string key);
    BurgrConfiguration BurgrConfiguration { get; set; }
    List<string> Subscriptions { get; }
    Task<IOpsResult> Reload(IServiceProvider serviceProvider);
    Dictionary<string, string> TechnicalUsers { get; }
    Dictionary<string, string> Override { get; set; }
}

public interface IAdditionalConfiguration
{
    Dictionary<string, string> Values { get; }
}
