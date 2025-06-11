using Microsoft.Extensions.DependencyInjection;
using SolidOps.UM.Shared.Domain.Configuration;
using SolidOps.UM.Shared.Domain.CrossCutting;
using SolidOps.UM.Shared.Domain.UnitOfWork;
using SolidOps.UM.Shared.Infrastructure;
using SolidOps.UM.Shared.Infrastructure.EF;
using SolidOps.UM.Domain.Repositories;
using SolidOps.UM.Infrastructure.EF;
using SolidOps.UM.Infrastructure.Repositories;
namespace SolidOps.UM.Infrastructure;
public static class AssemblyReference
{
}
public partial class UMInfrastructureServiceRegistrar : BaseInfrastructureServiceRegistrar, IServiceRegistrar
{
    public int Priority => 1;
    public void ConfigureServices(IServiceCollection services, IExtendedConfiguration configuration)
    {
        services.AddSingleton<IBurgrDBContextFactory, UMDBContextFactory>();
        // Object [EN][AG]
        services.AddTransient<ILocalUserRepository, LocalUserRepository>();

        services.AddTransient<IUserRepository, UserRepository>();

        services.AddTransient<IUserRightRepository, UserRightRepository>();

        services.AddTransient<IRightRepository, RightRepository>();

        services.AddTransient<IInviteRepository, InviteRepository>();

        ConfigureAdditionalServices(services, configuration);
    }
    partial void ConfigureAdditionalServices(IServiceCollection services, IExtendedConfiguration configuration);
}