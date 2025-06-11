using Microsoft.AspNetCore.Http;
using SolidOps.UM.Shared.Contracts.Endpoints;
using SolidOps.SubZero;
using System.Diagnostics;

namespace SolidOps.UM.Shared.Presentation;

public class DiagnosticMiddleware
{
    private readonly RequestDelegate _nextMiddleware;

    public DiagnosticMiddleware(RequestDelegate nextMiddleware)
    {
        _nextMiddleware = nextMiddleware;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        if (httpContext.Request.Headers.TryGetValue("X-Diag", out _))
        {
            var watch = Stopwatch.StartNew();
            httpContext.Items["Requests"] = new List<RequestStat>();
            httpContext.Response.OnStarting(state =>
            {
                var httpContext = (HttpContext)state;
                var requests = (List<RequestStat>)httpContext.Items["Requests"];
                var requestNumber = requests.Count;
                var requestsDuration = requests.Sum(r => r.Duration);
                httpContext.Response.Headers.Add("X-Diag", new[] { $"{watch.ElapsedMilliseconds}|{Convert.ToInt32(requestsDuration)}|{requestNumber}" });
                if (httpContext.Request.Headers.TryGetValue("X-Requests", out _))
                {
                    httpContext.Response.Headers.Add("X-Requests", new[] { Serializer.Serialize(requests) });
                }
                return Task.CompletedTask;
            }, httpContext);
        }
        await _nextMiddleware(httpContext);
    }
}
