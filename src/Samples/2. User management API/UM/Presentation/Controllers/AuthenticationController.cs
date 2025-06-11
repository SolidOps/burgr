using Microsoft.AspNetCore.Http;
using SolidOps.UM.Shared.Domain.UnitOfWork;

namespace SolidOps.UM.Presentation.Controllers;

public partial class AuthenticationController
{
    partial void AddAdditionalHeadersForLogin(HttpContext httpContext)
    {
        if (executionContext.TemporaryData.ContainsKey(IExecutionContext.TEMPORARY_DATA_TOKEN))
        {
            httpContext.Response.Headers.Add("Authorization", new[] { $"Bearer {executionContext.TemporaryData[IExecutionContext.TEMPORARY_DATA_TOKEN]}" });
        }
    }
}
