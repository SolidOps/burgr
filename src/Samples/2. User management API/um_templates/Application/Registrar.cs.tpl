using Microsoft.Extensions.DependencyInjection;
using SolidOps.UM.Shared.Domain.Configuration;
using SolidOps.UM.Shared.Domain.CrossCutting;
using SolidOps.UM.Shared.Application.Events;

namespace MetaCorp.Template.Application;

public static class AssemblyReference
{
}

public partial class TemplateApplicationServiceRegistrar : IServiceRegistrar
{
    public int Priority => 1;
    public void ConfigureServices(IServiceCollection services, IExtendedConfiguration configuration)
    {
        services.AddAsyncInitializer<TemplateApplicationInitializer>();

        #region foreach MODEL

        #region foreach CONSUMEDEVENT
        if (!configuration.Subscriptions.Contains("CONSUMEDEVENTTYPE"))
            configuration.Subscriptions.Add("CONSUMEDEVENTTYPE");
        services.AddTransient<IEventHandler<CONSUMEDEVENTTYPE>, EventHandlers.CLASSNAMEEventHandler>();
        #endregion foreach CONSUMEDEVENT

        #endregion foreach MODEL


        if (configuration["Legacy:Template"] != "True")
        {
            #region foreach DOMAIN_USECASE[I]
            services.AddTransient<Application.UseCases.IUSECASENAMEUseCase, Application.UseCases.USECASENAMEUseCase>();
            #endregion foreach DOMAIN_USECASE
        }

        ConfigureAdditionalServices(services, configuration);
    }

    partial void ConfigureAdditionalServices(IServiceCollection services, IExtendedConfiguration configuration);
}