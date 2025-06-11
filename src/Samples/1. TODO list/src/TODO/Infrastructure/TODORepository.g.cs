using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SolidOps.TODO.Shared;
using SolidOps.TODO.Shared.Domain.Results;
using SolidOps.TODO.Shared.Domain;
using SolidOps.TODO.Shared.Infrastructure;
using SolidOps.TODO.Domain.Repositories;
using System.Linq.Expressions;
using SolidOps.SubZero;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
namespace SolidOps.TODO.Infrastructure.Repositories;
// Object [EN][AG]
public partial class ItemRepository : BaseItemRepository, IItemRepository
{
    public ItemRepository(IUserContext userContext, IServiceProvider serviceProvider, IConfiguration configuration
        , IEnumerable<IEntityRules<Guid, Domain.AggregateRoots.Item>> entityRules

        ) : base(userContext, serviceProvider, configuration, entityRules

            )
    {
    }
}
public abstract partial class BaseItemRepository
{
    protected readonly IUserContext userContext;
    protected readonly IServiceProvider serviceProvider;
    protected readonly IEnumerable<IEntityRules<Guid, Domain.AggregateRoots.Item>> entityRules;
    protected readonly IConfiguration configuration;

    public BaseItemRepository(IUserContext userContext, IServiceProvider serviceProvider, IConfiguration configuration
        , IEnumerable<IEntityRules<Guid, Domain.AggregateRoots.Item>> entityRules

        )
    {
        this.userContext = userContext;
        this.serviceProvider = serviceProvider;
        this.entityRules = entityRules.OrderBy(s => s.Priority).ToList();
        this.configuration = configuration;

    }
    protected bool IsNotDefaultId(Guid value)
    {
        return !IdentityKeyHelper<Guid>.IsDefault(value);
    }
    protected bool IsNotDefaultId(Guid? value)
    {
        return value.HasValue && !IdentityKeyHelper<Guid>.IsDefault(value.Value);
    }
    protected DbContext DbContext
    {
        get
        {
            return this.userContext.CurrentUnitOfWork.DbContext;
        }
    }
    #region Queries
    public virtual IQueryable<Domain.AggregateRoots.Item> Queryable
    {
        get
        {
            return DbContext.Set<Domain.AggregateRoots.Item>().AsQueryable();
        }
    }
    public IQueryable<Domain.AggregateRoots.Item> GetQueryable()
    {
        return Queryable;
    }
    public async Task<Domain.AggregateRoots.Item> GetSingleById(Guid id, string includes = null)
    {
        using var unitOfWork = userContext.StartUnitOfWork(UnitOfWorkType.Read);
        var queryable = Queryable.Where(u => u.Id.Equals(id));
        queryable = AddIncludeFilterData(includes, queryable);
        var entity = await GetSingleOfQueryable(queryable);
        unitOfWork.Complete();
        return entity;
    }
    public async Task<Domain.AggregateRoots.Item> GetSingleByComposedId(string composedId, string includes = null)
    {
        using var unitOfWork = userContext.StartUnitOfWork(UnitOfWorkType.Read);
        var queryable = Queryable.Where(u => u.Id.Equals(IdentityKeyHelper<Guid>.ReadString(composedId)));
        queryable = AddIncludeFilterData(includes, queryable);
        var entity = await GetSingleOfQueryable(queryable);
        unitOfWork.Complete();
        return entity;
    }

    // UniqueQueryableProperty 
    public async Task<Domain.AggregateRoots.Item> GetSingleByName(String name, string includes = null)
    {
        using var unitOfWork = userContext.StartUnitOfWork(UnitOfWorkType.Read);
        var queryable = Queryable.Where(u => u.Name.Equals(name));
        queryable = AddIncludeFilterData(includes, queryable);
        var entity = await GetSingleOfQueryable(queryable);
        unitOfWork.Complete();
        return entity;
    }

