using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SolidOps.UM.Shared.Contracts.DTO;
using SolidOps.UM.Shared.Contracts.Results;
using SolidOps.UM.Shared.Domain.Configuration;
using SolidOps.UM.Shared.Domain.Entities;
using SolidOps.UM.Shared.Domain.UnitOfWork;
using SolidOps.UM.Shared.Infrastructure;
using SolidOps.UM.Shared.Infrastructure.Queries;
using MetaCorp.Template.Domain.Queries;
using MetaCorp.Template.Domain.Repositories;
using System.Linq.Expressions;
using SolidOps.SubZero;

namespace MetaCorp.Template.Infrastructure.Repositories;

#region foreach MODEL[EN][AG]
public partial class CLASSNAMERepository : BaseCLASSNAMERepository, ICLASSNAMERepository
{
    public CLASSNAMERepository(IExecutionContext executionContext, ILoggerFactory loggerFactory, IServiceProvider serviceProvider, IExtendedConfiguration configuration
        , IEnumerable<IEntityRules<_IDENTITY_KEY_TYPE_, Domain._DOMAINTYPE_.CLASSNAME>> entityRules
    #region foreach DEPENDENCY[EN][AG]
        , DEPENDENCYNAMESPACE.Domain.Repositories.IDEPENDENCYTYPERepository dependencyDEPENDENCYTYPERepository
    #endregion foreach DEPENDENCY
        ) : base(executionContext, loggerFactory, serviceProvider, configuration, entityRules
    #region foreach DEPENDENCY[EN][AG]
        , dependencyDEPENDENCYTYPERepository
    #endregion foreach DEPENDENCY
            )
    {
    }
}

public abstract partial class BaseCLASSNAMERepository
{
    protected readonly IExecutionContext executionContext;
    protected readonly ILogger<ICLASSNAMERepository> logger;
    protected readonly IServiceProvider serviceProvider;
    protected readonly IEnumerable<IEntityRules<_IDENTITY_KEY_TYPE_, Domain._DOMAINTYPE_.CLASSNAME>> entityRules;
    protected readonly IExtendedConfiguration configuration;

    public DatabaseInfo DatabaseInfo { get; set; }

    #region foreach DEPENDENCY[EN][AG]
    protected readonly DEPENDENCYNAMESPACE.Domain.Repositories.IDEPENDENCYTYPERepository _dependencyDEPENDENCYTYPERepository;
    #endregion foreach DEPENDENCY

