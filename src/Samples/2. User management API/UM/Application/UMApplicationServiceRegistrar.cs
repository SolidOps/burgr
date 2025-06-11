using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using SolidOps.UM.Shared.Domain.Configuration;
using SolidOps.UM.Domain.Services;

namespace SolidOps.UM.Application;

public partial class UMApplicationServiceRegistrar
{
    partial void ConfigureAdditionalServices(IServiceCollection services, IExtendedConfiguration configuration)
    {
        services.AddScoped(typeof(IPasswordHasher<>), typeof(PasswordHasher<>));
        services.AddScoped<IIdentityProviderService, LocalIdentityProviderService>();
    }
}
