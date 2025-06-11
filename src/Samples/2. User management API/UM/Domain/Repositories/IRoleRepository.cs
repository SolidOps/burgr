using SolidOps.Burgr.Shared.Contracts.Results;
using SolidOps.UM.Domain.Entities;

namespace SolidOps.UM.Domain.Repositories;

public partial interface IRoleRepository
{
    Task<IOpsResult> SyncOrganizationRoles(Guid organizationId, List<Role> roles);

    Task<IOpsResult<Guid>> AddOrUpdateOrganizationRole(Guid organizationId, Role role, IEnumerable<Role> roles = null);
}
