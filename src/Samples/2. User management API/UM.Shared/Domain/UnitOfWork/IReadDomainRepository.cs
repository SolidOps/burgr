using SolidOps.UM.Shared.Domain.Entities;
using System.Linq.Expressions;

namespace SolidOps.UM.Shared.Domain.UnitOfWork;

public interface IReadDomainRepository<T, TEntity>
    where T : struct
    where TEntity : IDomainEntity<T>
{
    bool LazyLoadingEnabled { get; set; }
    IQueryable<TEntity> GetQueryable();
    Task<TEntity> GetSingleById(T id, string includes = null, bool disableAccessRightCheck = false);
    Task<TEntity> GetSingleByComposedId(string composedId, string includes = null, bool disableAccessRightCheck = false);
    Task<IEnumerable<TEntity>> GetListOfQueryable(IQueryable<TEntity> queryable = null, string includes = null, bool disableAccessRightCheck = false);
    Task<IEnumerable<TEntity>> GetListWhere(Expression<Func<TEntity, bool>> expression, string includes = null, bool disableAccessRightCheck = false);
}
