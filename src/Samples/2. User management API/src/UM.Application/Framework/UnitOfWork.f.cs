using SolidOps.UM.Domain;

namespace SolidOps.UM.Application;

/// <summary>
/// Saves the changes made to the aggregates loaded by the repositories of a request, in one
/// transaction. Implemented by the infrastructure layer.
/// </summary>
public interface IUnitOfWork
{
    /// <summary>Whether an aggregate root or an entity of type <typeparamref name="T"/> has this Id.</summary>
    Task<bool> ExistsAsync<T>(Guid id, CancellationToken cancellationToken = default) where T : Entity;

    /// <summary>Null when saved; a conflict when a unique index or a foreign key is violated.</summary>
    Task<Error?> SaveChangesAsync(CancellationToken cancellationToken = default);
}
