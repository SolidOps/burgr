using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SolidOps.Burgr.Shared.Presentation;

namespace SolidOps.UM.API;

public abstract class BaseRemoteWoprStartup : CoreJwtStartup
{
    public BaseRemoteWoprStartup(IConfiguration configuration, string appName) : base(configuration, appName)
    {
        configuration["Jwt:Key"] = "remote";
    }

    public override void ConfigureServices(IServiceCollection services)
    {
        base.ConfigureServices(services);
    }
}
