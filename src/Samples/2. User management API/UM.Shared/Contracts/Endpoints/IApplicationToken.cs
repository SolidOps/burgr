using SolidOps.UM.Shared.Contracts.Results;

namespace SolidOps.UM.Shared.Contracts.Endpoints;

public interface IApplicationToken
{
    Task<IOpsResult> Login(string appName, string password);
    string Token { get; }
}
