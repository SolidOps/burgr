using SolidOps.UM.Shared.Contracts.Results;

namespace SolidOps.UM.Domain.Services;

public interface IIdentityProviderService
{
    Task<IOpsResult<bool>> Validate(string login, string password);
}
