using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using SolidOps.TODO.Shared;

namespace MetaCorp.Template.Application;

public static class AssemblyReference
{
}

public partial class TemplateApplicationServiceRegistrar : IServiceRegistrar
{
    public int Priority => 1;
    public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        #region foreach DOMAIN_USECASE[I]
        services.AddTransient<Contracts.UseCases.IUSECASENAMEUseCase, Application.UseCases.USECASENAMEUseCase>();
        #endregion foreach DOMAIN_USECASE

        ConfigureAdditionalServices(services, configuration);
    }

    partial void ConfigureAdditionalServices(IServiceCollection services, IConfiguration configuration);
}