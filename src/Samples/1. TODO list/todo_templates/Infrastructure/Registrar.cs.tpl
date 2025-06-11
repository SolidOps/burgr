using Microsoft.Extensions.DependencyInjection;
using SolidOps.TODO.Shared;
using SolidOps.TODO.Shared.Infrastructure;
using MetaCorp.Template.Domain.Repositories;
using MetaCorp.Template.Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;

namespace MetaCorp.Template.Infrastructure;

public static class AssemblyReference
{
}

public partial class TemplateInfrastructureServiceRegistrar: IServiceRegistrar
{
    public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IDBContextFactory, TemplateDBContextFactory>();

        #region foreach MODEL[EN][AG]
        services.AddTransient<ICLASSNAMERepository, CLASSNAMERepository>();

        #endregion foreach MODEL

        ConfigureAdditionalServices(services, configuration);
    }

    partial void ConfigureAdditionalServices(IServiceCollection services, IConfiguration configuration);
}