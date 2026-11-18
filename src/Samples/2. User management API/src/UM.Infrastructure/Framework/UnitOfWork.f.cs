using SolidOps.UM.Application;
using SolidOps.UM.Domain;
using Microsoft.EntityFrameworkCore;

namespace SolidOps.UM.Infrastructure;

/// <summary><see cref="IUnitOfWork"/> on the UM DbContext: one SaveChanges per request.</summary>
public sealed class UnitOfWork(UMDbContext db) : IUnitOfWork
{
    public Task<bool> ExistsAsync<T>(Guid id, CancellationToken cancellationToken = default) where T : Entity
    {
        return db.Set<T>().AnyAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<Error?> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            await db.SaveChangesAsync(cancellationToken);
            return null;
        }
        catch (DbUpdateException)
        {
            return Error.Conflict("A unique index or a foreign key is violated");
        }
    }
}
