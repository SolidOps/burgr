using SolidOps.Burgr.Shared.Domain.Entities;
using SolidOps.Burgr.Shared.Contracts.Results;
using SolidOps.Burgr.Shared.Domain.UnitOfWork;
using SolidOps.Burgr.Shared.Infrastructure;
namespace SolidOps.UM.Presentation.Cache;
// Object [EN][AG][CACHE]
public partial class BaseOrganizationCacheRules : IEntityRules<Guid, Domain.AggregateRoots.Organization>
{
    public int Priority
    {
        get
        {
            return 2;
        }
    }
    public virtual async Task<IOpsResult> OnBeforeAdd(Domain.AggregateRoots.Organization entity, IUnitOfWork unitOfWork)
    {
        return IOpsResult.Ok();
    }
    public virtual async Task<IOpsResult> OnAfterAdd(Guid id, Domain.AggregateRoots.Organization entity, IUnitOfWork unitOfWork)
    {
        unitOfWork.AddEvent(new OrganizationCacheEvent("Add", entity));
        return IOpsResult.Ok();
    }
    public virtual async Task<IOpsResult> OnBeforeUpdate(Domain.AggregateRoots.Organization entity, IUnitOfWork unitOfWork)
    {
        return IOpsResult.Ok();
    }
    public virtual async Task<IOpsResult> OnAfterUpdate(Domain.AggregateRoots.Organization entity, IUnitOfWork unitOfWork)
    {
        unitOfWork.AddEvent(new OrganizationCacheEvent("Update", entity));
        return IOpsResult.Ok();
    }
    public virtual async Task<IOpsResult> OnBeforeRemove(Domain.AggregateRoots.Organization entity, IUnitOfWork unitOfWork)
    {
        return IOpsResult.Ok();
    }
    public virtual async Task<IOpsResult> OnAfterRemove(Domain.AggregateRoots.Organization entity, IUnitOfWork unitOfWork)
    {
        unitOfWork.AddEvent(new OrganizationCacheEvent("Remove", entity));
        return IOpsResult.Ok();
    }
}
public partial class OrganizationCacheRules : BaseOrganizationCacheRules
{
}
public partial class OrganizationCacheEvent : SerializableEntityEventOf<Domain.AggregateRoots.Organization, Guid>
{
    // For deserialization only
    public OrganizationCacheEvent() : base(string.Empty, string.Empty, true)
    {
    }
    public OrganizationCacheEvent(string eventContext, string content) : base(eventContext, content, true)
    {
    }
    public OrganizationCacheEvent(string eventContext, Domain.AggregateRoots.Organization data) : base(eventContext, data, true)
    {
    }
}