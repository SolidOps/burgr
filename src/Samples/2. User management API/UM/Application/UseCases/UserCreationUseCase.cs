using SolidOps.UM.Shared.Contracts.Results;
using SolidOps.UM.Domain.AggregateRoots;
using SolidOps.UM.Domain.Entities;
using SolidOps.UM.Domain.Transients;

namespace SolidOps.UM.Application.UseCases;

public partial class UserCreationUseCase
{
    public override async Task<IOpsResult<Guid>> CreateUser(UserCreationInfo user_creation_info)
    {
        var user = User.Create(user_creation_info.UserEmail, null, false);
        if (user_creation_info.Rights != null && user_creation_info.Rights.Any())
        {
            user.UserRights = user_creation_info.Rights;
        }
        var result = await _dependencyUserRepository.Add(user);
        if(result.HasError) return result;

        //var userOrganization = UserOrganization.Create(user.Id, organizationId);
        //result = await _dependencyUserOrganizationRepository.Add(userOrganization);
        //if (result.HasError) return result;

        var local = await _dependencyLocalUserRepository.GetSingleByName(user.Email);
        if (user_creation_info.Password != null)
        {
            var updateResult = await _dependencyLocalUserRepository.UpdatePassword(local, user_creation_info.Password);
            if (updateResult.HasError) return updateResult.ToResult<Guid>();
        }

        //if (user_creation_info.RoleName != null)
        //{
        //    var role = Role.Create(user_creation_info.RoleName, organizationId);
        //    if (user_creation_info.Rights != null)
        //        role.ApplicationRights = user_creation_info.Rights;
        //    result = await _dependencyRoleRepository.AddOrUpdateOrganizationRole(organizationId, role);
        //    if (result.HasError) return result;

        //    var userRole = await _dependencyUserRoleRepository.GetSingleByUserAndRole(user.Id, result.Data);
        //    if (userRole == null)
        //    {
        //        userRole = UserRole.Create(user.Id, result.Data);
        //        result = await _dependencyUserRoleRepository.Add(userRole);
        //        if (result.HasError) return result;
        //    }
        //}

        return IOpsResult.Ok(user.Id);
    }
}
