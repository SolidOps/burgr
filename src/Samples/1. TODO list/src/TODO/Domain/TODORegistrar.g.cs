using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SolidOps.TODO.Shared;
using SolidOps.TODO.Shared.Domain;
namespace SolidOps.TODO.Domain;
public static class AssemblyReference
{
}
public partial class TODODomainServiceRegistrar : IServiceRegistrar
{
    public int Priority => 1;
    public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        // Object [EN][AG]
        services.AddTransient<IEntityRules<Guid, Domain.AggregateRoots.Item>, Domain.AggregateRoots.Rules.ItemRules>();

        ConfigureAdditionalServices(services, configuration);
    }
    partial void ConfigureAdditionalServices(IServiceCollection services, IConfiguration configuration);
}