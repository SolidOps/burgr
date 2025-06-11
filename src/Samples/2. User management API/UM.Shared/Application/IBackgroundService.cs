namespace SolidOps.UM.Shared.Application;

public interface IBackgroundService : IDisposable
{
    BackgroundServiceStatus Status { get; }

    Task Start(string userId);

    Task Sleep(CancellationToken token);

    Task WakeUp(CancellationToken token);
}
