#nullable enable

using Microsoft.Extensions.DependencyInjection;

namespace _MODULE_NAMESPACE_.Application;

public static partial class _MODULE_ApplicationRegistration
{
    /// <summary>The application services of the _MODULE_ module, one per aggregate root with an API.</summary>
    public static IServiceCollection Add_MODULE_Application(this IServiceCollection services)
    {
        #region foreach MODEL[AG][R]
        services.AddScoped<CLASSNAMEService>();
        #endregion foreach MODEL
        return services;
    }
}
