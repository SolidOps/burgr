//using SolidOps.UM.Domain.Entities;

//namespace SolidOps.UM.Infrastructure.Repositories;

//public partial class UserRoleRepository
//{
//    public async Task ClearRolesForUser(Guid userId)
//    {
//        var userRoles = await GetListOfQueryable(this.GetQueryable().Where(ur => ur.UserId == userId));

//        foreach (var ur in userRoles)
//        {
//            await this.Delete(ur);
//        }
//    }

//    public async Task<UserRole> GetSingleByUserAndRole(Guid userId, Guid roleId)
//    {
//        return (await this.GetListWhere(u => u.UserId == userId && u.RoleId == roleId)).SingleOrDefault();
//    }
//}
