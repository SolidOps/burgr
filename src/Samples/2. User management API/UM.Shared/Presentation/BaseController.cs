using Microsoft.AspNetCore.Mvc;
using SolidOps.UM.Shared.Contracts.Results;
using SolidOps.UM.Shared.Domain.Configuration;
using SolidOps.UM.Shared.Domain.UnitOfWork;
using SolidOps.SubZero;
using System.Security.Claims;

namespace SolidOps.UM.Shared.Presentation;

public abstract class BaseController : ControllerBase
{
    protected readonly IExecutionContext executionContext;
    private readonly IExtendedConfiguration configuration;
    protected readonly IServiceProvider serviceProvider;

    public BaseController(IExecutionContext executionContext, IExtendedConfiguration configuration, IServiceProvider serviceProvider)
    {
        executionContext.SetUserId(this.GetUserId);
        this.executionContext = executionContext;
        this.configuration = configuration;
        this.serviceProvider = serviceProvider;
    }

    private (string userId, List<string> rights, string authorization) GetUserId()
    {
        var userId = this.User?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        var serializedRights = this.User?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

        return (userId
            , serializedRights != null ? Serializer.Deserialize<List<string>>(serializedRights) : null
            , this.HttpContext?.Request?.Headers?.Authorization.ToString());
    }

    protected IActionResult Failure(OpsError error)
    {
        IActionResult result;
        switch (error.ErrorType)
        {
            case ErrorType.Forbidden:
                result = ErrorResultHelper.Generate(System.Net.HttpStatusCode.Forbidden, error.Message, null, false);
                break;
            default:
                result = ErrorResultHelper.Generate(System.Net.HttpStatusCode.BadRequest, error.Message, null, false);
                break;

        }

        return result;
    }
}
