using SolidOps.TODO.Shared.Domain;
using SolidOps.TODO.Shared.Domain.Results;

namespace SolidOps.TODO.Shared.Domain;

public interface IReadWriteDomainRepository<T, TEntity> : IReadDomainRepository<T, TEntity>
    where T : struct
    where TEntity : class, IEntityOfDomain<T>
{
    Task<IOpsResult<T>> Add(TEntity entity, T? forcedId = null);
    Task<IOpsResult> Update(TEntity entity);

    Task<IOpsResult> Remove(TEntity entity);
    Task<IOpsResult> RemoveWithId(string id);
}
