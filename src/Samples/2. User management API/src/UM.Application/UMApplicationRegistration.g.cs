#nullable enable
using Microsoft.Extensions.DependencyInjection;
namespace SolidOps.UM.Application;
public static partial class UMApplicationRegistration
{
    /// <summary>The application services of the UM module: one per aggregate root with an API, and those of the model.</summary>
    public static IServiceCollection AddUMApplication(this IServiceCollection services)
    {
        // Object [AG][R]
        services.AddScoped<UserService>();

        services.AddScoped<InviteService>();

        // Service 
        services.AddScoped<IAuthenticationService, AuthenticationService>();

        services.AddScoped<IInvitesService, InvitesService>();

        services.AddScoped<ISelfUserCreationService, SelfUserCreationService>();

        services.AddScoped<IServerStatusService, ServerStatusService>();

        services.AddScoped<ITokenValidationService, TokenValidationService>();

        services.AddScoped<IUserCreationService, UserCreationService>();

        return services;
    }
}