using SolidOps.UM.Shared.Contracts.Events;
using SolidOps.UM.Shared.Domain.Entities;

namespace SolidOps.UM.Shared.Application.Events;

public interface IEntityEventHandler<TEntity, TEvent, T> : IEventHandler<TEvent>
    where TEntity : class, IDomainEntity<T>
    where TEvent : ISerializableEventOf<TEntity>
    where T : struct
{

}
