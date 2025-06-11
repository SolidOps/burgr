//using SolidOps.UM.Shared.Contracts.Results;
//using SolidOps.UM.Domain.Entities;

//namespace SolidOps.UM.Infrastructure.Repositories;

//public partial class RoleRepository
//{
//    public async Task<IOpsResult> SyncOrganizationRoles(Guid organizationId, List<Role> rolesInfo)
//    {
//        var roles = await GetListByOrganizationId(organizationId);
//        List<Guid> deletableRoles = new List<Guid>(roles.Select(r => r.Id));
//        foreach (var roleInfo in rolesInfo)
//        {
//            var result = await AddOrUpdateOrganizationRole(organizationId, roleInfo, roles);
//            if (result.HasError) return result;
//            if (deletableRoles.Contains(result.Data))
//                deletableRoles.Remove(result.Data);
//        }

//        foreach (var role in deletableRoles)
//        {
//            var result = await RemoveWithId(role.ToString());
//            if (result.HasError) return result;
//        }
//        return IOpsResult.Ok();
//    }

//    public async Task<IOpsResult<Guid>> AddOrUpdateOrganizationRole(Guid organizationId, Role roleInfo, IEnumerable<Role> roles = null)
//    {
//        if (roles == null)
//        {
//            roles = await GetListByOrganizationId(organizationId);
//        }
//        var role = roles.SingleOrDefault(r => r.Name == roleInfo.Name);
//        if (role == null)
//        {
//            role = Role.Create(roleInfo.Name, organizationId);
//            if (roleInfo.ApplicationRights != null)
//            {
//                role.ApplicationRights = roleInfo.ApplicationRights.ToList();
//            }
//            var result = await Add(role);
//            if(result.HasError) return result;
//        }
//        else
//        {
//            if (roleInfo.ApplicationRights != null)
//            {
//                role.ApplicationRights = roleInfo.ApplicationRights.ToList();
//            }
//            var result = await Update(role);
//            if (result.HasError) return result.ToResult<Guid>();
//        }
//        return IOpsResult.Ok(role.Id);
//    }
//}
