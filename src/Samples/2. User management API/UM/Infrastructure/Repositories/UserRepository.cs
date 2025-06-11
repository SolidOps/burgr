using Microsoft.EntityFrameworkCore;
using SolidOps.UM.Shared.Application;
using SolidOps.UM.Shared.Domain.Configuration;
using SolidOps.SubZero;
using SolidOps.UM.Domain.AggregateRoots;
using System.Security.Claims;

namespace SolidOps.UM.Infrastructure.Repositories;

public partial class UserRepository
{
    public string CreateToken(User user)
    {
        var _jwtService = GetRequiredService<IServiceJwt>();

        var configuration = GetRequiredService<IExtendedConfiguration>().BurgrConfiguration;

        var tokenDuration = configuration.TokenDurationInMinutes;
        if (user.TechnicalUser)
            tokenDuration = configuration.TechnicalTokenDurationInMinutes;

        // Get Token
        var token = _jwtService.CreateTokenWithClaims(new[]
        {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Role, Serializer.Serialize(user.Rights))
            }, tokenDuration);

        return token;
    }
    
    protected override async Task<bool> HasOwnership(User entity, bool isDuringAdd)
    {
        await Task.CompletedTask;
        if (!isDuringAdd)
        {
            return entity.Id.ToString() == executionContext.UserId;
        }
        return true;
    }

    protected override async Task<IQueryable<User>> AddOwnershipCheck(IQueryable<User> query)
    {
        //if (executionContext != null && executionContext.UserId != null)
        //{
        //    var uos = await _dependencyUserOrganizationRepository.GetListByUserId(Guid.Parse(executionContext.UserId));
        //    if (uos != null)
        //    {
        //        var matchingUserOrganizationIds = uos.Select(uo => uo.OrganizationId).ToList();
        //        return query.Include(u => u.UserOrganizations).Where(u => u.UserOrganizations.Any(uo => matchingUserOrganizationIds.Contains(uo.OrganizationId)));
        //    }
        //}
        return query;
    }
}
