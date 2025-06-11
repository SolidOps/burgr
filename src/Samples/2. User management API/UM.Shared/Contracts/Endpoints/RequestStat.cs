namespace SolidOps.UM.Shared.Contracts.Endpoints;

public class RequestStat
{
    public RequestStat(string request, double duration)
    {
        Request = request;
        Duration = duration;
    }

    public string Request { get; private set; }
    public double Duration { get; private set; }
}
