using Microsoft.AspNetCore.Http;
using SolidOps.UM.Shared.Domain.UnitOfWork;

namespace SolidOps.UM.Presentation.Controllers;

public partial class SelfUserCreationController
{
    partial void AddAdditionalHeadersForCreateUser(HttpContext httpContext)
    {
        httpContext.Response.Headers.Add("Authorization", new[] { $"Bearer {executionContext.TemporaryData[IExecutionContext.TEMPORARY_DATA_TOKEN]}" });
    }
}
