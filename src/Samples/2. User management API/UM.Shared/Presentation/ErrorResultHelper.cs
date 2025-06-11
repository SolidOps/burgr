using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SolidOps.UM.Shared.Application;
using System.Net;

namespace SolidOps.UM.Shared.Presentation;

public static class ErrorResultHelper
{
    public static IActionResult Generate(HttpContext context, Exception exception, bool sendErrorToClient)
    {
        return ErrorResultHelper.Generate(System.Net.HttpStatusCode.InternalServerError, "unknown error", exception, sendErrorToClient);
    }

    public static IActionResult Generate(HttpStatusCode code, string message, Exception exception, bool sendErrorToClient)
    {
        if (sendErrorToClient)
        {
            return new JsonResult(exception.GetExceptionMessage())
            {
                StatusCode = (int)code,
            };
        }
        else
        {
            return new JsonResult(message)
            {
                StatusCode = (int)code,
            };
        }
    }
}
