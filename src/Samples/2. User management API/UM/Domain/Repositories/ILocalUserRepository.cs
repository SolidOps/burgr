using SolidOps.UM.Shared.Contracts.Results;
using SolidOps.UM.Domain.AggregateRoots;

namespace SolidOps.UM.Domain.Repositories;

public partial interface ILocalUserRepository
{
    Task<IOpsResult> UpdatePassword(LocalUser entity, string password);
}
