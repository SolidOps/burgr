using Microsoft.AspNetCore.Mvc.Filters;
using SolidOps.UM.Shared.Domain.Entities;
using SolidOps.UM.Shared.Domain.UnitOfWork;

namespace SolidOps.UM.Shared.Presentation.ETag.Filters;

public class ConstantETagFilter<T> : BaseETagFilter<ConstantETagFilter<T>.FakeEntity, T>, IActionFilter
    where T : struct
{
    public sealed class FakeEntity : BaseDomainEntity<T>
    {
        private FakeEntity() { }
    }

    public ConstantETagFilter(IETagRepository<FakeEntity, T> eTagRepository, IExecutionContext executionContext) : base(eTagRepository, executionContext)
    {

    }

    public override string GetETagForAction()
    {
        return eTagRepository.GetByQueryETag(string.Empty);
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
