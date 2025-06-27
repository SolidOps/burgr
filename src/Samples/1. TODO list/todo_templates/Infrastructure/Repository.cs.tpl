using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SolidOps.TODO.Shared;
using SolidOps.TODO.Shared.Domain.Results;
using SolidOps.TODO.Shared.Domain;
using SolidOps.TODO.Shared.Infrastructure;
using MetaCorp.Template.Domain.Repositories;
using System.Linq.Expressions;
using SolidOps.SubZero;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;

namespace MetaCorp.Template.Infrastructure.Repositories;

#region foreach MODEL[EN][AG]
public partial class CLASSNAMERepository : BaseCLASSNAMERepository, ICLASSNAMERepository
{
    public CLASSNAMERepository(IUserContext userContext, IServiceProvider serviceProvider, IConfiguration configuration
        , IEnumerable<IEntityRules<_IDENTITY_KEY_TYPE_, Domain._DOMAINTYPE_.CLASSNAME>> entityRules
    #region foreach DEPENDENCY[EN][AG]
        , DEPENDENCYNAMESPACE.Domain.Repositories.IDEPENDENCYTYPERepository dependencyDEPENDENCYTYPERepository
    #endregion foreach DEPENDENCY
        ) : base(userContext, serviceProvider, configuration, entityRules
    #region foreach DEPENDENCY[EN][AG]
        , dependencyDEPENDENCYTYPERepository
    #endregion foreach DEPENDENCY
            )
    {
    }
}

public abstract partial class BaseCLASSNAMERepository
{
    protected readonly IUserContext userContext;
    protected readonly IServiceProvider serviceProvider;
    protected readonly IEnumerable<IEntityRules<_IDENTITY_KEY_TYPE_, Domain._DOMAINTYPE_.CLASSNAME>> entityRules;
    protected readonly IConfiguration configuration;

    #region foreach DEPENDENCY[EN][AG]
    protected readonly DEPENDENCYNAMESPACE.Domain.Repositories.IDEPENDENCYTYPERepository _dependencyDEPENDENCYTYPERepository;
    #endregion foreach DEPENDENCY

    public BaseCLASSNAMERepository(IUserContext userContext, IServiceProvider serviceProvider, IConfiguration configuration
        , IEnumerable<IEntityRules<_IDENTITY_KEY_TYPE_, Domain._DOMAINTYPE_.CLASSNAME>> entityRules
    #region foreach DEPENDENCY[EN][AG]
        , DEPENDENCYNAMESPACE.Domain.Repositories.IDEPENDENCYTYPERepository dependencyDEPENDENCYTYPERepository // for base
    #endregion foreach DEPENDENCY
        )
    {
        this.userContext = userContext;
        this.serviceProvider = serviceProvider;
        this.entityRules = entityRules.OrderBy(s => s.Priority).ToList();
        this.configuration = configuration;

        #region foreach DEPENDENCY[EN][AG]
        _dependencyDEPENDENCYTYPERepository = dependencyDEPENDENCYTYPERepository ?? throw new ArgumentNullException(nameof(dependencyDEPENDENCYTYPERepository));
        #endregion foreach DEPENDENCY
    }

    protected bool IsNotDefaultId(_IDENTITY_KEY_TYPE_ value)
    {
        return !IdentityKeyHelper<_IDENTITY_KEY_TYPE_>.IsDefault(value);
    }

    protected bool IsNotDefaultId(_IDENTITY_KEY_TYPE_? value)
    {
        return value.HasValue && !IdentityKeyHelper<_IDENTITY_KEY_TYPE_>.IsDefault(value.Value);
    }

    protected DbContext DbContext
    {
        get
        {
            return this.userContext.CurrentUnitOfWork.DbContext;
        }
    }

    #region Queries
    public virtual IQueryable<Domain._DOMAINTYPE_.CLASSNAME> Queryable
    {
        get
        {
            return DbContext.Set<Domain._DOMAINTYPE_.CLASSNAME>().AsQueryable();
        }
    }

    public IQueryable<Domain._DOMAINTYPE_.CLASSNAME> GetQueryable()
    {
        return Queryable;
    }

