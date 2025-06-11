using SolidOps.UM.Shared.Contracts.Results;
using SolidOps.UM.Shared.Domain.Entities;

namespace SolidOps.UM.Shared.Domain.UnitOfWork;

public interface IReadWriteDomainRepository<T, TEntity> : IReadDomainRepository<T, TEntity>
    where T : struct
    where TEntity : class, IDomainEntity<T>
{
    Task<IOpsResult<T>> Add(TEntity entity, T? forcedId = null);
    Task<IOpsResult> Update(TEntity entity);

    Task<IOpsResult> Remove(TEntity entity);
    Task<IOpsResult> RemoveWithId(string id);
}
