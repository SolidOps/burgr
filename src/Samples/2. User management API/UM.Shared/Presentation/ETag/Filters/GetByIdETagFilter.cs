using Microsoft.AspNetCore.Mvc.Filters;
using SolidOps.UM.Shared.Domain.Entities;
using SolidOps.UM.Shared.Domain.UnitOfWork;

namespace SolidOps.UM.Shared.Presentation.ETag.Filters;

public class GetByIdETagFilter<TEntity, T> : BaseETagFilter<TEntity, T>, IActionFilter
    where TEntity : BaseDomainEntity<T>
    where T : struct
{
    private string currentId;
    public GetByIdETagFilter(IETagRepository<TEntity, T> eTagRepository, IExecutionContext executionContext) : base(eTagRepository, executionContext)
    {

    }

    public override void SetRequestContext(ActionExecutingContext context)
    {
        currentId = context.ActionArguments["id"].ToString();
    }

    public override string GetETagForAction()
    {
        if (currentId != null)
        {
            return eTagRepository.GetByIdETag(executionContext.UserId, currentId);
        }
        return null;
    }

    public override void SetETagForAction()
    {
        if (currentId != null)
        {
            eTagRepository.ChangeETag(currentId);
        }
    }
}
