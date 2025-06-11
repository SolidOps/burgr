using Microsoft.Extensions.DependencyInjection;
using SolidOps.UM.Shared.Domain.Configuration;

namespace SolidOps.UM.Shared.Tests;

public interface IAppServiceInfo
{
    public string ServiceName { get; set; }
    public bool Remote { get; set; }
    public string Address { get; set; }
    public bool IsUsedForAuthentication { get; set; }
    public EventOption EventOption { get; set; }
    public Action<IServiceCollection> OnConfiguration { get; set; }
    public Action<int, IServiceProvider> OnAppServiceInitialize { get; set; }
    public Action<int, IServiceProvider> OnAppServiceCleanup { get; set; }
    public bool DelayInit { get; set; }
    public string ForcedSuffix { get; set; }
    public Type StartupType { get; }
}
public class AppServiceInfo<TStartup> : IAppServiceInfo
{
    public AppServiceInfo()
    {
        StartupType = typeof(TStartup);
    }
    public string ServiceName { get; set; }
    public bool Remote { get; set; }
    public string Address { get; set; }
    public bool IsUsedForAuthentication { get; set; }
    public EventOption EventOption { get; set; }
    public Action<IServiceCollection> OnConfiguration { get; set; }
    public Action<int, IServiceProvider> OnAppServiceInitialize { get; set; }
    public Action<int, IServiceProvider> OnAppServiceCleanup { get; set; }
    public bool DelayInit { get; set; }
    public string ForcedSuffix { get; set; }
    public Type StartupType { get; private set; }
}
