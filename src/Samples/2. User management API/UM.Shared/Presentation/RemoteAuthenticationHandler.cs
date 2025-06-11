using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SolidOps.UM.Shared.Application;
using SolidOps.UM.Shared.Contracts.Endpoints;
using SolidOps.SubZero;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace SolidOps.UM.Shared.Presentation;

public class RemoteAuthenticationOptions : AuthenticationSchemeOptions
{
}

public class RemoteAuthenticationHandler : AuthenticationHandler<RemoteAuthenticationOptions>
{
    private readonly IInternalCommunicationService internalCommunicationService;

    public RemoteAuthenticationHandler(
        IOptionsMonitor<RemoteAuthenticationOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        ISystemClock clock,
        IInternalCommunicationService internalCommunicationService)
        : base(options, logger, encoder, clock)
    {
        this.internalCommunicationService = internalCommunicationService;
    }

    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (!Request.Headers.ContainsKey("Authorization"))
            return AuthenticateResult.Fail("Unauthorized");

        string authorizationHeader = Request.Headers["Authorization"];
        if (string.IsNullOrEmpty(authorizationHeader))
        {
            return AuthenticateResult.NoResult();
        }

        if (!authorizationHeader.StartsWith("bearer", StringComparison.OrdinalIgnoreCase))
        {
            return AuthenticateResult.Fail("Unauthorized");
        }

        string token = authorizationHeader.Substring("bearer".Length).Trim();

        if (string.IsNullOrEmpty(token))
        {
            return AuthenticateResult.Fail("Unauthorized");
        }

        try
        {
            return await ValidateToken(token);
        }
        catch (Exception ex)
        {
            return AuthenticateResult.Fail(ex.Message);
        }
    }

    private async Task<AuthenticateResult> ValidateToken(string token)
    {
        var client = this.internalCommunicationService.GetClient("UM", "Bearer " + token);

        var  response = await client.GetAsync("wopr/token-validation/validate");
        var stringContent = await response.Content.ReadAsStringAsync();
        var result = Serializer.Deserialize<ValidateTokenResultDTO>(stringContent);
        
        var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, result.UserId),
                new Claim(ClaimTypes.Role, Serializer.Serialize(result.Rights) ?? "")
        };

        var identity = new ClaimsIdentity(claims, Scheme.Name);
        var principal = new System.Security.Principal.GenericPrincipal(identity, null);
        var ticket = new AuthenticationTicket(principal, Scheme.Name);
        return AuthenticateResult.Success(ticket);
    }
}
