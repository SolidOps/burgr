namespace SolidOps.UM.Domain;

/// <summary>
/// Loads and stores the aggregate roots of type <typeparamref name="T"/>. Changes are saved by
/// the unit of work of the application layer, not by the repository.
/// </summary>
public interface IRepository<T> where T : AggregateRoot
{
    /// <summary>The aggregate root with its list navigations, tracked for changes; null when unknown.</summary>
    Task<T?> FindAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>One page of aggregate roots, without their navigations, not tracked.</summary>
    Task<Page<T>> QueryAsync(PageRequest request, CancellationToken cancellationToken = default);

    void Add(T aggregate);

    void Remove(T aggregate);
}

/// <param name="Page">From 1.</param>
/// <param name="PageSize">From 1 to <see cref="MaximumPageSize"/>.</param>
/// <param name="OrderBy">A property name (PascalCase); by Id when null.</param>
public sealed record PageRequest(int Page = 1, int PageSize = PageRequest.DefaultPageSize, string? OrderBy = null, bool Descending = false)
{
    public const int DefaultPageSize = 50;
    public const int MaximumPageSize = 200;
}

public sealed record Page<T>(IReadOnlyList<T> Items, int PageNumber, int PageSize, int TotalCount)
{
    public Page<TResult> Map<TResult>(Func<T, TResult> map) => new(Items.Select(map).ToList(), PageNumber, PageSize, TotalCount);
}
