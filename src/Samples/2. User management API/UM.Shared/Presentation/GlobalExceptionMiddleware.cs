using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SolidOps.UM.Shared.Domain.Configuration;

namespace SolidOps.UM.Shared.Presentation;

public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _nextMiddleware;
    private readonly ILogger<GlobalExceptionMiddleware> _logger;
    private readonly IExtendedConfiguration configuration;

    public GlobalExceptionMiddleware(RequestDelegate nextMiddleware, ILoggerFactory loggerFactory, IExtendedConfiguration configuration)
    {
        _logger = loggerFactory.CreateLogger<GlobalExceptionMiddleware>();
        this.configuration = configuration;
        _nextMiddleware = nextMiddleware;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _nextMiddleware(httpContext);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, httpContext.TraceIdentifier + ": An unhandled exception occured");
            await HandleExceptionAsync(httpContext, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        IActionResult result = ErrorResultHelper.Generate(context, exception, configuration.BurgrConfiguration.SendErrorToClient);

        await result.ExecuteResultAsync(new ActionContext
        {
            HttpContext = context,
        });
    }
}