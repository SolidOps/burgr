using Microsoft.Extensions.DependencyInjection;
using SolidOps.TODO.Shared;
using SolidOps.TODO.Shared.Infrastructure;
using SolidOps.TODO.Domain.Repositories;
using SolidOps.TODO.Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
namespace SolidOps.TODO.Infrastructure;
public static class AssemblyReference
{
}
public partial class TODOInfrastructureServiceRegistrar: IServiceRegistrar
{
    public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IDBContextFactory, TODODBContextFactory>();
        // Object [EN][AG]
        services.AddTransient<IItemRepository, ItemRepository>();

        ConfigureAdditionalServices(services, configuration);
    }
    partial void ConfigureAdditionalServices(IServiceCollection services, IConfiguration configuration);
}