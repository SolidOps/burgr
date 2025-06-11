using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SolidOps.UM.Shared.Contracts.Endpoints;

namespace SolidOps.UM.Shared.Tests;

public class TestSetup
{
    public Dictionary<string, List<AppService>> AppServices = new Dictionary<string, List<AppService>>();

    public int DBState = (int)DBStateEnum.Empty;

    public Dictionary<int, string> Suffixes;
    public readonly List<string> AlternateConnectionStringFiles = new List<string>();
    public bool EnableDiagnostic;
    public UserTestInstance AnonymousUserTestInstance;
    public AppServiceClient AnonymousClient;

    public List<IAppServiceInfo> AppServicesStartInfo = new List<IAppServiceInfo>();

    public TestSetup()
    {
        
    }

    public virtual async Task Start()
    {
        StartAppServices();

        if (AppServices.Count > 0)
        {
            var firstService = AppServices.First().Key;

            AnonymousUserTestInstance = await NewUserTestInstance(string.Empty, firstService);
            AnonymousClient = AnonymousUserTestInstance.PickClient(firstService);
        }
    }

    public virtual void BeforeStart()
    {

    }

    protected void StartAppServices()
    {
        foreach(var service in AppServicesStartInfo)
        {
            var mi = typeof(TestSetup).GetMethod("StartAppService");
            var fooRef = mi.MakeGenericMethod(service.StartupType);
            fooRef.Invoke(this, new object[] { service });
        }
    }

    public virtual void RegisterAppService(IAppServiceInfo startServiceInfo)
    {
        var found = AppServicesStartInfo.Where(info => info.ServiceName == startServiceInfo.ServiceName).SingleOrDefault();
        if (found != null)
            AppServicesStartInfo.Remove(found);
        AppServicesStartInfo.Add(startServiceInfo);
    }

    public virtual void StartAppService<TStartup>(AppServiceInfo<TStartup> startServiceInfo) where TStartup : class
    {
        string suffix = startServiceInfo.ForcedSuffix;
        if (suffix == null && Suffixes.ContainsKey(DBState))
        {
            suffix = Suffixes[DBState];
        }

        var service = new AppService<TStartup>(DBState, suffix, startServiceInfo);
        if (!AppServices.ContainsKey(startServiceInfo.ServiceName))
        {
            AppServices.Add(startServiceInfo.ServiceName, new List<AppService>());
        }
        AppServices[startServiceInfo.ServiceName].Add(service);
    }

    public void StopAppServices()
    {
        foreach (var kvp in AppServices)
        {
            foreach (var srv in kvp.Value)
            {
                srv.Dispose();
            }
            kvp.Value.Clear();
        }
        AppServices.Clear();
    }

    public virtual async Task<UserTestInstance> NewUserTestInstance(string userName, params string[] knownServices)
    {
        var userTestInstance = new UserTestInstance();
        foreach (var serviceName in knownServices)
        {
            userTestInstance.Clients.Add(serviceName, new List<AppServiceClient>());
            foreach (var service in AppServices[serviceName])
            {
                AppServiceClient client = new TestServiceClient(service.CreateHttpClient(), EnableDiagnostic, service.Remote, service.CreateHandler());
                userTestInstance.Clients[serviceName].Add(client);
            }
        }
        await Task.CompletedTask;

        return userTestInstance;
    }

    public virtual async Task Cleanup()
    {
        if (AnonymousClient != null)
            AnonymousClient.Dispose();
        if (AnonymousUserTestInstance != null)
            AnonymousUserTestInstance.Dispose();

        StopAppServices();
        await Task.CompletedTask;
    }

    public void ReplaceInternalCommunicationService(IServiceCollection services)
    {
        var sd = services.SingleOrDefault(sd => sd.ServiceType == typeof(IInternalCommunicationService));
        services.Remove(sd);
        services.AddTransient(typeof(IInternalCommunicationService), (services) =>
        {
            Dictionary<string, AppService> internalClients = new Dictionary<string, AppService>();
            foreach (var kvp in AppServices)
            {
                internalClients.Add(kvp.Key, kvp.Value.First());
            }
            return new TestInternalCommunicationService(internalClients, services.GetRequiredService<ILoggerFactory>());
        });
    }
}
