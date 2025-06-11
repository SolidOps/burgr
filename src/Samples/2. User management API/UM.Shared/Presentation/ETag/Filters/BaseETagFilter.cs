using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Primitives;
using SolidOps.UM.Shared.Domain.Entities;
using SolidOps.UM.Shared.Domain.UnitOfWork;

namespace SolidOps.UM.Shared.Presentation.ETag.Filters;

public abstract class BaseETagFilter<TEntity, T> : IActionFilter
    where TEntity : BaseDomainEntity<T>
    where T : struct
{
    private readonly int[] _statusCodes;
    protected IETagRepository<TEntity, T> eTagRepository;
    protected readonly IExecutionContext executionContext;

    public BaseETagFilter(IETagRepository<TEntity, T> eTagRepository, IExecutionContext executionContext)
    {
        _statusCodes = new[] { 200 };
        this.eTagRepository = eTagRepository;
        this.executionContext = executionContext;
    }

    public void OnActionExecuting(ActionExecutingContext context)
    {
        SetRequestContext(context);
        StringValues ifNoneMatch;
        if (context.HttpContext.Request.Headers.TryGetValue("If-None-Match", out ifNoneMatch))
        {
            var eTag = GetETagForAction();
            if (eTag != null && eTag == ifNoneMatch.First())
            {
                context.Result = new StatusCodeResult(304);
            }
        }
    }

    public abstract void SetRequestContext(ActionExecutingContext context);
    public abstract string GetETagForAction();
    public abstract void SetETagForAction();

    public void OnActionExecuted(ActionExecutedContext context)
    {
        if (_statusCodes.Contains(context.HttpContext.Response.StatusCode) && !context.HttpContext.Response.Headers.TryGetValue("ETag", out _))
        {
            if (context.HttpContext.Request.Method == "GET")
            {
                string eTag = GetETagForAction();
                if(eTag == null)
                {
                    SetETagForAction();
                    eTag = GetETagForAction();
                }

                if (eTag != null)
                {
                    context.HttpContext.Response.Headers.Add("ETag", eTag);
                }
            }
        }
    }

}
