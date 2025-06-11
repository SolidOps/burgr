namespace SolidOps.UM.Shared.Application;

public interface IBackgroundServicesRunner : IDisposable
{
    Task RunBackgroundServices(string[] wantedBackgroundServices = null);

    Task SleepBackgroundServices();

    Task WakeUpBackgroundServices();
}