    public BaseCLASSNAMERepository(IExecutionContext executionContext, ILoggerFactory loggerFactory, IServiceProvider serviceProvider, IExtendedConfiguration configuration
        , IEnumerable<IEntityRules<_IDENTITY_KEY_TYPE_, Domain._DOMAINTYPE_.CLASSNAME>> entityRules
    #region foreach DEPENDENCY[EN][AG]
        , DEPENDENCYNAMESPACE.Domain.Repositories.IDEPENDENCYTYPERepository dependencyDEPENDENCYTYPERepository // for base
    #endregion foreach DEPENDENCY
        )
    {
        this.executionContext = executionContext;
        this.logger = loggerFactory.CreateLogger<ICLASSNAMERepository>();
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

    public Domain._DOMAINTYPE_.CLASSNAME MapFromDomain(Domain._DOMAINTYPE_.CLASSNAME domain, bool isNew)
    {
        if (domain == null)
            return null;

        Domain._DOMAINTYPE_.CLASSNAME efEntity;
        if (isNew)
        {
            efEntity = Domain._DOMAINTYPE_.CLASSNAME.CreateEmpty();
        }
        else
        {
            efEntity = Queryable.Where(e => e.Id.Equals(domain.Id)).SingleOrDefault();
            if (efEntity == null || domain == null)
                return null;
        }
        if (!Equals(efEntity.Id, domain.Id))
            efEntity.SetId(domain.Id);
        #region foreach PROPERTY[S][NO]
        if (!Equals(efEntity._SIMPLE__PROPERTYNAME_, domain._SIMPLE__PROPERTYNAME_))
            efEntity._SIMPLE__PROPERTYNAME_ = domain._SIMPLE__PROPERTYNAME_;
        #endregion foreach PROPERTY

        #region foreach PROPERTY[E][NO]
        if (!Equals(efEntity._ENUM__PROPERTYNAME_, domain._ENUM__PROPERTYNAME_))
            efEntity._ENUM__PROPERTYNAME_ = domain._ENUM__PROPERTYNAME_;
        #endregion foreach PROPERTY

        #region foreach PROPERTY[M][NO][EN][AG]
        if (!Equals(efEntity._PROPERTYNAME_Id, domain._PROPERTYNAME_Id))
        {
            if (IsNotDefaultId(domain._PROPERTYNAME_Id))
            {
                efEntity._PROPERTYNAME_Id = domain._PROPERTYNAME_Id;
            }
            else
            {
                efEntity._PROPERTYNAME_Id = default(_IDENTITY_KEY_TYPE_/*_ISNULL_*/);
            }
        }
        #endregion foreach PROPERTY

        #region foreach PROPERTY[R][NO][EN][AG]
        if (!Equals(efEntity._REF__PROPERTYNAME_Id, domain._REF__PROPERTYNAME_Id))
        {
            if (IsNotDefaultId(domain._REF__PROPERTYNAME_Id))
            {
                efEntity._REF__PROPERTYNAME_Id = domain._REF__PROPERTYNAME_Id;
            }
            else
            {
                efEntity._REF__PROPERTYNAME_Id = default(_IDENTITY_KEY_TYPE_/*_ISNULL_*/);
            }
        }
        #endregion foreach PROPERTY

        #region foreach PROPERTY[M][R][NO][VO]
        if (!Equals(efEntity._VO__PROPERTYNAME_, domain._VO__PROPERTYNAME_))
        {
            efEntity._VO__PROPERTYNAME_ = domain._VO__PROPERTYNAME_;
        }
        #endregion foreach PROPERTY

        #region foreach PROPERTY[M][R][LNA][VO]
        if (!Equals(efEntity._VO__NAVIGATION__NAVIGATION__PROPERTYNAME_, domain._VO__NAVIGATION__NAVIGATION__PROPERTYNAME_))
        {
            efEntity._VO__NAVIGATION__NAVIGATION__PROPERTYNAME_ = domain._VO__NAVIGATION__NAVIGATION__PROPERTYNAME_;
        }
        #endregion foreach PROPERTY

        return efEntity;
    }

    public Domain._DOMAINTYPE_.CLASSNAME MapToDomain(Domain._DOMAINTYPE_.CLASSNAME efEntity, Dictionary<Type, List<object>> mappingCache, bool enableLazyLoading = false)
    {
        if (efEntity == null)
            return null;

        efEntity.LazyLoadingEnabled = enableLazyLoading;

        if (!mappingCache.ContainsKey(typeof(Domain._DOMAINTYPE_.CLASSNAME)))
        {
            mappingCache.Add(typeof(Domain._DOMAINTYPE_.CLASSNAME), new List<object>());
        }

        Domain._DOMAINTYPE_.CLASSNAME cachedEntity = mappingCache[typeof(Domain._DOMAINTYPE_.CLASSNAME)].Cast<Domain._DOMAINTYPE_.CLASSNAME>().SingleOrDefault(m => m.Id.Equals(efEntity.Id));
        if (cachedEntity == null)
        {
            mappingCache[typeof(Domain._DOMAINTYPE_.CLASSNAME)].Add(efEntity);

            #region foreach PROPERTY[M][NO][EN][AG]
            if (efEntity._PROPERTYNAME_ != null)
            {
                var subRepository = serviceProvider.GetRequiredService<Domain.Repositories.I_PROPERTYTYPE_Repository>();
                efEntity._PROPERTYNAME_ = subRepository.MapToDomain(efEntity._PROPERTYNAME_, mappingCache, enableLazyLoading);
            }
            #endregion foreach PROPERTY

            #region foreach PROPERTY[R][NO][EN][AG]
            if (efEntity._PROPERTYNAME_ != null)
            {
                var subRepository = serviceProvider.GetRequiredService<DEPENDENCYNAMESPACE.Domain.Repositories.I_PROPERTYTYPE_Repository>();
                efEntity._REF__PROPERTYNAME_ = subRepository.MapToDomain(efEntity._REF__PROPERTYNAME_, mappingCache, enableLazyLoading);
            }
            #endregion foreach PROPERTY

            // navigation
            #region foreach PROPERTY[M][SNA]
            if (efEntity._NAVIGATION__PROPERTYNAME_ != null)
            {
                var subRepository = serviceProvider.GetRequiredService<Domain.Repositories.I_PROPERTYTYPE_Repository>();
                efEntity._NAVIGATION__PROPERTYNAME_ = subRepository.MapToDomain(efEntity._NAVIGATION__PROPERTYNAME_, mappingCache, enableLazyLoading);
            }
            #endregion foreach PROPERTY

            #region foreach PROPERTY[R][SNA]
            if (efEntity._REF__NAVIGATION__PROPERTYNAME_ != null)
            {
                var subRepository = serviceProvider.GetRequiredService<DEPENDENCYNAMESPACE.Domain.Repositories.I_PROPERTYTYPE_Repository>();
                efEntity._REF__NAVIGATION__PROPERTYNAME_ = subRepository.MapToDomain(efEntity._REF__NAVIGATION__PROPERTYNAME_, mappingCache, enableLazyLoading);
            }
            #endregion foreach PROPERTY

            #region foreach PROPERTY[M][LNA][EN][AG]
            if (efEntity._NAVIGATION__NAVIGATION__PROPERTYNAME_ != null)
            {
                foreach (var item in efEntity._NAVIGATION__NAVIGATION__PROPERTYNAME_)
                {
                    if (item != null)
                    {
                        var subRepository = serviceProvider.GetRequiredService<Domain.Repositories.I_PROPERTYTYPE_Repository>();
                        subRepository.MapToDomain(item, mappingCache, enableLazyLoading);
                    }
                }
            }
            #endregion foreach PROPERTY

            #region foreach PROPERTY[R][LNA][EN][AG]
            if (efEntity._REF__NAVIGATION__NAVIGATION__PROPERTYNAME_ != null)
            {
                foreach (var item in efEntity._REF__NAVIGATION__NAVIGATION__PROPERTYNAME_)
                {
                    if (item != null)
                    {
                        var subRepository = serviceProvider.GetRequiredService<DEPENDENCYNAMESPACE.Domain.Repositories.I_PROPERTYTYPE_Repository>();
                        subRepository.MapToDomain(item, mappingCache, enableLazyLoading);
                    }
                }
            }
            #endregion foreach PROPERTY
        }

        return efEntity;
    }

    protected T GetRequiredService<T>()
    {
        return this.serviceProvider.GetRequiredService<T>();
    }

    public bool LazyLoadingEnabled { get; set; } = false;

    #region Queries
    public virtual IQueryable<Domain._DOMAINTYPE_.CLASSNAME> Queryable
    {
        get
        {
            throw new NotImplementedException();
        }
    }

    public IQueryable<Domain._DOMAINTYPE_.CLASSNAME> GetQueryable()
    {
        return Queryable;
    }

    public async Task<Domain._DOMAINTYPE_.CLASSNAME> GetSingleById(_IDENTITY_KEY_TYPE_ id, string includes = null, bool disableAccessRightCheck = false)
    {
        logger?.LogDebug("GetSingleById");
        using var unitOfWork = executionContext.StartUnitOfWork("Template", "CLASSNAMERepository GetSingleById", UnitOfWorkType.Read);
        var queryable = Queryable.Where(u => u.Id.Equals(id));
        if (!disableAccessRightCheck)
        {
            queryable = await AddAccessRightsCheckInQuery(queryable);
        }
        queryable = AddIncludeFilterData(includes, queryable);
        var entity = await GetSingleOfQueryable(queryable, LazyLoadingEnabled || includes == "Lazy");
        entity?.SetId(entity.Id);
        unitOfWork.Complete();
        return entity;
    }
    public async Task<Domain._DOMAINTYPE_.CLASSNAME> GetSingleByComposedId(string composedId, string includes = null, bool disableAccessRightCheck = false)
    {
        logger?.LogDebug("GetSingleByComposedId");
        using var unitOfWork = executionContext.StartUnitOfWork("Template", "CLASSNAMERepository GetSingleByComposedId", UnitOfWorkType.Read);
        var queryable = Queryable.Where(u => u.Id.Equals(IdentityKeyHelper<_IDENTITY_KEY_TYPE_>.ReadString(composedId)));
        if (!disableAccessRightCheck)
        {
            queryable = await AddAccessRightsCheckInQuery(queryable);
        }
        queryable = AddIncludeFilterData(includes, queryable);
        var entity = await GetSingleOfQueryable(queryable, LazyLoadingEnabled || includes == "Lazy");
        entity?.SetId(entity.Id);
        unitOfWork.Complete();
        return entity;
    }
    #region foreach QUERYABLE_PROPERTY
    public async Task<IEnumerable<Domain._DOMAINTYPE_.CLASSNAME>> GetListBy_SIMPLE__PROPERTYNAME_(_SIMPLE__TYPE_ _SIMPLE__FIELDNAME_, string includes = null, bool disableAccessRightCheck = false)
    {
        logger?.LogDebug("GetListBy_SIMPLE__PROPERTYNAME_");
        using var unitOfWork = executionContext.StartUnitOfWork("Template", "CLASSNAMERepository GetListBy_PROPERTYNAME_", UnitOfWorkType.Read);
        var queryable = Queryable.Where(u => u._PROPERTYNAME_.Equals(_SIMPLE__FIELDNAME_));
        if (!disableAccessRightCheck)
        {
            queryable = await AddAccessRightsCheckInQuery(queryable);
        }
        queryable = AddIncludeFilterData(includes, queryable);
        var result = await InternalGetListOfQueryable(queryable, LazyLoadingEnabled || includes == "Lazy");
        foreach (var item in result)
        {
            item.SetId(item.Id);
        }
        unitOfWork.Complete();
        return result as IEnumerable<Domain._DOMAINTYPE_.CLASSNAME>;

    }
    #endregion foreach QUERYABLE_PROPERTY

    #region foreach UNIQUE_QUERYABLE_PROPERTY
    public async Task<Domain._DOMAINTYPE_.CLASSNAME> GetSingleBy_SIMPLE__PROPERTYNAME_(_SIMPLE__TYPE_ _SIMPLE__FIELDNAME_, string includes = null, bool disableAccessRightCheck = false)
    {
        logger?.LogDebug("GetSingleBy_SIMPLE__PROPERTYNAME_");
        using var unitOfWork = executionContext.StartUnitOfWork("Template", "CLASSNAMERepository GetSingleBy_SIMPLE__PROPERTYNAME_", UnitOfWorkType.Read);
        var queryable = Queryable.Where(u => u._PROPERTYNAME_.Equals(_SIMPLE__FIELDNAME_));
        if (!disableAccessRightCheck)
        {
            queryable = await AddAccessRightsCheckInQuery(queryable);
        }
        queryable = AddIncludeFilterData(includes, queryable);
        var entity = await GetSingleOfQueryable(queryable, LazyLoadingEnabled || includes == "Lazy");
        entity?.SetId(entity.Id);
        unitOfWork.Complete();
        return entity;
    }
    #endregion foreach UNIQUE_QUERYABLE_PROPERTY

    #region foreach MULTIPLE_UNIQUE_QUERYABLE_PROPERTY
    public async Task<Domain._DOMAINTYPE_.CLASSNAME> GetSingleBy_MULTIPLE_CONSTRAINT_(/*_CONSTRAINT_PARAMS_TYPED_*/, string includes = null, bool disableAccessRightCheck = false)
    {
        logger?.LogDebug("GetSingleBy_MULTIPLE_CONSTRAINT_");
        using var unitOfWork = executionContext.StartUnitOfWork("Template", "CLASSNAMERepository GetSingleBy_MULTIPLE_CONSTRAINT_", UnitOfWorkType.Read);
        var entity = await GetSingleWhere(/*_CONSTRAINT_EXPRESSION_*/, includes);
        entity?.SetId(entity.Id);
        unitOfWork.Complete();
        return entity;
    }
    #endregion foreach MULTIPLE_UNIQUE_QUERYABLE_PROPERTY

    public async Task<Domain._DOMAINTYPE_.CLASSNAME> GetSingleWhere(Expression<Func<Domain._DOMAINTYPE_.CLASSNAME, bool>> expression, string includes = null, bool disableAccessRightCheck = false)
    {
        var queryable = Queryable;
        if (expression != null)
            queryable = queryable.Where(expression);
        if (!disableAccessRightCheck)
        {
            queryable = await AddAccessRightsCheckInQuery(queryable);
        }
        queryable = AddIncludeFilterData(includes, queryable);
        var entity = await GetSingleOfQueryable(queryable, LazyLoadingEnabled || includes == "Lazy");
        entity?.SetId(entity.Id);
        return entity;
    }

    public async Task<IEnumerable<Domain._DOMAINTYPE_.CLASSNAME>> GetListWhere(Expression<Func<Domain._DOMAINTYPE_.CLASSNAME, bool>> expression, string includes = null, bool disableAccessRightCheck = false)
    {
        var queryable = Queryable;
        if (expression != null)
            queryable = queryable.Where(expression);
        if (!disableAccessRightCheck)
        {
            queryable = await AddAccessRightsCheckInQuery(queryable);
        }
        queryable = AddIncludeFilterData(includes, queryable);
        var result = await InternalGetListOfQueryable(queryable, LazyLoadingEnabled || includes == "Lazy");
        foreach (var item in result)
        {
            item.SetId(item.Id);
        }
        return result;
    }

    public async Task<IEnumerable<Domain._DOMAINTYPE_.CLASSNAME>> GetListOfQueryable(IQueryable<Domain._DOMAINTYPE_.CLASSNAME> queryable = null, string includes = null, bool disableAccessRightCheck = false)
    {
        if (queryable == null)
        {
            queryable = Queryable;
        }
        if (!disableAccessRightCheck)
        {
            queryable = await AddAccessRightsCheckInQuery(queryable);
        }
        queryable = AddIncludeFilterData(includes, queryable);
        var result = await InternalGetListOfQueryable(queryable, LazyLoadingEnabled || includes == "Lazy");
        foreach (var item in result)
        {
            item.SetId(item.Id);
        }
        return result;
    }

    public async Task<IEnumerable<Domain._DOMAINTYPE_.CLASSNAME>> GetList(CLASSNAMEQueryFilter filter = null, string includes = null, bool disableAccessRightCheck = false)
    {
        if (filter == null)
        {
            filter = new CLASSNAMEQueryFilter() { };
        }

        logger?.LogDebug("GetList");
        using var unitOfWork = executionContext.StartUnitOfWork("Template", "CLASSNAMERepository GetList", UnitOfWorkType.Read);
        var expression = CreateExpressionFromQueryFilter(filter);
        var queryable = Queryable;
        if (expression != null)
            queryable = queryable.Where(expression);
        if (filter.OrderBy != null && filter.OrderBy.Count > 0)
        {
            if (filter.OrderBy[0].Way == OrderByWay.Ascending)
            {
                queryable = QueryHelper.OrderByProperty(queryable, filter.OrderBy[0].Member);
            }
            else
            {
                queryable = QueryHelper.OrderByPropertyDescending(queryable, filter.OrderBy[0].Member);
            }

            for (var i = 1; i < filter.OrderBy.Count; i++)
            {
                if (filter.OrderBy[1].Way == OrderByWay.Ascending)
                {
                    queryable = QueryHelper.ThenByProperty(queryable, filter.OrderBy[1].Member);
                }
                else
                {
                    queryable = QueryHelper.ThenByPropertyDescending(queryable, filter.OrderBy[1].Member);
                }
            }
        }
        if (filter.ContinuationId != null)
        {
            int skip = Pagination.GetSkip(filter.ContinuationId);
            queryable = queryable.Skip(skip);
        }
        if (filter.MaxResults.HasValue)
            queryable = queryable.Take(filter.MaxResults.Value);
        if (!disableAccessRightCheck)
        {
            queryable = await AddAccessRightsCheckInQuery(queryable);
        }
        queryable = AddIncludeFilterData(includes, queryable);
        // perform request
        var result = await InternalGetListOfQueryable(queryable, LazyLoadingEnabled || includes == "Lazy");
        foreach (var item in result)
        {
            item.SetId(item.Id);
        }

        if (filter.MaxResults.HasValue)
        {
            filter.ContinuationId = null;
            if (result.Any())
            {
                int skip = Pagination.GetSkip(filter.ContinuationId);
                filter.ContinuationId = Pagination.CreateContinuationId(skip + filter.MaxResults.Value, result.Last().Id);
            }
        }
        unitOfWork.Complete();
        return result as IEnumerable<Domain._DOMAINTYPE_.CLASSNAME>;

    }

    public async Task<IEnumerable<Domain._DOMAINTYPE_.CLASSNAME>> GetList()
    {
        return await GetList(null);
    }


    public async Task<Domain._DOMAINTYPE_.CLASSNAME> GetSingle(CLASSNAMEQueryFilter filter, string includes = null, bool disableAccessRightCheck = false)
    {
        logger?.LogDebug("GetUniqueByQuery");
        using var unitOfWork = executionContext.StartUnitOfWork("Template", "CLASSNAMERepository GetUniqueByQuery", UnitOfWorkType.Read);
        var expression = CreateExpressionFromQueryFilter(filter);
        var queryable = Queryable;
        if (expression != null)
            queryable = queryable.Where(expression);
        if (!disableAccessRightCheck)
        {
            queryable = await AddAccessRightsCheckInQuery(queryable);
        }
        var entity = await GetSingleOfQueryable(queryable, LazyLoadingEnabled || includes == "Lazy");
        entity?.SetId(entity.Id);
        unitOfWork.Complete();
        return entity;
    }

    public virtual IQueryable<Domain._DOMAINTYPE_.CLASSNAME> AddIncludeFilterData(string includes, IQueryable<Domain._DOMAINTYPE_.CLASSNAME> queryable)
    {
        throw new NotImplementedException();
    }

    #endregion

    #region Commands
    public virtual async Task<IOpsResult<_IDENTITY_KEY_TYPE_>> Add(Domain._DOMAINTYPE_.CLASSNAME entity, _IDENTITY_KEY_TYPE_? forcedId = null)
    {
        logger?.LogDebug("Add");
        using var unitOfWork = executionContext.StartUnitOfWork("Template", "CLASSNAMERepository Add", UnitOfWorkType.Write);

        IOpsResult result;
        result = await EnsureAccessRights(entity, true);
        if (result.HasError) return result.ToResult<_IDENTITY_KEY_TYPE_>();

        foreach (var rule in entityRules)
        {
            result = await rule.OnBeforeAdd(entity, unitOfWork);
            if (result.HasError) return result.ToResult<_IDENTITY_KEY_TYPE_>();
        }

        var id = await Create(entity, this.executionContext.UserId, forcedId);
        entity.SetId(id);

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
        logger?.LogDebug("Update");
        using var unitOfWork = executionContext.StartUnitOfWork("Template", "CLASSNAMERepository Update", UnitOfWorkType.Write);

        IOpsResult result;
        result = await EnsureAccessRights(entity, false);
        if (result.HasError) return result;

        foreach (var rule in entityRules)
        {
            result = await rule.OnBeforeUpdate(entity, unitOfWork);
            if (result.HasError) return result;
        }

        await Update(entity, this.executionContext.UserId);

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
        return await RemoveWithId(entity.ComposedId);
    }

    public async virtual Task<IOpsResult> RemoveWithId(string id)
    {
        logger?.LogDebug("RemoveWithId");
        using var unitOfWork = executionContext.StartUnitOfWork("Template", "CLASSNAMERepository RemoveWithId", UnitOfWorkType.Write);
        var entity = await GetSingleByComposedId(id);
        if (entity == null)
        {
            return IOpsResult.Invalid($"could not find CLASSNAME with Id {id}");
        }

        IOpsResult result;
        result = await EnsureAccessRights(entity, false);
        if (result.HasError) return result;

        foreach (var rule in entityRules)
        {
            result = await rule.OnBeforeRemove(entity, unitOfWork);
            if (result.HasError) return result;
        }

        result = await CascadeDeleteAggregateData(entity);
        if (result.HasError) return result;
        await Delete(entity);

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

    private void BindFromDictionary(Domain._DOMAINTYPE_.CLASSNAME entity, Dictionary<string, string> dictionary)
    {
        logger?.LogDebug("BindFromDictionary");
        if (dictionary == null)
            throw new ArgumentNullException(nameof(dictionary));

        #region foreach PROPERTY[S][NO]
        if (dictionary.ContainsKey("_SIMPLE__PROPERTYNAME_"))
            entity._SIMPLE__PROPERTYNAME_ = (_SIMPLE__TYPE_)DisplayConverter.ConvertTo/*_PROPERTYTYPE_*/(dictionary["_SIMPLE__PROPERTYNAME_"]);
        #endregion foreach PROPERTY

        #region foreach PROPERTY[E][NO]
        if (dictionary.ContainsKey("_ENUM__PROPERTYNAME_") && !string.IsNullOrEmpty(dictionary["_ENUM__PROPERTYNAME_"]))
            entity._ENUM__PROPERTYNAME_ = DisplayConverter.GetEnumData/*_ISLIST_*/<DEPENDENCYNAMESPACE.Contracts.Enums._ENUMNULLABLETYPE_, Contracts.Enums._ENUMTYPE_Enum>(dictionary["_ENUM__PROPERTYNAME_"]);
        #endregion foreach PROPERTY

        #region foreach PROPERTY[M][R][NO][EN][AG]
        if (dictionary.ContainsKey("_PROPERTYNAME_Id"))
            entity._PROPERTYNAME_Id = DisplayConverter.ConvertToIdentity<_IDENTITY_KEY_TYPE_>(dictionary["_PROPERTYNAME_Id"]);
        #endregion foreach PROPERTY

        #region foreach PROPERTY[M][R][NO][VO][NAR]
        if (dictionary.ContainsKey("_PROPERTYNAME_"))
            entity._PROPERTYNAME_ = Serializer.Deserialize<Domain.ValueObjects._PROPERTYTYPE_>((string)dictionary["_PROPERTYNAME_"]);
        #endregion foreach PROPERTY

        #region foreach PROPERTY[M][R][NO][VO][AR]
        if (dictionary.ContainsKey("_PROPERTYNAME_"))
            entity._PROPERTYNAME_ = Serializer.Deserialize<List<Domain.ValueObjects._PROPERTYTYPE_>>((string)dictionary["_PROPERTYNAME_"]);
        #endregion foreach PROPERTY

        #region foreach PROPERTY[M][R][LNA][VO]
        // list
        if (dictionary.ContainsKey("_PROPERTYNAME_"))
            entity._PROPERTYNAME_ = Serializer.Deserialize<List<Domain.ValueObjects._PROPERTYTYPE_>>((string)dictionary["_PROPERTYNAME_"])/*.AsReadOnly()*/;
        #endregion foreach PROPERTY
    }

    #region Access Rights
    protected async Task<IQueryable<Domain._DOMAINTYPE_.CLASSNAME>> AddAccessRightsCheckInQuery(IQueryable<Domain._DOMAINTYPE_.CLASSNAME> queryable)
    {
        var result = HasOwnershipOverrideRight();
        if (result.HasError || !result.Data)
        {
            return await AddOwnershipCheck(queryable);
        }
        return queryable;
    }

    protected async Task<IOpsResult> EnsureAccessRights(Domain._DOMAINTYPE_.CLASSNAME entity, bool isDuringAdd)
    {
        var result = HasOwnershipOverrideRight();
        if (result.HasError || !result.Data)
        {
            if (!(await HasOwnership(entity, isDuringAdd)))
            {
                if (isDuringAdd)
                    return IOpsResult.Forbidden($"Current user cannot create a CLASSNAME");
                return IOpsResult.Forbidden($"Current user cannot update CLASSNAME with Id {entity.Id}");
            }
            return result;
        }
        return IOpsResult.Ok();
    }

    private IOpsResult<bool> HasOwnershipOverrideRight()
    {
        if (executionContext.HasRight(IExecutionContext.ALLRIGHT))
            return IOpsResult.Ok(true);

        var result = EnsureMandatoryRights();
        if (result.HasError) return result.ToResult<bool>();

        foreach (var ownershipOverrideRight in executionContext.MandatoryRights.Where(r => !string.IsNullOrEmpty(r)))
        {
            if (executionContext.HasRight(ownershipOverrideRight))
            {
                return IOpsResult.Ok(true);
            }
        }
        return IOpsResult.Ok(false);
    }

    private IOpsResult EnsureMandatoryRights()
    {
        foreach (var mandatoryRight in executionContext.MandatoryRights.Where(r => !string.IsNullOrEmpty(r)))
        {
            if (!executionContext.HasRight(mandatoryRight))
            {
                return IOpsResult.Forbidden($"Missing mandatory right {mandatoryRight}");
            }
        }
        return IOpsResult.Ok();
    }

    protected virtual async Task<IQueryable<Domain._DOMAINTYPE_.CLASSNAME>> AddOwnershipCheck(IQueryable<Domain._DOMAINTYPE_.CLASSNAME> query)
    {
        await Task.CompletedTask;
        return query;
    }

    protected virtual async Task<bool> HasOwnership(Domain._DOMAINTYPE_.CLASSNAME entity, bool isDuringAdd)
    {
        await Task.CompletedTask;
        return true;
    }
    #endregion

    #region FromDbSet
    public virtual Task<Domain._DOMAINTYPE_.CLASSNAME> GetSingleOfQueryable(IQueryable<Domain._DOMAINTYPE_.CLASSNAME> queryable, bool enableLazyLoading = false)
    {
        throw new NotImplementedException();
    }

    internal virtual async Task<IEnumerable<Domain._DOMAINTYPE_.CLASSNAME>> InternalGetListOfQueryable(IQueryable<Domain._DOMAINTYPE_.CLASSNAME> queryable, bool enableLazyLoading = false)
    {
        throw new NotImplementedException();
    }

    public Expression<Func<Domain._DOMAINTYPE_.CLASSNAME, bool>> CreateExpressionFromQueryFilter(IQueryFilter filter)
    {
        if (string.IsNullOrEmpty(filter.LiteralQuery))
            return null;

        QueryElementFilteringTreeVisitor<_IDENTITY_KEY_TYPE_, Domain._DOMAINTYPE_.CLASSNAME> visitor = new QueryElementFilteringTreeVisitor<_IDENTITY_KEY_TYPE_, Domain._DOMAINTYPE_.CLASSNAME>();
        var allQuery = new QueryElementParser().Parse(filter.LiteralQuery);
        var exp = visitor.Visit(allQuery);
        return Expression.Lambda<Func<Domain._DOMAINTYPE_.CLASSNAME, bool>>(exp, visitor.ParameterExpression);
    }

    public async Task<_IDENTITY_KEY_TYPE_> Create(Domain._DOMAINTYPE_.CLASSNAME entity, string userId, _IDENTITY_KEY_TYPE_? forceId)
    {
        var efEntity = await Add(entity, userId, forceId);
        return efEntity.Id;
    }

    public virtual Task<Domain._DOMAINTYPE_.CLASSNAME> Add(Domain._DOMAINTYPE_.CLASSNAME entity, string userId, _IDENTITY_KEY_TYPE_? forceId)
    {
        throw new NotImplementedException();
    }

    public virtual Task<Domain._DOMAINTYPE_.CLASSNAME> Update(Domain._DOMAINTYPE_.CLASSNAME entity, string userId)
    {
        throw new NotImplementedException();
    }

    public virtual Task Delete(Domain._DOMAINTYPE_.CLASSNAME entity)
    {
        throw new NotImplementedException();
    }
    #endregion
}
#endregion foreach MODEL