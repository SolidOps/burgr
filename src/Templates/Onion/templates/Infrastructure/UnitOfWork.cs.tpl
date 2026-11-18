using _MODULE_NAMESPACE_.Application;
using _MODULE_NAMESPACE_.Domain;
using Microsoft.EntityFrameworkCore;

namespace _MODULE_NAMESPACE_.Infrastructure;

/// <summary><see cref="IUnitOfWork"/> on the _MODULE_ DbContext: one SaveChanges per request.</summary>
public sealed class UnitOfWork(_MODULE_DbContext db) : IUnitOfWork
{
    public Task<bool> ExistsAsync<T>(_IDENTITY_KEY_TYPE_ id, CancellationToken cancellationToken = default) where T : Entity
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
