using Microsoft.Extensions.DependencyInjection;
using SolidOps.Burgr.Shared.Domain.Configuration;
using SolidOps.Burgr.Shared.Domain.CrossCutting;
using SolidOps.Burgr.Shared.Domain.Entities;
namespace SolidOps.UM.Presentation;
public static class AssemblyReference
{
}
public partial class UMPresentationServiceRegistrator : IServiceRegistrator
{
    public int Priority => 1;
    public void ConfigureServices(IServiceCollection services, IExtendedConfiguration configuration)
    {
        services.AddAsyncInitializer<UMPresentationInitializer>();
        // Object [EN][AG][CACHE]
        services.AddTransient<IEntityRules<Guid, Domain.AggregateRoots.Organization>, Presentation.Cache.OrganizationCacheRules>();

        ConfigureAdditionalServices(services, configuration);
    }
    partial void ConfigureAdditionalServices(IServiceCollection services, IExtendedConfiguration configuration);
}