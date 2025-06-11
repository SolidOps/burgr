using SolidOps.UM.Shared.Contracts.Results;
using SolidOps.UM.Domain.AggregateRoots;

namespace SolidOps.UM.Application.UseCases;

public partial class ServerStatusUseCase
{
    public override async Task<IOpsResult<bool>> NeedTechUserPasswordUpdate(string techUser)
    {
        LocalUser localUser = await _dependencyLocalUserRepository.GetSingleByName(techUser);
        return IOpsResult.Ok(localUser.HashedPassword == string.Empty);
    }
}
