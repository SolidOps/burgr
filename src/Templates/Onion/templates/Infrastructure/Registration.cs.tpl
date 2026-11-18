#nullable enable

using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace _MODULE_NAMESPACE_.Infrastructure;

public static partial class _MODULE_InfrastructureRegistration
{
    /// <summary>The _MODULE_ DbContext on the database chosen by <paramref name="configureDatabase"/>, its unit of work and repositories.</summary>
    public static IServiceCollection Add_MODULE_Infrastructure(this IServiceCollection services, Action<DbContextOptionsBuilder> configureDatabase)
    {
        services.AddDbContext<_MODULE_DbContext>(configureDatabase);
        services.AddScoped<Application.IUnitOfWork, UnitOfWork>();
        #region foreach MODEL[AG]
        services.AddScoped<Domain.ICLASSNAMERepository, CLASSNAMERepository>();
        #endregion foreach MODEL
        return services;
    }
}
