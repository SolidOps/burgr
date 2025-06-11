using Microsoft.Extensions.DependencyInjection;
using SolidOps.UM.Shared.Application;
using SolidOps.UM.Shared.Contracts.Endpoints;
using SolidOps.UM.Shared.Domain.Configuration;
using SolidOps.UM.Shared.Domain.CrossCutting;
using SolidOps.UM.Shared.Infrastructure;

namespace SolidOps.UM.Shared.Presentation;

public class CoreServiceRegistrator : IServiceRegistrar
{
    public int Priority => 0;

    public void ConfigureServices(IServiceCollection services, IExtendedConfiguration configuration)
    {
        services.AddTransient<IServiceJwt, ServiceJwt>();
        services.AddTransient<IInternalCommunicationService, InternalCommunicationService>();
        services.AddTransient<IEventBusProvider, EventBusProvider>();

        services.AddSingleton<IBackgroundServicesRunner, BackgroundServicesRunner>();

        services.AddTransient<IEmailService, EmailService>();

        services.AddTransient<IOutputSerializer, JsonOutputSerializer>();
    }

    public List<string> GetRights()
    {
        return new List<string>();
    }
}
