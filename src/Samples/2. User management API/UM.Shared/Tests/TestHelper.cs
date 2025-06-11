using Extensions.Hosting.AsyncInitialization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using SolidOps.UM.Shared.Application;
using SolidOps.UM.Shared.Domain.Configuration;
using SolidOps.UM.Shared.Domain.CrossCutting;
using SolidOps.UM.Shared.Domain.UnitOfWork;
using SolidOps.UM.Shared.Infrastructure;
using SolidOps.UM.Shared.Presentation;

namespace SolidOps.UM.Shared.Tests;

public class TestHelper
{
    public static ServiceProvider ConfigureInjection(string configFileName
        , List<IServiceRegistrar> registrators
        , int DBState = (int)DBStateEnum.Empty
        , string suffix = null
        , EventOption eventOption = EventOption.Disabled)
    {
        IConfiguration configuration = new ConfigurationBuilder()
            .AddJsonFile(configFileName, optional: true, reloadOnChange: true)
            .AddEnvironmentVariables()
            .Build();

        Serilog.Log.Logger = new LoggerConfiguration()
           .ReadFrom.Configuration(configuration)
           .CreateLogger();

        var provider = RegisterServices(configuration, registrators, DBState, suffix, eventOption);
        return provider;
    }

    public static ServiceProvider RegisterServices(IConfiguration configuration
        , List<IServiceRegistrar> registrators
        , int DBState
        , string suffix
        , EventOption eventOption)
    {
        var services = new ServiceCollection();
        services.AddSingleton<IConfiguration>(configuration);
        services.AddLogging(configure => configure.AddSerilog());
        services.AddScoped<IExecutionContext, BurgrExecutionContext>();

        var burgrConfiguration = new ExtendedConfiguration(configuration);

        burgrConfiguration["DBState"] = DBState.ToString();
        if (suffix == null)
        {
            if (Enum.IsDefined(typeof(DBStateEnum), DBState))
            {
                var name = Enum.GetName(typeof(DBStateEnum), DBState).ToLower();
                burgrConfiguration["Suffix"] = "_" + name;
            }
            else
            {
                burgrConfiguration["Suffix"] = "_undefined";
            }
        }
        else
        {
            burgrConfiguration["Suffix"] = suffix;
        }

        burgrConfiguration["EventOption"] = eventOption.ToString();

        services.AddSingleton<IExtendedConfiguration>(burgrConfiguration);

        RegisterModule(new CoreServiceRegistrator(), services, burgrConfiguration);
        foreach (var registrator in registrators.OrderBy(r => r.Priority))
        {
            RegisterModule(registrator, services, burgrConfiguration);
        }

        services.AddAsyncInitializer<TestInitializer>();
        services.AddAsyncInitializer<DBInitializer>();
        services.AddAsyncInitializer<BurgrInitializer>();

        return services.BuildServiceProvider(true);
    }

    public static async Task Init(ServiceProvider provider)
    {
        var asyncInits = provider.GetServices<IAsyncInitializer>();
        foreach (var asyncInit in asyncInits)
        {
            await asyncInit.InitializeAsync(new CancellationToken());
        }
    }

    public static void RegisterModule(IServiceRegistrar registrator, ServiceCollection services, IExtendedConfiguration configuration)
    {
        registrator.ConfigureServices(services, configuration);
    }

    public static void DisposeServices(ServiceProvider serviceProvider)
    {
        if (serviceProvider == null)
        {
            return;
        }
        if (serviceProvider is IDisposable)
        {
            ((IDisposable)serviceProvider).Dispose();
        }
    }
}
