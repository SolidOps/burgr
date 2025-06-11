using SolidOps.UM.Shared.Contracts.Events;
using SolidOps.UM.Shared.Domain.Entities;

namespace SolidOps.UM.Shared.Domain.Events;

public interface IEntitySerializableEventOf<TEntity, T> : ISerializableEventOf<TEntity>
    where TEntity : class, IDomainEntity<T>
    where T : struct
{
    string IdentityFullType { get; }
    string IdentityAssemblyName { get; }
}