    public async Task<Domain._DOMAINTYPE_.CLASSNAME> GetSingleById(_IDENTITY_KEY_TYPE_ id, string includes = null)
    {
        using var unitOfWork = userContext.StartUnitOfWork(UnitOfWorkType.Read);
        var queryable = Queryable.Where(u => u.Id.Equals(id));
        queryable = AddIncludeFilterData(includes, queryable);
        var entity = await GetSingleOfQueryable(queryable);
        unitOfWork.Complete();
        return entity;
    }
    public async Task<Domain._DOMAINTYPE_.CLASSNAME> GetSingleByIdInJustAdded(_IDENTITY_KEY_TYPE_ id)
    {
        using var unitOfWork = userContext.StartUnitOfWork(UnitOfWorkType.Read);
        
        var entity = DbContext.ChangeTracker.Entries()
        .Where(x => x.State == EntityState.Added && x.Entity is Domain._DOMAINTYPE_.CLASSNAME)
        .Select(x => x.Entity as Domain._DOMAINTYPE_.CLASSNAME)
        .Where(u => u.Id.Equals(id))
        .SingleOrDefault();
        
        unitOfWork.Complete();
        return entity;
    }
    public async Task<Domain._DOMAINTYPE_.CLASSNAME> GetSingleByComposedId(string composedId, string includes = null)
    {
        using var unitOfWork = userContext.StartUnitOfWork(UnitOfWorkType.Read);
        var queryable = Queryable.Where(u => u.Id.Equals(IdentityKeyHelper<_IDENTITY_KEY_TYPE_>.ReadString(composedId)));
        queryable = AddIncludeFilterData(includes, queryable);
        var entity = await GetSingleOfQueryable(queryable);
        unitOfWork.Complete();
        return entity;
    }
    #region foreach QUERYABLE_PROPERTY
    public async Task<IEnumerable<Domain._DOMAINTYPE_.CLASSNAME>> GetListBy_SIMPLE__PROPERTYNAME_(_SIMPLE__TYPE_ _SIMPLE__FIELDNAME_, string includes = null)
    {
        using var unitOfWork = userContext.StartUnitOfWork(UnitOfWorkType.Read);
        var queryable = Queryable.Where(u => u._PROPERTYNAME_.Equals(_SIMPLE__FIELDNAME_));
        queryable = AddIncludeFilterData(includes, queryable);
        var result = await InternalGetListOfQueryable(queryable);
        unitOfWork.Complete();
        return result as IEnumerable<Domain._DOMAINTYPE_.CLASSNAME>;

    }
    #endregion foreach QUERYABLE_PROPERTY

    #region foreach UNIQUE_QUERYABLE_PROPERTY
    public async Task<Domain._DOMAINTYPE_.CLASSNAME> GetSingleBy_SIMPLE__PROPERTYNAME_(_SIMPLE__TYPE_ _SIMPLE__FIELDNAME_, string includes = null)
    {
        using var unitOfWork = userContext.StartUnitOfWork(UnitOfWorkType.Read);
        var queryable = Queryable.Where(u => u._PROPERTYNAME_.Equals(_SIMPLE__FIELDNAME_));
        queryable = AddIncludeFilterData(includes, queryable);
        var entity = await GetSingleOfQueryable(queryable);
        unitOfWork.Complete();
        return entity;
    }
    #endregion foreach UNIQUE_QUERYABLE_PROPERTY

    #region foreach MULTIPLE_UNIQUE_QUERYABLE_PROPERTY
    public async Task<Domain._DOMAINTYPE_.CLASSNAME> GetSingleBy_MULTIPLE_CONSTRAINT_(/*_CONSTRAINT_PARAMS_TYPED_*/, string includes = null)
    {
        using var unitOfWork = userContext.StartUnitOfWork(UnitOfWorkType.Read);
        var entity = await GetSingleWhere(/*_CONSTRAINT_EXPRESSION_*/, includes);
        unitOfWork.Complete();
        return entity;
    }
    #endregion foreach MULTIPLE_UNIQUE_QUERYABLE_PROPERTY

    public async Task<Domain._DOMAINTYPE_.CLASSNAME> GetSingleWhere(Expression<Func<Domain._DOMAINTYPE_.CLASSNAME, bool>> expression, string includes = null)
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

    public async Task<IEnumerable<Domain._DOMAINTYPE_.CLASSNAME>> GetListWhere(Expression<Func<Domain._DOMAINTYPE_.CLASSNAME, bool>> expression, string includes = null)
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

    public async Task<IEnumerable<Domain._DOMAINTYPE_.CLASSNAME>> GetListOfQueryable(IQueryable<Domain._DOMAINTYPE_.CLASSNAME> queryable = null, string includes = null)
    {
        if (queryable == null)
        {
            queryable = Queryable;
        }
        queryable = AddIncludeFilterData(includes, queryable);
        var result = await InternalGetListOfQueryable(queryable);
        return result;
    }

