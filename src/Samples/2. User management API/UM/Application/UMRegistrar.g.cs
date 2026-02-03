using Microsoft.Extensions.DependencyInjection;
using SolidOps.UM.Shared.Domain.Configuration;
using SolidOps.UM.Shared.Domain.CrossCutting;
using SolidOps.UM.Shared.Application.Events;
namespace SolidOps.UM.Application;
public static class AssemblyReference
{
}
public partial class UMApplicationServiceRegistrar : IServiceRegistrar
{
    public int Priority => 1;
    public void ConfigureServices(IServiceCollection services, IExtendedConfiguration configuration)
    {
        services.AddAsyncInitializer<UMApplicationInitializer>();
        // Object 

        if (configuration["Legacy:UM"] != "True")
        {
            // Service 
            services.AddTransient<Application.Services.IAuthenticationService, Application.Services.AuthenticationService>();

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