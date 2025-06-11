using Microsoft.Extensions.DependencyInjection;
using SolidOps.Burgr.Shared.Domain.Configuration;
using SolidOps.Burgr.Shared.Domain.CrossCutting;
using SolidOps.Burgr.Shared.Application.Events;
namespace SolidOps.UM.Application;
public static class AssemblyReference
{
}
public partial class UMApplicationServiceRegistrator : IServiceRegistrator
{
    public int Priority => 1;
    public void ConfigureServices(IServiceCollection services, IExtendedConfiguration configuration)
    {
        services.AddAsyncInitializer<UMApplicationInitializer>();
        // Object 

        if (configuration["Legacy:UM"] != "True")
        {
            // Service [I]
            services.AddTransient<Application.Services.IAuthenticationService, Application.Services.AuthenticationService>();

            services.AddTransient<Application.Services.IConfigurationsService, Application.Services.ConfigurationsService>();

            services.AddTransient<Application.Services.IInvitesService, Application.Services.InvitesService>();

            services.AddTransient<Application.Services.ISelfUserCreationService, Application.Services.SelfUserCreationService>();

            services.AddTransient<Application.Services.IServerStatusService, Application.Services.ServerStatusService>();

            services.AddTransient<Application.Services.ITokenValidationService, Application.Services.TokenValidationService>();

            services.AddTransient<Application.Services.IUserCreationService, Application.Services.UserCreationService>();

        }
        ConfigureAdditionalServices(services, configuration);
    }
    partial void ConfigureAdditionalServices(IServiceCollection services, IExtendedConfiguration configuration);
}