using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;

namespace SolidOps.UM.Shared.Presentation;

public class SecurityRequirementsOperationFilter : IOperationFilter
{
    private readonly string _securitySchemeId;

    public SecurityRequirementsOperationFilter(string securitySchemeId)
    {
        if (string.IsNullOrEmpty(securitySchemeId))
        {
            throw new ArgumentException("securitySchemeId parameter is needed for SecurityRequirementsOperationFilter.");
        }

        _securitySchemeId = securitySchemeId;
    }

    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        if (!context.MethodInfo.GetCustomAttributes().Any(a => a is AllowAnonymousAttribute))
        {
            (operation.Security ??= []).Add(new OpenApiSecurityRequirement()
            {
                {
                    new OpenApiSecuritySchemeReference(_securitySchemeId),
                    new List<string>()
                }
            });
        }
    }
}