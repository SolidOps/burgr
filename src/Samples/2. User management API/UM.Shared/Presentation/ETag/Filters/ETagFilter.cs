using Microsoft.AspNetCore.Mvc.Filters;
using SolidOps.UM.Shared.Domain.Entities;
using SolidOps.UM.Shared.Domain.UnitOfWork;

namespace SolidOps.UM.Shared.Presentation.ETag.Filters;

public class ETagFilter<TEntity, T> : BaseETagFilter<TEntity, T>, IActionFilter
    where TEntity : BaseDomainEntity<T>
    where T : struct
{
    private string currentId;
    private bool isGetWithNoParametersRequest;

    public ETagFilter(IETagRepository<TEntity, T> eTagRepository, IExecutionContext context) : base(eTagRepository, context)
    {

    }

    public override void SetRequestContext(ActionExecutingContext context)
    {
        if (context.HttpContext.Request.Method == "GET")
        {
            if (context.ActionArguments.ContainsKey("id"))
            {
                currentId = context.ActionArguments["id"].ToString();
            }
            else if (context.ActionArguments.Count == 0)
            {
                isGetWithNoParametersRequest = true;
            }
        }
    }

    public override string GetETagForAction()
    {
        if (currentId != null)
        {
            return eTagRepository.GetByIdETag(executionContext.UserId, currentId);
        }
        else if (isGetWithNoParametersRequest)
        {
            return eTagRepository.GetByQueryETag(executionContext.UserId);
        }
        return null;
    }

    public override void SetETagForAction()
    {
        if (currentId != null)
        {
            eTagRepository.ChangeETag(currentId);
        }
        else if (isGetWithNoParametersRequest)
        {
            eTagRepository.ChangeWholeTableETag();
        }
    }
}
