using SolidOps.TODO.Shared.Domain;
using System.Linq.Expressions;

namespace SolidOps.TODO.Shared.Domain;

public interface IReadDomainRepository<T, TEntity>
    where T : struct
    where TEntity : IEntityOfDomain<T>
{
    IQueryable<TEntity> GetQueryable();
    Task<TEntity> GetSingleById(T id, string includes = null);
    Task<TEntity> GetSingleByIdInJustAdded(T id);
    Task<IEnumerable<TEntity>> GetListOfQueryable(IQueryable<TEntity> queryable = null, string includes = null);
    Task<IEnumerable<TEntity>> GetListWhere(Expression<Func<TEntity, bool>> expression, string includes = null);
}
