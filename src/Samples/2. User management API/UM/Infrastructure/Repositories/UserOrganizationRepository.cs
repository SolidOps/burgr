//using SolidOps.UM.Domain.Entities;
//using SolidOps.UM.Domain.Repositories;

//namespace SolidOps.UM.Infrastructure.Repositories;

//public partial class UserOrganizationRepository : IUserOrganizationRepository
//{
//    public async Task ClearOrganizationsForUser(Guid userId)
//    {
//        var userOrganizations = await GetListOfQueryable(this.GetQueryable().Where(uo => uo.UserId == userId));

//        foreach (var uo in userOrganizations)
//        {
//            await this.Delete(uo);
//        }
//    }

//    protected override async Task<bool> HasOwnership(UserOrganization entity, bool isDuringAdd)
//    {
//        var matchingUserOrganization = await GetSingleByOrganizationAndUser(entity.OrganizationId, Guid.Parse(executionContext.UserId));
//        if (matchingUserOrganization != null)
//            return true;
//        return false;
//    }

//}
