#nullable enable
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
namespace SolidOps.UM.Infrastructure;
public static partial class UMInfrastructureRegistration
{
    /// <summary>The UM DbContext on the database chosen by <paramref name="configureDatabase"/>, its unit of work and repositories.</summary>
    public static IServiceCollection AddUMInfrastructure(this IServiceCollection services, Action<DbContextOptionsBuilder> configureDatabase)
    {
        services.AddDbContext<UMDbContext>(configureDatabase);
        services.AddScoped<Application.IUnitOfWork, UnitOfWork>();
        // Object [AG]
        services.AddScoped<Domain.ILocalUserRepository, LocalUserRepository>();

        services.AddScoped<Domain.IUserRepository, UserRepository>();

        services.AddScoped<Domain.IInviteRepository, InviteRepository>();

        return services;
    }
}