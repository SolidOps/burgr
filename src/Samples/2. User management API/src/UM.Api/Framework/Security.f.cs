using System.Security.Claims;
using SolidOps.UM.Application;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace SolidOps.UM.Api;

/// <summary>
/// Authorization of the UM endpoints. An endpoint requires an authenticated user, unless
/// the model says is_anonymous; a mandatory_right requires that right too (see
/// <see cref="ICurrentUser"/>). Authentication itself is configured in Program.cs.
/// </summary>
public static class Security
{
    /// <summary>Authorization by right, <see cref="ICurrentUser"/> and the validation of the request bodies.</summary>
    public static IServiceCollection AddUMApi(this IServiceCollection services)
    {
        services.AddAuthorization();
        services.AddSingleton<IAuthorizationPolicyProvider, RightPolicyProvider>();
        services.AddHttpContextAccessor();
        services.AddScoped<ICurrentUser, HttpCurrentUser>();
        return services;
    }

    /// <summary>An authenticated user, holding <paramref name="right"/> when it is not empty.</summary>
    public static TBuilder RequireRight<TBuilder>(this TBuilder builder, string right) where TBuilder : IEndpointConventionBuilder
    {
        return string.IsNullOrEmpty(right)
            ? builder.RequireAuthorization()
            : builder.RequireAuthorization(RightPolicyProvider.PolicyPrefix + right);
    }

    /// <summary>Answers 400 with the errors of an <see cref="IValidatable"/> argument, before the handler runs.</summary>
    public static TBuilder ValidateBody<TBuilder>(this TBuilder builder) where TBuilder : IEndpointConventionBuilder
    {
        return builder.AddEndpointFilter(async (context, next) =>
        {
            foreach (var argument in context.Arguments)
            {
                if (argument is IValidatable validatable && validatable.Validate() is { Count: > 0 } errors)
                {
                    return TypedResults.ValidationProblem(errors);
                }
            }
            return await next(context);
        });
    }
}

/// <summary>Builds the policy "right:X" on demand: an authenticated user holding X or every right.</summary>
public sealed class RightPolicyProvider(IOptions<AuthorizationOptions> options) : DefaultAuthorizationPolicyProvider(options)
{
    public const string PolicyPrefix = "right:";

    public override Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
    {
        if (!policyName.StartsWith(PolicyPrefix, StringComparison.Ordinal))
        {
            return base.GetPolicyAsync(policyName);
        }

        var right = policyName[PolicyPrefix.Length..];
        var policy = new AuthorizationPolicyBuilder()
            .RequireAuthenticatedUser()
            .RequireAssertion(context => HttpCurrentUser.HasRight(context.User, right))
            .Build();
        return Task.FromResult<AuthorizationPolicy?>(policy);
    }
}

/// <summary><see cref="ICurrentUser"/> read from the claims of the current request.</summary>
public sealed class HttpCurrentUser(IHttpContextAccessor httpContextAccessor) : ICurrentUser
{
    private ClaimsPrincipal? User => httpContextAccessor.HttpContext?.User;

    public bool IsAuthenticated => User?.Identity?.IsAuthenticated == true;

    public string? Name => IsAuthenticated ? User!.Identity!.Name : null;

    public bool HasRight(string right) => IsAuthenticated && HasRight(User!, right);

    public static bool HasRight(ClaimsPrincipal user, string right)
    {
        return user.HasClaim(ICurrentUser.RightClaimType, right) || user.HasClaim(ICurrentUser.RightClaimType, ICurrentUser.AllRights);
    }
}
