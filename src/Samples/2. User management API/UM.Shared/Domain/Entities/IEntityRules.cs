using SolidOps.UM.Shared.Contracts.Results;
using SolidOps.UM.Shared.Domain.UnitOfWork;

namespace SolidOps.UM.Shared.Domain.Entities;

public interface IEntityRules<T, TEntity>
    where T : struct
    where TEntity : IDomainEntity<T>
{

    int Priority { get; }
    Task<IOpsResult> OnBeforeAdd(TEntity entity, IUnitOfWork unitOfWork);
    Task<IOpsResult> OnAfterAdd(T id, TEntity entity, IUnitOfWork unitOfWork);
    Task<IOpsResult> OnBeforeUpdate(TEntity entity, IUnitOfWork unitOfWork);
    Task<IOpsResult> OnAfterUpdate(TEntity entity, IUnitOfWork unitOfWork);
    Task<IOpsResult> OnBeforeRemove(TEntity entity, IUnitOfWork unitOfWork);
    Task<IOpsResult> OnAfterRemove(TEntity entity, IUnitOfWork unitOfWork);
}
