using _MODULE_NAMESPACE_.Domain;
using Microsoft.EntityFrameworkCore;

namespace _MODULE_NAMESPACE_.Infrastructure;

/// <summary>
/// <see cref="IRepository{T}"/> on the _MODULE_ DbContext. The repository of each aggregate
/// root derives from it; override <see cref="WithDetails"/> to load more than the list
/// navigations.
/// </summary>
public abstract class EfRepository<T>(_MODULE_DbContext db) : IRepository<T> where T : AggregateRoot
{
    protected _MODULE_DbContext Db { get; } = db;

    /// <summary>The navigations loaded by <see cref="FindAsync"/>.</summary>
    protected virtual IQueryable<T> WithDetails(IQueryable<T> query) => query;

    public Task<T?> FindAsync(_IDENTITY_KEY_TYPE_ id, CancellationToken cancellationToken = default)
    {
        return WithDetails(Db.Set<T>()).FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<Page<T>> QueryAsync(PageRequest request, CancellationToken cancellationToken = default)
    {
        IQueryable<T> query = Db.Set<T>().AsNoTracking();
        var totalCount = await query.CountAsync(cancellationToken);
        query = request.OrderBy is null
            ? query.OrderBy(x => x.Id)
            : request.Descending
                ? query.OrderByDescending(x => EF.Property<object>(x, request.OrderBy))
                : query.OrderBy(x => EF.Property<object>(x, request.OrderBy));
        var items = await query
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(cancellationToken);
        return new Page<T>(items, request.Page, request.PageSize, totalCount);
    }

    public void Add(T aggregate) => Db.Set<T>().Add(aggregate);

    public void Remove(T aggregate) => Db.Set<T>().Remove(aggregate);
}
