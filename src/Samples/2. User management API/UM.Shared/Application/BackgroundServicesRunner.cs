using Microsoft.Extensions.Logging;

namespace SolidOps.UM.Shared.Application;

public class BackgroundServicesRunner : IBackgroundServicesRunner
{
    private List<IBackgroundService> ActiveBackgroundServices = new List<IBackgroundService>();
    ILogger<BackgroundServicesRunner> _logger;
    private IEnumerable<IBackgroundService> _backgroundServices;

    public BackgroundServicesRunner(ILoggerFactory loggerFactory, IEnumerable<IBackgroundService> backgroundServices)
    {
        _logger = loggerFactory.CreateLogger<BackgroundServicesRunner>();
        _backgroundServices = backgroundServices;
    }

    public async Task RunBackgroundServices(string[] wantedBackgroundServices = null)
    {
        await Task.CompletedTask;
        if (_backgroundServices == null) return;
        foreach (var backgroundService in _backgroundServices)
        {
            if (backgroundService == null)
                continue;

            var tplgin = backgroundService.GetType().ToString();
            if (wantedBackgroundServices != null && !wantedBackgroundServices.Any(p => tplgin.Contains(p)))
                continue;

            string s;
            s = $"Starting run of backgroundService {backgroundService.GetType()}";
            _logger.LogInformation(s);
            var t = new Thread(async () =>
            {
                try
                {
                    // TODO EF : Send a system user account to backgroundService
                    await backgroundService.Start(null);
                }
                catch (Exception e)
                {
                    s = $"Error running backgroundService {backgroundService.GetType()} : {e.GetBaseException().Message}";
                    var c = Console.ForegroundColor;
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(s);
                    Console.ForegroundColor = c;
                    _logger.LogError(e, s);
                }
            })
            {
                Name = backgroundService.GetType().ToString()
            };
            t.Start();
            ActiveBackgroundServices.Add(backgroundService);
        }

        while(ActiveBackgroundServices.Where(bs => bs.Status == BackgroundServiceStatus.Stopped).Any())
        {
            Thread.Sleep(100);
        }
        return;
    }

    public async Task SleepBackgroundServices()
    {
        var taskList = new List<Task>();
        var cts = new CancellationTokenSource();

        foreach (var backgroundService in ActiveBackgroundServices)
        {
            _logger.LogInformation($"Starting sleep of backgroundService {backgroundService.GetType()}");
            var task = backgroundService.Sleep(cts.Token);
            _logger.LogInformation($"Ended sleep of backgroundService {backgroundService.GetType()}");
            taskList.Add(task);
        }

        if (!Task.WaitAll(taskList.ToArray(), 10000))
        {
            // Cancel task
            cts.Cancel();
            await WakeUpBackgroundServices();
            throw new TimeoutException($"Some background services failed to sleep in time");
        }
    }

    public async Task WakeUpBackgroundServices()
    {
        var taskList = new List<Task>();
        var cts = new CancellationTokenSource();

        foreach (var backgroundService in ActiveBackgroundServices)
        {
            _logger.LogInformation($"Starting wake up of background service {backgroundService.GetType()}");
            var task = backgroundService.WakeUp(cts.Token);
            _logger.LogInformation($"Ended wake up of background service {backgroundService.GetType()}");
            taskList.Add(task);
        }

        if (!Task.WaitAll(taskList.ToArray(), 10000))
        {
            // Cancel task
            cts.Cancel();
            await SleepBackgroundServices();
            throw new TimeoutException($"Some background services failed to wake up in time");
        }
    }

    private void DisposeBackgroundServices()
    {
        foreach (var backgroundService in ActiveBackgroundServices.ToArray())
        {
            backgroundService.Dispose();
            ActiveBackgroundServices.Remove(backgroundService);
        }
    }

    #region Dispose
    private bool isDisposed;

    public void Dispose()
    {
        Dispose(true);
    }

    // Correct implementation to be called by aspdotncore injection disposing
    protected virtual void Dispose(bool disposing)
    {
        if (!isDisposed)
        {
            if (disposing)
            {
                DisposeBackgroundServices();
            }

            isDisposed = true;
        }
    }
    #endregion Dispose
}
