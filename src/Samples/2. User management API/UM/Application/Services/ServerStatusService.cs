using SolidOps.Burgr.Shared.Contracts.Results;
using SolidOps.UM.Domain.AggregateRoots;

namespace SolidOps.UM.Application.Services;

public partial class ServerStatusService
{
    public override async Task<IOpsResult<bool>> NeedTechUserPasswordUpdate(string techUser)
    {
        LocalUser localUser = await _dependencyLocalUserRepository.GetSingleByName(techUser);
        return IOpsResult.Ok(localUser.HashedPassword == string.Empty);
    }
}
