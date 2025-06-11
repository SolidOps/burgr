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
            // UseCase [I]
            services.AddTransient<Application.UseCases.IAuthenticationUseCase, Application.UseCases.AuthenticationUseCase>();

            services.AddTransient<Application.UseCases.IInvitesUseCase, Application.UseCases.InvitesUseCase>();

            services.AddTransient<Application.UseCases.ISelfUserCreationUseCase, Application.UseCases.SelfUserCreationUseCase>();

            services.AddTransient<Application.UseCases.IServerStatusUseCase, Application.UseCases.ServerStatusUseCase>();

            services.AddTransient<Application.UseCases.ITokenValidationUseCase, Application.UseCases.TokenValidationUseCase>();

            services.AddTransient<Application.UseCases.IUserCreationUseCase, Application.UseCases.UserCreationUseCase>();

        }
        ConfigureAdditionalServices(services, configuration);
    }
    partial void ConfigureAdditionalServices(IServiceCollection services, IExtendedConfiguration configuration);
}