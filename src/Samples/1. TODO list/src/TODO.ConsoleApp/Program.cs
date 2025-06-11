using Extensions.Hosting.AsyncInitialization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SolidOps.TODO.Presentation;
using SolidOps.TODO.Shared;
using SolidOps.TODO.Shared.Infrastructure;
using System.Reflection;

namespace TODO.ConsoleApp;

internal class Program
{
    static async Task Main(string[] args)
    {
        IConfiguration configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .AddCommandLine(args)
                .Build();

        var provider = RegisterServices(configuration);

        var asyncInits = provider.GetServices<IAsyncInitializer>();
        foreach (var asyncInit in asyncInits)
        {
            await asyncInit.InitializeAsync(new CancellationToken());
        }

        IServiceScope scope = provider.CreateScope();
        await scope.ServiceProvider.GetRequiredService<TODOConsoleSession>().Run();

        DisposeServices(provider);
    }

    private static ServiceProvider RegisterServices(IConfiguration configuration)
    {
        var services = new ServiceCollection();
        services.AddSingleton<IConfiguration>(configuration);
        services.AddTransient<TODOConsoleSession>();
        services.AddScoped<IUserContext, UserContext>();
        services.AddAsyncInitializer<DBInitializer>();

        RegisterModules(services, configuration, new List<Assembly>()
        {
            typeof(SolidOps.TODO.Application.AssemblyReference).Assembly
        });

        return services.BuildServiceProvider(true);
    }

    private static void RegisterModules(IServiceCollection services, IConfiguration configuration, List<Assembly> dependencies)
    {
        var registratorType = typeof(IServiceRegistrar);
        var registrationTypes = new List<Type>();

        foreach (var assembly in dependencies)
        {
            registrationTypes.AddRange(assembly.GetTypes());
        }

        var serviceRegistrarTypes = registrationTypes
                    .Where(t => registratorType.IsAssignableFrom(t) && !t.IsInterface)
                    .ToList();

        List<IServiceRegistrar> registrars = new List<IServiceRegistrar>();
        serviceRegistrarTypes.ForEach(p =>
        {
            registrars.Add((IServiceRegistrar)Activator.CreateInstance(p));
        });

        foreach (var registrator in registrars)
        {
            registrator.ConfigureServices(services, configuration);
        }
    }

    private static void DisposeServices(ServiceProvider serviceProvider)
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
