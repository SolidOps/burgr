using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SolidOps.UM.Shared.Domain.Configuration;
using SolidOps.UM.Shared.Infrastructure;

namespace SolidOps.UM.Shared.Tests;

public class AppServiceWebApplicationFactory<TStartup>
: WebApplicationFactory<TStartup> where TStartup : class
{
    private readonly int DBState;
    private readonly Action<IServiceCollection> onConfiguration;
    private readonly Action<int, IServiceProvider> onAppServiceInitialize;
    private readonly Action<int, IServiceProvider> OnAppServiceCleanup;
    private readonly EventOption eventOption;
    private readonly string suffix;
    private readonly bool delayInit;
    private bool isDisposed = false;

    public AppServiceWebApplicationFactory(int dbState, string suffix, AppServiceInfo<TStartup> startAppServiceInfo)
    {
        DBState = dbState;
        this.onConfiguration = startAppServiceInfo.OnConfiguration;
        this.onAppServiceInitialize = startAppServiceInfo.OnAppServiceInitialize;
        this.OnAppServiceCleanup = startAppServiceInfo.OnAppServiceCleanup;
        this.eventOption = startAppServiceInfo.EventOption;
        this.delayInit = startAppServiceInfo.DelayInit;
        this.suffix = suffix;
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            // Make TestInitializer first initializer
            services.AddAsyncInitializerOnTop<TestInitializer>();
            
            if (onConfiguration != null)
            {
                onConfiguration(services);
            }
        });
    }

    protected override IHost CreateHost(IHostBuilder builder)
    {
        var host = base.CreateHost(builder);

        var serviceProvider = host.Services;

        var config = serviceProvider.GetRequiredService<IExtendedConfiguration>();
        config["DBState"] = DBState.ToString();
        config["Suffix"] = suffix;
        config["EventOption"] = eventOption.ToString();

        if (!delayInit)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                host.InitAsync().Wait();

                if (onAppServiceInitialize != null)
                {
                    onAppServiceInitialize(DBState, scope.ServiceProvider);
                }
            }
        }
        return host;
    }

    protected override void Dispose(bool disposing)
    {
        if (!isDisposed)
        {
            using (var scope = this.Services.CreateScope())
            {
                if (OnAppServiceCleanup != null)
                {
                    this.OnAppServiceCleanup(DBState, scope.ServiceProvider);
                }
            }
        }
        isDisposed = true;

        base.Dispose(disposing);
    }
}
