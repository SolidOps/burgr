using Microsoft.Extensions.Logging;
using SolidOps.UM.Shared.Domain.UnitOfWork;

namespace SolidOps.UM.Shared.Domain.Entities;

[Serializable]
public abstract class BaseDomainEntity<T, TEntity> : BaseDomainEntity<T>
    where TEntity : IDomainEntity<T>
    where T : struct
{
    public abstract void CopyValues(TEntity copy);
}