    public async Task<Domain.AggregateRoots.Item> GetSingleWhere(Expression<Func<Domain.AggregateRoots.Item, bool>> expression, string includes = null)
    {
        using var unitOfWork = userContext.StartUnitOfWork(UnitOfWorkType.Read);
        var queryable = Queryable;
        if (expression != null)
            queryable = queryable.Where(expression);
        queryable = AddIncludeFilterData(includes, queryable);
        var entity = await GetSingleOfQueryable(queryable);
        unitOfWork.Complete();
        return entity;
    }
    public async Task<IEnumerable<Domain.AggregateRoots.Item>> GetListWhere(Expression<Func<Domain.AggregateRoots.Item, bool>> expression, string includes = null)
    {
        using var unitOfWork = userContext.StartUnitOfWork(UnitOfWorkType.Read);
        var queryable = Queryable;
        if (expression != null)
            queryable = queryable.Where(expression);
        queryable = AddIncludeFilterData(includes, queryable);
        var result = await InternalGetListOfQueryable(queryable);
        unitOfWork.Complete();
        return result;
    }
    public async Task<IEnumerable<Domain.AggregateRoots.Item>> GetListOfQueryable(IQueryable<Domain.AggregateRoots.Item> queryable = null, string includes = null)
    {
        if (queryable == null)
        {
            queryable = Queryable;
        }
        queryable = AddIncludeFilterData(includes, queryable);
        var result = await InternalGetListOfQueryable(queryable);
        return result;
    }
    public async Task<IEnumerable<Domain.AggregateRoots.Item>> GetList()
    {
        return await Queryable.ToListAsync();
    }
    public virtual IQueryable<Domain.AggregateRoots.Item> AddIncludeFilterData(string includes, IQueryable<Domain.AggregateRoots.Item> queryable)
    {
        if (includes != null)
        {
            List<string> lstIncludes = includes.Split('|', StringSplitOptions.RemoveEmptyEntries).ToList();
            foreach (var include in lstIncludes)
            {
                queryable = queryable.Include(include);
            }
        }
        return queryable;
    }
    #endregion
    #region Commands
    public virtual async Task<IOpsResult<Guid>> Add(Domain.AggregateRoots.Item entity, Guid? forcedId = null)
    {
        using var unitOfWork = userContext.StartUnitOfWork(UnitOfWorkType.Write);
        IOpsResult result;
        foreach (var rule in entityRules)
        {
            result = await rule.OnBeforeAdd(entity, unitOfWork);
            if (result.HasError) return result.ToResult<Guid>();
        }
        var id = await DbSetAdd(entity, forcedId);
        foreach (var rule in entityRules)
        {
            result = await rule.OnAfterAdd(id, entity, unitOfWork);
            if (result.HasError) return result.ToResult<Guid>();
        }
        unitOfWork.Complete();
        return IOpsResult.Ok<Guid>(entity.Id);
    }
    public virtual async Task<IOpsResult> Update(Domain.AggregateRoots.Item entity)
    {
        using var unitOfWork = userContext.StartUnitOfWork(UnitOfWorkType.Write);
        IOpsResult result;
        foreach (var rule in entityRules)
        {
            result = await rule.OnBeforeUpdate(entity, unitOfWork);
            if (result.HasError) return result;
        }
        await DbSetUpdate(entity);
        foreach (var rule in entityRules)
        {
            result = await rule.OnAfterUpdate(entity, unitOfWork);
            if (result.HasError) return result;
        }
        unitOfWork.Complete();
        return IOpsResult.Ok();
    }
    public async Task<IOpsResult> Remove(Domain.AggregateRoots.Item entity)
    {
        return await RemoveWithId(entity.Id.ToString());
    }
    public async virtual Task<IOpsResult> RemoveWithId(string id)
    {
        using var unitOfWork = userContext.StartUnitOfWork(UnitOfWorkType.Write);
        var entity = await GetSingleByComposedId(id);
        if (entity == null)
        {
            return IOpsResult.Invalid($"could not find Item with Id {id}");
        }
        IOpsResult result;
        foreach (var rule in entityRules)
        {
            result = await rule.OnBeforeRemove(entity, unitOfWork);
            if (result.HasError) return result;
        }
        result = await CascadeDeleteAggregateData(entity);
        if (result.HasError) return result;
        await DbSetDelete(entity);
        foreach (var rule in entityRules)
        {
            result = await rule.OnAfterRemove(entity, unitOfWork);
            if (result.HasError) return result;
        }
        unitOfWork.Complete();
        return IOpsResult.Ok();
    }
    protected async Task<IOpsResult> CascadeDeleteAggregateData(Domain.AggregateRoots.Item entity)
    {
        await Task.CompletedTask;

        return IOpsResult.Ok();
    }
    #endregion
    #region FromDbSet
    public virtual async Task<Domain.AggregateRoots.Item> GetSingleOfQueryable(IQueryable<Domain.AggregateRoots.Item> queryable)
    {
        return await queryable.FirstOrDefaultAsync();
    }
    internal virtual async Task<IEnumerable<Domain.AggregateRoots.Item>> InternalGetListOfQueryable(IQueryable<Domain.AggregateRoots.Item> queryable)
    {
        return await queryable.ToListAsync();
    }
    public virtual async Task<Guid> DbSetAdd(Domain.AggregateRoots.Item entity, Guid? forceId)
    {
        await Task.CompletedTask; 
        DbContext.Set<Domain.AggregateRoots.Item>().Add(entity);
        return entity.Id;
    }
    public virtual async Task DbSetUpdate(Domain.AggregateRoots.Item entity)
    {
        await Task.CompletedTask; 
        DbContext.Set<Domain.AggregateRoots.Item>().Update(entity);
    }
    public virtual async Task DbSetDelete(Domain.AggregateRoots.Item entity)
    {
        await Task.CompletedTask; 
        DbContext.Set<Domain.AggregateRoots.Item>().Remove(entity);
    }
    #endregion
}