using Microsoft.AspNetCore.Mvc.Filters;
using SolidOps.UM.Shared.Domain.Entities;
using SolidOps.UM.Shared.Domain.UnitOfWork;

namespace SolidOps.UM.Shared.Presentation.ETag.Filters;

public class GetListETagFilter<TEntity, T> : BaseETagFilter<TEntity, T>, IActionFilter
    where TEntity : BaseDomainEntity<T>
    where T : struct
{
    public GetListETagFilter(IETagRepository<TEntity, T> eTagRepository, IExecutionContext executionContext) : base(eTagRepository, executionContext)
    {

    }

    public override string GetETagForAction()
    {
        return eTagRepository.GetByQueryETag(executionContext.UserId);
    }

    public override void SetETagForAction()
    {
        eTagRepository.ChangeWholeTableETag();
    }

    public override void SetRequestContext(ActionExecutingContext context)
    {
        // do nothing
    }
}
