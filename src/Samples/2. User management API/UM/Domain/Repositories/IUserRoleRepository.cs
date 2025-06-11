using SolidOps.UM.Domain.Entities;

namespace SolidOps.UM.Domain.Repositories;

public partial interface IUserRoleRepository
{
    Task ClearRolesForUser(Guid userId);
    Task<UserRole> GetSingleByUserAndRole(Guid userId, Guid roleId);
}
