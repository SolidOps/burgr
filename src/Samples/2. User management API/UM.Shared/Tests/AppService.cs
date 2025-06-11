using Microsoft.Extensions.DependencyInjection;
using SolidOps.UM.Shared.Contracts.Endpoints;
using SolidOps.UM.Shared.Domain.UnitOfWork;
using SolidOps.UM.Shared.Application.Events;

namespace SolidOps.UM.Shared.Tests;

public class AppService : IDisposable
{
    public bool Remote { get; set; }
    public TestSession CurrentSession { get; set; }

    public Dictionary<string, string> PasswordPerUser { get; private set; }

    public List<ResponseDiagnostic> Diagnostics { get; set; }

    public AppService(IAppServiceInfo startAppServiceInfo)
    {
        Remote = startAppServiceInfo.Remote;
        PasswordPerUser = new Dictionary<string, string>();
        PasswordPerUser.Add(IExecutionContext.ROOTUSER, Guid.NewGuid().ToString());

        Diagnostics = new List<ResponseDiagnostic>();
    }

    public virtual void Dispose()
    {
        CurrentSession.Dispose();
        CurrentSession = null;
    }

    public virtual HttpClient CreateHttpClient()
    {
        var httpClient = new HttpClient();
        httpClient.Timeout = new TimeSpan(1, 0, 0);
        return httpClient;
    }

    public virtual HttpMessageHandler CreateHandler()
    {
        return null;
    }

    public virtual async Task DelayInit<T>()
        where T : IDelayStart
    {
        await Task.CompletedTask;
    }

    public virtual IServiceScope GetServiceScope()
    {
        return null;
    }
}

public class AppService<TStartup> : AppService where TStartup : class
{
    public AppServiceWebApplicationFactory<TStartup> Factory { get; private set; }

    public AppService(int dbState, string suffix, AppServiceInfo<TStartup> startAppServiceInfo) : base(startAppServiceInfo)
    {
        if (!startAppServiceInfo.Remote)
        {
            Factory = new AppServiceWebApplicationFactory<TStartup>(dbState, suffix, startAppServiceInfo);
            CurrentSession = new TestSession(CreateHttpClient());
        }
        else
        {
            CurrentSession = new TestSession(new HttpClient());
        }
    }

    public override HttpClient CreateHttpClient()
    {
        var httpClient = Factory.CreateClient();
        httpClient.Timeout = new TimeSpan(1, 0, 0);
        return httpClient;
    }

    public override HttpMessageHandler CreateHandler()
    {
        return Factory.Server.CreateHandler();
    }

    public override async Task DelayInit<T>()
    {
        var delayStartService = Factory.Services.GetService<T>();
        if (delayStartService != null)
        {
            await delayStartService.Start();
        }
    }

    public override void Dispose()
    {
        base.Dispose();

        Factory?.Dispose();
        CurrentSession?.Dispose();
    }

    public override IServiceScope GetServiceScope()
    {
        return Factory.Services.CreateScope();
    }
}

public class TestSession : IDisposable
{
    public TestSession(HttpClient client)
    {
        Client = client;
    }
    public HttpClient Client { get; }

    public void Dispose()
    {
        Client?.Dispose();
    }
}
