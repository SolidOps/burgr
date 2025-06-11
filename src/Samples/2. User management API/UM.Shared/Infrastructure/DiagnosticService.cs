namespace SolidOps.UM.Shared.Infrastructure;

public class DiagnosticService
{
    private object _lockObject = new object();
    private int _concurrentDBRequest = 0;
    private int _concurrentHttpRequest = 0;

    public void AddHttpRequest()
    {
        lock (_lockObject)
        {
            _concurrentDBRequest++;
        }
    }

    public void RemoveHttpRequest()
    {
        lock (_lockObject)
        {
            _concurrentDBRequest--;
        }
    }
    public void AddDBRequest()
    {
        lock (_lockObject)
        {
            _concurrentDBRequest++;
        }
    }

    public void RemoveDBRequest()
    {
        lock (_lockObject)
        {
            _concurrentDBRequest--;
        }
    }

    public (int concurrentHttpRequest, int concurrentDBRequest) GetConcurentRequests()
    {
        lock (_lockObject)
        {
            return (_concurrentHttpRequest, _concurrentDBRequest);
        }
    }
}
