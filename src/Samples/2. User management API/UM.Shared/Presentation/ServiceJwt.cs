using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using SolidOps.UM.Shared.Application;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace SolidOps.UM.Shared.Presentation;

public class ServiceJwt : IServiceJwt
{

    private IConfiguration _configuration = null;
    private readonly ILogger<ServiceJwt> logger;

    public ServiceJwt(IConfiguration configuration, ILoggerFactory loggerFactory)
    {
        _configuration = configuration;
        this.logger = loggerFactory.CreateLogger<ServiceJwt>();
    }

    /// <summary>
    ///   Create a new JWT Token with claims data
    /// </summary>
    /// <param name="claims">List of Claim define by another controller</param>
    /// <param name="durationInMinutes">Duration of token validation</param>
    /// <returns></returns>
    public string CreateTokenWithClaims(Claim[] claims, int durationInMinutes)
    {
        this.logger.LogDebug($"CreateTokenWithClaims");
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(claims: claims, expires: DateTime.Now.AddMinutes(durationInMinutes),
                                         signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    /// <summary>
    ///   Get claims value from user authenticated
    /// </summary>
    /// <param name="user">ClaimsPrincipal User Identity</param>
    /// <param name="key">Claims Key to retrieve</param>
    /// <returns></returns>
    public string GetClaimsValue(ClaimsPrincipal user, string key)
    {
        this.logger.LogDebug($"GetClaimsValue");
        var value = "";
        // Is User authenticated ?
        if (user.Identity.IsAuthenticated)
        {
            var name = user.Claims.Where(c => c.Type == key).FirstOrDefault();
            if (name != null)
            {
                value = name.Value;
            }
        }

        return value;
    }
}
