using Microsoft.Extensions.DependencyInjection;
using SolidOps.Burgr.Shared.Domain.Configuration;
using SolidOps.Burgr.Shared.Domain.CrossCutting;
using SolidOps.Burgr.Shared.Domain.UnitOfWork;
using SolidOps.Burgr.Shared.Infrastructure;
using SolidOps.UM.Domain.Repositories;
using SolidOps.UM.Infrastructure.Repositories;
namespace SolidOps.UM.Infrastructure;
public static class AssemblyReference
{
}
public partial class UMInfrastructureServiceRegistrator : BaseInfrastructureServiceRegistrator, IServiceRegistrator
{
    public int Priority => 1;
    public void ConfigureServices(IServiceCollection services, IExtendedConfiguration configuration)
    {
        var dataFactory = InstantiateDataAccessFactory(configuration, "UM", "Module", false) as IUMDataAccessFactory;
        if (dataFactory != null)
        {
            services.AddSingleton<IUMDataAccessFactory>(dataFactory);
            services.AddSingletonFromExisting<IDataAccessFactory, IUMDataAccessFactory>();
        }
        // Object [EN][AG]
        services.AddTransient<IApplicationRepository, ApplicationRepository>();

        services.AddTransient<IApplicationEnvironmentRepository, ApplicationEnvironmentRepository>();

        services.AddTransient<IEnvironmentRepository, EnvironmentRepository>();

        services.AddTransient<ILocalUserRepository, LocalUserRepository>();

        services.AddTransient<IUserRepository, UserRepository>();

        services.AddTransient<IUserRoleRepository, UserRoleRepository>();

        services.AddTransient<IRoleRepository, RoleRepository>();

        services.AddTransient<IOrganizationRepository, OrganizationRepository>();

        services.AddTransient<IUserOrganizationRepository, UserOrganizationRepository>();

        services.AddTransient<IInviteRepository, InviteRepository>();

        ConfigureAdditionalServices(services, configuration);
    }
    partial void ConfigureAdditionalServices(IServiceCollection services, IExtendedConfiguration configuration);
}