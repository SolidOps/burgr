using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using SolidOps.UM.Application;

namespace SolidOps.UM.Api;

public sealed class JwtTokenIssuer(SymmetricSecurityKey key) : ITokenIssuer
{
    public const string Issuer = "um";
    private static readonly TimeSpan Lifetime = TimeSpan.FromHours(1);

    public static SymmetricSecurityKey KeyFrom(IConfiguration configuration)
    {
        var secret = configuration["Jwt:Key"] ?? "development key of the UM sample, replaced in production";
        return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret.PadRight(32)));
    }

    public string Issue(string name, IEnumerable<string> rights)
    {
        var claims = rights.Select(right => new Claim(ICurrentUser.RightClaimType, right)).Prepend(new Claim(JwtRegisteredClaimNames.Name, name));
        return new JsonWebTokenHandler().CreateToken(new SecurityTokenDescriptor
        {
            Issuer = Issuer,
            Audience = Issuer,
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.Add(Lifetime),
            SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256),
        });
    }
}
