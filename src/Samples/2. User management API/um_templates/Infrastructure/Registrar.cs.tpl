using Microsoft.Extensions.DependencyInjection;
using SolidOps.UM.Shared.Domain.Configuration;
using SolidOps.UM.Shared.Domain.CrossCutting;
using SolidOps.UM.Shared.Domain.UnitOfWork;
using SolidOps.UM.Shared.Infrastructure;
using SolidOps.UM.Shared.Infrastructure.EF;
using MetaCorp.Template.Domain.Repositories;
using MetaCorp.Template.Infrastructure.EF;
using MetaCorp.Template.Infrastructure.Repositories;

namespace MetaCorp.Template.Infrastructure;

public static class AssemblyReference
{
}

public partial class TemplateInfrastructureServiceRegistrar : BaseInfrastructureServiceRegistrar, IServiceRegistrar
{
    public int Priority => 1;
    public void ConfigureServices(IServiceCollection services, IExtendedConfiguration configuration)
    {
        services.AddSingleton<IBurgrDBContextFactory, TemplateDBContextFactory>();

        #region foreach MODEL[EN][AG]
        services.AddTransient<ICLASSNAMERepository, CLASSNAMERepository>();

        #endregion foreach MODEL

        ConfigureAdditionalServices(services, configuration);
    }

    partial void ConfigureAdditionalServices(IServiceCollection services, IExtendedConfiguration configuration);
}