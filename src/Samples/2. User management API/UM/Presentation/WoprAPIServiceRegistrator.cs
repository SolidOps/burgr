using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using SolidOps.Burgr.Shared.Domain.Configuration;
using SolidOps.UM.Domain.Services;

namespace SolidOps.UM.Presentation;

public partial class UMPresentationServiceRegistrator
{
    partial void ConfigureAdditionalServices(IServiceCollection services, IExtendedConfiguration configuration)
    {
        services.AddScoped(typeof(IPasswordHasher<>), typeof(PasswordHasher<>));
        services.AddScoped<IIdentityProviderService, LocalIdentityProviderService>();
    }
}
