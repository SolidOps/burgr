using SolidOps.UM.Shared.Contracts.Results;

namespace SolidOps.UM.Shared.Domain.Configuration;

public interface IRemoteConfigurationService
{
    Task<IOpsResult<Dictionary<string, string>>> GetRemoteConfiguration(string application, string environment, string password);
}
