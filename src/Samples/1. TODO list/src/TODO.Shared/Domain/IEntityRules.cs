using SolidOps.TODO.Shared.Domain.Results;

namespace SolidOps.TODO.Shared.Domain;

public interface IEntityRules<T, TEntity>
    where T : struct
    where TEntity : IEntityOfDomain<T>
{

    int Priority { get; }
    Task<IOpsResult> OnBeforeAdd(TEntity entity, IUnitOfWork unitOfWork);
    Task<IOpsResult> OnAfterAdd(T id, TEntity entity, IUnitOfWork unitOfWork);
    Task<IOpsResult> OnBeforeUpdate(TEntity entity, IUnitOfWork unitOfWork);
    Task<IOpsResult> OnAfterUpdate(TEntity entity, IUnitOfWork unitOfWork);
    Task<IOpsResult> OnBeforeRemove(TEntity entity, IUnitOfWork unitOfWork);
    Task<IOpsResult> OnAfterRemove(TEntity entity, IUnitOfWork unitOfWork);
}
