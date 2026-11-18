#nullable enable
using Microsoft.Extensions.DependencyInjection;
namespace SolidOps.UM.Application;
public static partial class UMApplicationRegistration
{
    /// <summary>The application services of the UM module, one per aggregate root with an API.</summary>
    public static IServiceCollection AddUMApplication(this IServiceCollection services)
    {
        // Object [AG][R]
        services.AddScoped<UserService>();

        services.AddScoped<InviteService>();

        return services;
    }
}