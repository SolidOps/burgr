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
        #region foreach DOMAIN_SERVICE
        services.AddTransient<Contracts.Services.ISERVICENAMEService, Application.Services.SERVICENAMEService>();
        #endregion foreach DOMAIN_SERVICE

        ConfigureAdditionalServices(services, configuration);
    }

    partial void ConfigureAdditionalServices(IServiceCollection services, IConfiguration configuration);
}