using SolidOps.UM.Shared.Contracts.Endpoints;
using SolidOps.UM.Shared.Domain.UnitOfWork;

namespace SolidOps.UM.Shared.Tests;

public class UserTestInstance : IDisposable
{
    public Dictionary<string, List<AppServiceClient>> Clients = new Dictionary<string, List<AppServiceClient>>();

    public Dictionary<string, string> PasswordPerUser { get; private set; }

    public List<ResponseDiagnostic> Diagnostics { get; set; }

    public bool EnableDiagnostic { get; set; }

    public ResponseDiagnostic LastDiagnostic
    {
        get
        {
            if (Diagnostics != null && Diagnostics.Count > 0)
            {
                return Diagnostics[Diagnostics.Count - 1];
            }
            return null;
        }
    }

    public ResponseDiagnostic OverallDiagnostic
    {
        get
        {
            if (Diagnostics != null && Diagnostics.Count > 0)
            {
                ResponseDiagnostic overallDiagnostic = new ResponseDiagnostic("overall");
                overallDiagnostic.DurationMs = Diagnostics.Sum(d => d.DurationMs);
                overallDiagnostic.NumberOfRequests = Diagnostics.Sum(d => d.NumberOfRequests);
                overallDiagnostic.NetworkShare = Diagnostics.Sum(d => d.NetworkShare) / Diagnostics.Count;
                overallDiagnostic.ApplicationLayerShare = Diagnostics.Sum(d => d.ApplicationLayerShare) / Diagnostics.Count;
                overallDiagnostic.DataAccessLayerShare = Diagnostics.Sum(d => d.DataAccessLayerShare) / Diagnostics.Count;
                return overallDiagnostic;
            }
            return null;

        }
    }

    public UserTestInstance()
    {
        PasswordPerUser = new Dictionary<string, string>();
        PasswordPerUser.Add(IExecutionContext.ROOTUSER, Guid.NewGuid().ToString());

        Diagnostics = new List<ResponseDiagnostic>();
    }

    public void Dispose()
    {
        foreach (var kvp in Clients)
        {
            foreach (var client in kvp.Value)
            {
                client.Dispose();
            }
        }
    }

    public AppServiceClient PickClient(string serviceName)
    {
        return Clients[serviceName].First();
    }
}