    public async Task<IEnumerable<Domain._DOMAINTYPE_.CLASSNAME>> GetList()
    {
        return await Queryable.ToListAsync();
    }

    public virtual IQueryable<Domain._DOMAINTYPE_.CLASSNAME> AddIncludeFilterData(string includes, IQueryable<Domain._DOMAINTYPE_.CLASSNAME> queryable)
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
    public virtual async Task<IOpsResult<_IDENTITY_KEY_TYPE_>> Add(Domain._DOMAINTYPE_.CLASSNAME entity, _IDENTITY_KEY_TYPE_? forcedId = null)
    {
        using var unitOfWork = userContext.StartUnitOfWork(UnitOfWorkType.Write);

        IOpsResult result;

        foreach (var rule in entityRules)
        {
            result = await rule.OnBeforeAdd(entity, unitOfWork);
            if (result.HasError) return result.ToResult<_IDENTITY_KEY_TYPE_>();
        }

        var id = await DbSetAdd(entity, forcedId);

        foreach (var rule in entityRules)
        {
            result = await rule.OnAfterAdd(id, entity, unitOfWork);
            if (result.HasError) return result.ToResult<_IDENTITY_KEY_TYPE_>();
        }

        unitOfWork.Complete();
        return IOpsResult.Ok<_IDENTITY_KEY_TYPE_>(entity.Id);
    }

    public virtual async Task<IOpsResult> Update(Domain._DOMAINTYPE_.CLASSNAME entity)
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

    public async Task<IOpsResult> Remove(Domain._DOMAINTYPE_.CLASSNAME entity)
    {
        return await RemoveWithId(entity.Id.ToString());
    }

    public async virtual Task<IOpsResult> RemoveWithId(string id)
    {
        using var unitOfWork = userContext.StartUnitOfWork(UnitOfWorkType.Write);
        var entity = await GetSingleByComposedId(id);
        if (entity == null)
        {
            return IOpsResult.Invalid($"could not find CLASSNAME with Id {id}");
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

    protected async Task<IOpsResult> CascadeDeleteAggregateData(Domain._DOMAINTYPE_.CLASSNAME entity)
    {
        await Task.CompletedTask;
        #region foreach DEPENDENTFOR[EN][AG]
        {
            var repo = GetRequiredService<DEPENDENCYNAMESPACE.Domain.Repositories._PROPERTYFULLINTERFACE_Repository>();
            var relatedEntities = await repo.GetListWhere(e => e._PROPERTYNAME_Id == entity.Id, null, false);
            foreach (var relatedEntity in relatedEntities)
            {
                var result = await repo.Remove(relatedEntity);
                if (result.HasError) return result;
            }
        }
        #endregion foreach DEPENDENTFOR

        return IOpsResult.Ok();
    }

    #endregion

    #region FromDbSet
    public virtual async Task<Domain._DOMAINTYPE_.CLASSNAME> GetSingleOfQueryable(IQueryable<Domain._DOMAINTYPE_.CLASSNAME> queryable)
    {
        return await queryable.FirstOrDefaultAsync();
    }

    internal virtual async Task<IEnumerable<Domain._DOMAINTYPE_.CLASSNAME>> InternalGetListOfQueryable(IQueryable<Domain._DOMAINTYPE_.CLASSNAME> queryable)
    {
        return await queryable.ToListAsync();
    }

    public virtual async Task<_IDENTITY_KEY_TYPE_> DbSetAdd(Domain._DOMAINTYPE_.CLASSNAME entity, _IDENTITY_KEY_TYPE_? forceId)
    {
        await Task.CompletedTask; 
        DbContext.Set<Domain._DOMAINTYPE_.CLASSNAME>().Add(entity);
        return entity.Id;
    }

    public virtual async Task DbSetUpdate(Domain._DOMAINTYPE_.CLASSNAME entity)
    {
        await Task.CompletedTask; 
        DbContext.Set<Domain._DOMAINTYPE_.CLASSNAME>().Update(entity);
    }

    public virtual async Task DbSetDelete(Domain._DOMAINTYPE_.CLASSNAME entity)
    {
        await Task.CompletedTask; 
        DbContext.Set<Domain._DOMAINTYPE_.CLASSNAME>().Remove(entity);
    }
    #endregion
}
#endregion foreach MODEL