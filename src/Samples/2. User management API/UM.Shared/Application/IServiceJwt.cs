using System.Security.Claims;

namespace SolidOps.UM.Shared.Application;

public interface IServiceJwt
{
    string CreateTokenWithClaims(Claim[] claims, int durationInMinutes);
    string GetClaimsValue(ClaimsPrincipal user, string key);
}
