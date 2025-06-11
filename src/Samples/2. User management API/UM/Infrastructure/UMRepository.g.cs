using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SolidOps.UM.Shared.Contracts.DTO;
using SolidOps.UM.Shared.Contracts.Results;
using SolidOps.UM.Shared.Domain.Configuration;
using SolidOps.UM.Shared.Domain.Entities;
using SolidOps.UM.Shared.Domain.UnitOfWork;
using SolidOps.UM.Shared.Infrastructure;
using SolidOps.UM.Shared.Infrastructure.Queries;
using SolidOps.UM.Domain.Queries;
using SolidOps.UM.Domain.Repositories;
using System.Linq.Expressions;
using SolidOps.SubZero;
namespace SolidOps.UM.Infrastructure.Repositories;
// Object [EN][AG]
public partial class LocalUserRepository : BaseLocalUserRepository, ILocalUserRepository
{
    public LocalUserRepository(IExecutionContext executionContext, ILoggerFactory loggerFactory, IServiceProvider serviceProvider, IExtendedConfiguration configuration
        , IEnumerable<IEntityRules<Guid, Domain.AggregateRoots.LocalUser>> entityRules

        ) : base(executionContext, loggerFactory, serviceProvider, configuration, entityRules

            )
    {
    }
}
public abstract partial class BaseLocalUserRepository
{
    protected readonly IExecutionContext executionContext;
    protected readonly ILogger<ILocalUserRepository> logger;
    protected readonly IServiceProvider serviceProvider;
    protected readonly IEnumerable<IEntityRules<Guid, Domain.AggregateRoots.LocalUser>> entityRules;
    protected readonly IExtendedConfiguration configuration;
    public DatabaseInfo DatabaseInfo { get; set; }

    public BaseLocalUserRepository(IExecutionContext executionContext, ILoggerFactory loggerFactory, IServiceProvider serviceProvider, IExtendedConfiguration configuration
        , IEnumerable<IEntityRules<Guid, Domain.AggregateRoots.LocalUser>> entityRules

        )
    {
        this.executionContext = executionContext;
        this.logger = loggerFactory.CreateLogger<ILocalUserRepository>();
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
    public Domain.AggregateRoots.LocalUser MapFromDomain(Domain.AggregateRoots.LocalUser domain, bool isNew)
    {
        if (domain == null)
            return null;
        Domain.AggregateRoots.LocalUser efEntity;
        if (isNew)
        {
            efEntity = Domain.AggregateRoots.LocalUser.CreateEmpty();
        }
        else
        {
            efEntity = Queryable.Where(e => e.Id.Equals(domain.Id)).SingleOrDefault();
            if (efEntity == null || domain == null)
                return null;
        }
        if (!Equals(efEntity.Id, domain.Id))
            efEntity.SetId(domain.Id);
        // Property [S][NO]
        if (!Equals(efEntity.Name, domain.Name))
            efEntity.Name = domain.Name;

        if (!Equals(efEntity.HashedPassword, domain.HashedPassword))
            efEntity.HashedPassword = domain.HashedPassword;

        return efEntity;
    }
    public Domain.AggregateRoots.LocalUser MapToDomain(Domain.AggregateRoots.LocalUser efEntity, Dictionary<Type, List<object>> mappingCache, bool enableLazyLoading = false)
    {
        if (efEntity == null)
            return null;
        efEntity.LazyLoadingEnabled = enableLazyLoading;
        if (!mappingCache.ContainsKey(typeof(Domain.AggregateRoots.LocalUser)))
        {
            mappingCache.Add(typeof(Domain.AggregateRoots.LocalUser), new List<object>());
        }
        Domain.AggregateRoots.LocalUser cachedEntity = mappingCache[typeof(Domain.AggregateRoots.LocalUser)].Cast<Domain.AggregateRoots.LocalUser>().SingleOrDefault(m => m.Id.Equals(efEntity.Id));
        if (cachedEntity == null)
        {
            mappingCache[typeof(Domain.AggregateRoots.LocalUser)].Add(efEntity);

            // navigation

        }
        return efEntity;
    }
    protected T GetRequiredService<T>()
    {
        return this.serviceProvider.GetRequiredService<T>();
    }
    public bool LazyLoadingEnabled { get; set; } = false;
    #region Queries
    public virtual IQueryable<Domain.AggregateRoots.LocalUser> Queryable
    {
        get
        {
            throw new NotImplementedException();
        }
    }
    public IQueryable<Domain.AggregateRoots.LocalUser> GetQueryable()
    {
        return Queryable;
    }
    public async Task<Domain.AggregateRoots.LocalUser> GetSingleById(Guid id, string includes = null, bool disableAccessRightCheck = false)
    {
        logger?.LogDebug("GetSingleById");
        using var unitOfWork = executionContext.StartUnitOfWork("UM", "LocalUserRepository GetSingleById", UnitOfWorkType.Read);
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
    public async Task<Domain.AggregateRoots.LocalUser> GetSingleByComposedId(string composedId, string includes = null, bool disableAccessRightCheck = false)
    {
        logger?.LogDebug("GetSingleByComposedId");
        using var unitOfWork = executionContext.StartUnitOfWork("UM", "LocalUserRepository GetSingleByComposedId", UnitOfWorkType.Read);
        var queryable = Queryable.Where(u => u.Id.Equals(IdentityKeyHelper<Guid>.ReadString(composedId)));
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

    // UniqueQueryableProperty 
    public async Task<Domain.AggregateRoots.LocalUser> GetSingleByName(String name, string includes = null, bool disableAccessRightCheck = false)
    {
        logger?.LogDebug("GetSingleByName");
        using var unitOfWork = executionContext.StartUnitOfWork("UM", "LocalUserRepository GetSingleByName", UnitOfWorkType.Read);
        var queryable = Queryable.Where(u => u.Name.Equals(name));
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

    public async Task<Domain.AggregateRoots.LocalUser> GetSingleWhere(Expression<Func<Domain.AggregateRoots.LocalUser, bool>> expression, string includes = null, bool disableAccessRightCheck = false)
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
    public async Task<IEnumerable<Domain.AggregateRoots.LocalUser>> GetListWhere(Expression<Func<Domain.AggregateRoots.LocalUser, bool>> expression, string includes = null, bool disableAccessRightCheck = false)
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
    public async Task<IEnumerable<Domain.AggregateRoots.LocalUser>> GetListOfQueryable(IQueryable<Domain.AggregateRoots.LocalUser> queryable = null, string includes = null, bool disableAccessRightCheck = false)
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
    public async Task<IEnumerable<Domain.AggregateRoots.LocalUser>> GetList(LocalUserQueryFilter filter = null, string includes = null, bool disableAccessRightCheck = false)
    {
        if (filter == null)
        {
            filter = new LocalUserQueryFilter() { };
        }
        logger?.LogDebug("GetList");
        using var unitOfWork = executionContext.StartUnitOfWork("UM", "LocalUserRepository GetList", UnitOfWorkType.Read);
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
        return result as IEnumerable<Domain.AggregateRoots.LocalUser>;
    }
    public async Task<IEnumerable<Domain.AggregateRoots.LocalUser>> GetList()
    {
        return await GetList(null);
    }
    public async Task<Domain.AggregateRoots.LocalUser> GetSingle(LocalUserQueryFilter filter, string includes = null, bool disableAccessRightCheck = false)
    {
        logger?.LogDebug("GetUniqueByQuery");
        using var unitOfWork = executionContext.StartUnitOfWork("UM", "LocalUserRepository GetUniqueByQuery", UnitOfWorkType.Read);
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
    public virtual IQueryable<Domain.AggregateRoots.LocalUser> AddIncludeFilterData(string includes, IQueryable<Domain.AggregateRoots.LocalUser> queryable)
    {
        throw new NotImplementedException();
    }
    #endregion
    #region Commands
    public virtual async Task<IOpsResult<Guid>> Add(Domain.AggregateRoots.LocalUser entity, Guid? forcedId = null)
    {
        logger?.LogDebug("Add");
        using var unitOfWork = executionContext.StartUnitOfWork("UM", "LocalUserRepository Add", UnitOfWorkType.Write);
        IOpsResult result;
        result = await EnsureAccessRights(entity, true);
        if (result.HasError) return result.ToResult<Guid>();
        foreach (var rule in entityRules)
        {
            result = await rule.OnBeforeAdd(entity, unitOfWork);
            if (result.HasError) return result.ToResult<Guid>();
        }
        var id = await Create(entity, this.executionContext.UserId, forcedId);
        entity.SetId(id);
        foreach (var rule in entityRules)
        {
            result = await rule.OnAfterAdd(id, entity, unitOfWork);
            if (result.HasError) return result.ToResult<Guid>();
        }
        unitOfWork.Complete();
        return IOpsResult.Ok<Guid>(entity.Id);
    }
    public virtual async Task<IOpsResult> Update(Domain.AggregateRoots.LocalUser entity)
    {
        logger?.LogDebug("Update");
        using var unitOfWork = executionContext.StartUnitOfWork("UM", "LocalUserRepository Update", UnitOfWorkType.Write);
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
    public async Task<IOpsResult> Remove(Domain.AggregateRoots.LocalUser entity)
    {
        return await RemoveWithId(entity.ComposedId);
    }
    public async virtual Task<IOpsResult> RemoveWithId(string id)
    {
        logger?.LogDebug("RemoveWithId");
        using var unitOfWork = executionContext.StartUnitOfWork("UM", "LocalUserRepository RemoveWithId", UnitOfWorkType.Write);
        var entity = await GetSingleByComposedId(id);
        if (entity == null)
        {
            return IOpsResult.Invalid($"could not find LocalUser with Id {id}");
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
    protected async Task<IOpsResult> CascadeDeleteAggregateData(Domain.AggregateRoots.LocalUser entity)
    {
        await Task.CompletedTask;

        return IOpsResult.Ok();
    }
    #endregion
    private void BindFromDictionary(Domain.AggregateRoots.LocalUser entity, Dictionary<string, string> dictionary)
    {
        logger?.LogDebug("BindFromDictionary");
        if (dictionary == null)
            throw new ArgumentNullException(nameof(dictionary));
        // Property [S][NO]
        if (dictionary.ContainsKey("Name"))
            entity.Name = (System.String)DisplayConverter.ConvertToString(dictionary["Name"]);

        if (dictionary.ContainsKey("HashedPassword"))
            entity.HashedPassword = (System.String)DisplayConverter.ConvertToString(dictionary["HashedPassword"]);

    }
    #region Access Rights
    protected async Task<IQueryable<Domain.AggregateRoots.LocalUser>> AddAccessRightsCheckInQuery(IQueryable<Domain.AggregateRoots.LocalUser> queryable)
    {
        var result = HasOwnershipOverrideRight();
        if (result.HasError || !result.Data)
        {
            return await AddOwnershipCheck(queryable);
        }
        return queryable;
    }
    protected async Task<IOpsResult> EnsureAccessRights(Domain.AggregateRoots.LocalUser entity, bool isDuringAdd)
    {
        var result = HasOwnershipOverrideRight();
        if (result.HasError || !result.Data)
        {
            if (!(await HasOwnership(entity, isDuringAdd)))
            {
                if (isDuringAdd)
                    return IOpsResult.Forbidden($"Current user cannot create a LocalUser");
                return IOpsResult.Forbidden($"Current user cannot update LocalUser with Id {entity.Id}");
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
    protected virtual async Task<IQueryable<Domain.AggregateRoots.LocalUser>> AddOwnershipCheck(IQueryable<Domain.AggregateRoots.LocalUser> query)
    {
        await Task.CompletedTask;
        return query;
    }
    protected virtual async Task<bool> HasOwnership(Domain.AggregateRoots.LocalUser entity, bool isDuringAdd)
    {
        await Task.CompletedTask;
        return true;
    }
    #endregion
    #region FromDbSet
    public virtual Task<Domain.AggregateRoots.LocalUser> GetSingleOfQueryable(IQueryable<Domain.AggregateRoots.LocalUser> queryable, bool enableLazyLoading = false)
    {
        throw new NotImplementedException();
    }
    internal virtual async Task<IEnumerable<Domain.AggregateRoots.LocalUser>> InternalGetListOfQueryable(IQueryable<Domain.AggregateRoots.LocalUser> queryable, bool enableLazyLoading = false)
    {
        throw new NotImplementedException();
    }
    public Expression<Func<Domain.AggregateRoots.LocalUser, bool>> CreateExpressionFromQueryFilter(IQueryFilter filter)
    {
        if (string.IsNullOrEmpty(filter.LiteralQuery))
            return null;
        QueryElementFilteringTreeVisitor<Guid, Domain.AggregateRoots.LocalUser> visitor = new QueryElementFilteringTreeVisitor<Guid, Domain.AggregateRoots.LocalUser>();
        var allQuery = new QueryElementParser().Parse(filter.LiteralQuery);
        var exp = visitor.Visit(allQuery);
        return Expression.Lambda<Func<Domain.AggregateRoots.LocalUser, bool>>(exp, visitor.ParameterExpression);
    }
    public async Task<Guid> Create(Domain.AggregateRoots.LocalUser entity, string userId, Guid? forceId)
    {
        var efEntity = await Add(entity, userId, forceId);
        return efEntity.Id;
    }
    public virtual Task<Domain.AggregateRoots.LocalUser> Add(Domain.AggregateRoots.LocalUser entity, string userId, Guid? forceId)
    {
        throw new NotImplementedException();
    }
    public virtual Task<Domain.AggregateRoots.LocalUser> Update(Domain.AggregateRoots.LocalUser entity, string userId)
    {
        throw new NotImplementedException();
    }
    public virtual Task Delete(Domain.AggregateRoots.LocalUser entity)
    {
        throw new NotImplementedException();
    }
    #endregion
}
public partial class UserRepository : BaseUserRepository, IUserRepository
{
    public UserRepository(IExecutionContext executionContext, ILoggerFactory loggerFactory, IServiceProvider serviceProvider, IExtendedConfiguration configuration
        , IEnumerable<IEntityRules<Guid, Domain.AggregateRoots.User>> entityRules
    // Dependency [EN][AG]
        , SolidOps.UM.Domain.Repositories.ILocalUserRepository dependencyLocalUserRepository

        ) : base(executionContext, loggerFactory, serviceProvider, configuration, entityRules
    // Dependency [EN][AG]
        , dependencyLocalUserRepository

            )
    {
    }
}
public abstract partial class BaseUserRepository
{
    protected readonly IExecutionContext executionContext;
    protected readonly ILogger<IUserRepository> logger;
    protected readonly IServiceProvider serviceProvider;
    protected readonly IEnumerable<IEntityRules<Guid, Domain.AggregateRoots.User>> entityRules;
    protected readonly IExtendedConfiguration configuration;
    public DatabaseInfo DatabaseInfo { get; set; }
    // Dependency [EN][AG]
    protected readonly SolidOps.UM.Domain.Repositories.ILocalUserRepository _dependencyLocalUserRepository;

    public BaseUserRepository(IExecutionContext executionContext, ILoggerFactory loggerFactory, IServiceProvider serviceProvider, IExtendedConfiguration configuration
        , IEnumerable<IEntityRules<Guid, Domain.AggregateRoots.User>> entityRules
    // Dependency [EN][AG]
        , SolidOps.UM.Domain.Repositories.ILocalUserRepository dependencyLocalUserRepository // for base

        )
    {
        this.executionContext = executionContext;
        this.logger = loggerFactory.CreateLogger<IUserRepository>();
        this.serviceProvider = serviceProvider;
        this.entityRules = entityRules.OrderBy(s => s.Priority).ToList();
        this.configuration = configuration;
        // Dependency [EN][AG]
        _dependencyLocalUserRepository = dependencyLocalUserRepository ?? throw new ArgumentNullException(nameof(dependencyLocalUserRepository));

    }
    protected bool IsNotDefaultId(Guid value)
    {
        return !IdentityKeyHelper<Guid>.IsDefault(value);
    }
    protected bool IsNotDefaultId(Guid? value)
    {
        return value.HasValue && !IdentityKeyHelper<Guid>.IsDefault(value.Value);
    }
    public Domain.AggregateRoots.User MapFromDomain(Domain.AggregateRoots.User domain, bool isNew)
    {
        if (domain == null)
            return null;
        Domain.AggregateRoots.User efEntity;
        if (isNew)
        {
            efEntity = Domain.AggregateRoots.User.CreateEmpty();
        }
        else
        {
            efEntity = Queryable.Where(e => e.Id.Equals(domain.Id)).SingleOrDefault();
            if (efEntity == null || domain == null)
                return null;
        }
        if (!Equals(efEntity.Id, domain.Id))
            efEntity.SetId(domain.Id);
        // Property [S][NO]
        if (!Equals(efEntity.Email, domain.Email))
            efEntity.Email = domain.Email;

        if (!Equals(efEntity.Provider, domain.Provider))
            efEntity.Provider = domain.Provider;

        if (!Equals(efEntity.TechnicalUser, domain.TechnicalUser))
            efEntity.TechnicalUser = domain.TechnicalUser;

        return efEntity;
    }
    public Domain.AggregateRoots.User MapToDomain(Domain.AggregateRoots.User efEntity, Dictionary<Type, List<object>> mappingCache, bool enableLazyLoading = false)
    {
        if (efEntity == null)
            return null;
        efEntity.LazyLoadingEnabled = enableLazyLoading;
        if (!mappingCache.ContainsKey(typeof(Domain.AggregateRoots.User)))
        {
            mappingCache.Add(typeof(Domain.AggregateRoots.User), new List<object>());
        }
        Domain.AggregateRoots.User cachedEntity = mappingCache[typeof(Domain.AggregateRoots.User)].Cast<Domain.AggregateRoots.User>().SingleOrDefault(m => m.Id.Equals(efEntity.Id));
        if (cachedEntity == null)
        {
            mappingCache[typeof(Domain.AggregateRoots.User)].Add(efEntity);

            // navigation

            // Property [M][LNA][EN][AG]
            if (efEntity.UserRights != null)
            {
                foreach (var item in efEntity.UserRights)
                {
                    if (item != null)
                    {
                        var subRepository = serviceProvider.GetRequiredService<Domain.Repositories.IUserRightRepository>();
                        subRepository.MapToDomain(item, mappingCache, enableLazyLoading);
                    }
                }
            }

        }
        return efEntity;
    }
    protected T GetRequiredService<T>()
    {
        return this.serviceProvider.GetRequiredService<T>();
    }
    public bool LazyLoadingEnabled { get; set; } = false;
    #region Queries
    public virtual IQueryable<Domain.AggregateRoots.User> Queryable
    {
        get
        {
            throw new NotImplementedException();
        }
    }
    public IQueryable<Domain.AggregateRoots.User> GetQueryable()
    {
        return Queryable;
    }
    public async Task<Domain.AggregateRoots.User> GetSingleById(Guid id, string includes = null, bool disableAccessRightCheck = false)
    {
        logger?.LogDebug("GetSingleById");
        using var unitOfWork = executionContext.StartUnitOfWork("UM", "UserRepository GetSingleById", UnitOfWorkType.Read);
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
    public async Task<Domain.AggregateRoots.User> GetSingleByComposedId(string composedId, string includes = null, bool disableAccessRightCheck = false)
    {
        logger?.LogDebug("GetSingleByComposedId");
        using var unitOfWork = executionContext.StartUnitOfWork("UM", "UserRepository GetSingleByComposedId", UnitOfWorkType.Read);
        var queryable = Queryable.Where(u => u.Id.Equals(IdentityKeyHelper<Guid>.ReadString(composedId)));
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

    // UniqueQueryableProperty 
    public async Task<Domain.AggregateRoots.User> GetSingleByEmail(String email, string includes = null, bool disableAccessRightCheck = false)
    {
        logger?.LogDebug("GetSingleByEmail");
        using var unitOfWork = executionContext.StartUnitOfWork("UM", "UserRepository GetSingleByEmail", UnitOfWorkType.Read);
        var queryable = Queryable.Where(u => u.Email.Equals(email));
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

    public async Task<Domain.AggregateRoots.User> GetSingleWhere(Expression<Func<Domain.AggregateRoots.User, bool>> expression, string includes = null, bool disableAccessRightCheck = false)
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
    public async Task<IEnumerable<Domain.AggregateRoots.User>> GetListWhere(Expression<Func<Domain.AggregateRoots.User, bool>> expression, string includes = null, bool disableAccessRightCheck = false)
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
    public async Task<IEnumerable<Domain.AggregateRoots.User>> GetListOfQueryable(IQueryable<Domain.AggregateRoots.User> queryable = null, string includes = null, bool disableAccessRightCheck = false)
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
    public async Task<IEnumerable<Domain.AggregateRoots.User>> GetList(UserQueryFilter filter = null, string includes = null, bool disableAccessRightCheck = false)
    {
        if (filter == null)
        {
            filter = new UserQueryFilter() { };
        }
        logger?.LogDebug("GetList");
        using var unitOfWork = executionContext.StartUnitOfWork("UM", "UserRepository GetList", UnitOfWorkType.Read);
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
        return result as IEnumerable<Domain.AggregateRoots.User>;
    }
    public async Task<IEnumerable<Domain.AggregateRoots.User>> GetList()
    {
        return await GetList(null);
    }
    public async Task<Domain.AggregateRoots.User> GetSingle(UserQueryFilter filter, string includes = null, bool disableAccessRightCheck = false)
    {
        logger?.LogDebug("GetUniqueByQuery");
        using var unitOfWork = executionContext.StartUnitOfWork("UM", "UserRepository GetUniqueByQuery", UnitOfWorkType.Read);
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
    public virtual IQueryable<Domain.AggregateRoots.User> AddIncludeFilterData(string includes, IQueryable<Domain.AggregateRoots.User> queryable)
    {
        throw new NotImplementedException();
    }
    #endregion
    #region Commands
    public virtual async Task<IOpsResult<Guid>> Add(Domain.AggregateRoots.User entity, Guid? forcedId = null)
    {
        logger?.LogDebug("Add");
        using var unitOfWork = executionContext.StartUnitOfWork("UM", "UserRepository Add", UnitOfWorkType.Write);
        IOpsResult result;
        result = await EnsureAccessRights(entity, true);
        if (result.HasError) return result.ToResult<Guid>();
        foreach (var rule in entityRules)
        {
            result = await rule.OnBeforeAdd(entity, unitOfWork);
            if (result.HasError) return result.ToResult<Guid>();
        }
        var id = await Create(entity, this.executionContext.UserId, forcedId);
        entity.SetId(id);
        foreach (var rule in entityRules)
        {
            result = await rule.OnAfterAdd(id, entity, unitOfWork);
            if (result.HasError) return result.ToResult<Guid>();
        }
        unitOfWork.Complete();
        return IOpsResult.Ok<Guid>(entity.Id);
    }
    public virtual async Task<IOpsResult> Update(Domain.AggregateRoots.User entity)
    {
        logger?.LogDebug("Update");
        using var unitOfWork = executionContext.StartUnitOfWork("UM", "UserRepository Update", UnitOfWorkType.Write);
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
    public async Task<IOpsResult> Remove(Domain.AggregateRoots.User entity)
    {
        return await RemoveWithId(entity.ComposedId);
    }
    public async virtual Task<IOpsResult> RemoveWithId(string id)
    {
        logger?.LogDebug("RemoveWithId");
        using var unitOfWork = executionContext.StartUnitOfWork("UM", "UserRepository RemoveWithId", UnitOfWorkType.Write);
        var entity = await GetSingleByComposedId(id);
        if (entity == null)
        {
            return IOpsResult.Invalid($"could not find User with Id {id}");
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
    protected async Task<IOpsResult> CascadeDeleteAggregateData(Domain.AggregateRoots.User entity)
    {
        await Task.CompletedTask;
        // DependentFor [EN][AG]
        {
            var repo = GetRequiredService<SolidOps.UM.Domain.Repositories.IUserRightRepository>();
            var relatedEntities = await repo.GetListWhere(e => e.UserId == entity.Id, null, false);
            foreach (var relatedEntity in relatedEntities)
            {
                var result = await repo.Remove(relatedEntity);
                if (result.HasError) return result;
            }
        }

        return IOpsResult.Ok();
    }
    #endregion
    private void BindFromDictionary(Domain.AggregateRoots.User entity, Dictionary<string, string> dictionary)
    {
        logger?.LogDebug("BindFromDictionary");
        if (dictionary == null)
            throw new ArgumentNullException(nameof(dictionary));
        // Property [S][NO]
        if (dictionary.ContainsKey("Email"))
            entity.Email = (System.String)DisplayConverter.ConvertToString(dictionary["Email"]);

        if (dictionary.ContainsKey("Provider"))
            entity.Provider = (System.String)DisplayConverter.ConvertToString(dictionary["Provider"]);

        if (dictionary.ContainsKey("TechnicalUser"))
            entity.TechnicalUser = (System.Boolean)DisplayConverter.ConvertToBoolean(dictionary["TechnicalUser"]);

    }
    #region Access Rights
    protected async Task<IQueryable<Domain.AggregateRoots.User>> AddAccessRightsCheckInQuery(IQueryable<Domain.AggregateRoots.User> queryable)
    {
        var result = HasOwnershipOverrideRight();
        if (result.HasError || !result.Data)
        {
            return await AddOwnershipCheck(queryable);
        }
        return queryable;
    }
    protected async Task<IOpsResult> EnsureAccessRights(Domain.AggregateRoots.User entity, bool isDuringAdd)
    {
        var result = HasOwnershipOverrideRight();
        if (result.HasError || !result.Data)
        {
            if (!(await HasOwnership(entity, isDuringAdd)))
            {
                if (isDuringAdd)
                    return IOpsResult.Forbidden($"Current user cannot create a User");
                return IOpsResult.Forbidden($"Current user cannot update User with Id {entity.Id}");
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
    protected virtual async Task<IQueryable<Domain.AggregateRoots.User>> AddOwnershipCheck(IQueryable<Domain.AggregateRoots.User> query)
    {
        await Task.CompletedTask;
        return query;
    }
    protected virtual async Task<bool> HasOwnership(Domain.AggregateRoots.User entity, bool isDuringAdd)
    {
        await Task.CompletedTask;
        return true;
    }
    #endregion
    #region FromDbSet
    public virtual Task<Domain.AggregateRoots.User> GetSingleOfQueryable(IQueryable<Domain.AggregateRoots.User> queryable, bool enableLazyLoading = false)
    {
        throw new NotImplementedException();
    }
    internal virtual async Task<IEnumerable<Domain.AggregateRoots.User>> InternalGetListOfQueryable(IQueryable<Domain.AggregateRoots.User> queryable, bool enableLazyLoading = false)
    {
        throw new NotImplementedException();
    }
    public Expression<Func<Domain.AggregateRoots.User, bool>> CreateExpressionFromQueryFilter(IQueryFilter filter)
    {
        if (string.IsNullOrEmpty(filter.LiteralQuery))
            return null;
        QueryElementFilteringTreeVisitor<Guid, Domain.AggregateRoots.User> visitor = new QueryElementFilteringTreeVisitor<Guid, Domain.AggregateRoots.User>();
        var allQuery = new QueryElementParser().Parse(filter.LiteralQuery);
        var exp = visitor.Visit(allQuery);
        return Expression.Lambda<Func<Domain.AggregateRoots.User, bool>>(exp, visitor.ParameterExpression);
    }
    public async Task<Guid> Create(Domain.AggregateRoots.User entity, string userId, Guid? forceId)
    {
        var efEntity = await Add(entity, userId, forceId);
        return efEntity.Id;
    }
    public virtual Task<Domain.AggregateRoots.User> Add(Domain.AggregateRoots.User entity, string userId, Guid? forceId)
    {
        throw new NotImplementedException();
    }
    public virtual Task<Domain.AggregateRoots.User> Update(Domain.AggregateRoots.User entity, string userId)
    {
        throw new NotImplementedException();
    }
    public virtual Task Delete(Domain.AggregateRoots.User entity)
    {
        throw new NotImplementedException();
    }
    #endregion
}
public partial class UserRightRepository : BaseUserRightRepository, IUserRightRepository
{
    public UserRightRepository(IExecutionContext executionContext, ILoggerFactory loggerFactory, IServiceProvider serviceProvider, IExtendedConfiguration configuration
        , IEnumerable<IEntityRules<Guid, Domain.Entities.UserRight>> entityRules

        ) : base(executionContext, loggerFactory, serviceProvider, configuration, entityRules

            )
    {
    }
}
public abstract partial class BaseUserRightRepository
{
    protected readonly IExecutionContext executionContext;
    protected readonly ILogger<IUserRightRepository> logger;
    protected readonly IServiceProvider serviceProvider;
    protected readonly IEnumerable<IEntityRules<Guid, Domain.Entities.UserRight>> entityRules;
    protected readonly IExtendedConfiguration configuration;
    public DatabaseInfo DatabaseInfo { get; set; }

    public BaseUserRightRepository(IExecutionContext executionContext, ILoggerFactory loggerFactory, IServiceProvider serviceProvider, IExtendedConfiguration configuration
        , IEnumerable<IEntityRules<Guid, Domain.Entities.UserRight>> entityRules

        )
    {
        this.executionContext = executionContext;
        this.logger = loggerFactory.CreateLogger<IUserRightRepository>();
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
    public Domain.Entities.UserRight MapFromDomain(Domain.Entities.UserRight domain, bool isNew)
    {
        if (domain == null)
            return null;
        Domain.Entities.UserRight efEntity;
        if (isNew)
        {
            efEntity = Domain.Entities.UserRight.CreateEmpty();
        }
        else
        {
            efEntity = Queryable.Where(e => e.Id.Equals(domain.Id)).SingleOrDefault();
            if (efEntity == null || domain == null)
                return null;
        }
        if (!Equals(efEntity.Id, domain.Id))
            efEntity.SetId(domain.Id);

        // Property [M][NO][EN][AG]
        if (!Equals(efEntity.UserId, domain.UserId))
        {
            if (IsNotDefaultId(domain.UserId))
            {
                efEntity.UserId = domain.UserId;
            }
            else
            {
                efEntity.UserId = default(Guid);
            }
        }

        if (!Equals(efEntity.RightId, domain.RightId))
        {
            if (IsNotDefaultId(domain.RightId))
            {
                efEntity.RightId = domain.RightId;
            }
            else
            {
                efEntity.RightId = default(Guid);
            }
        }

        return efEntity;
    }
    public Domain.Entities.UserRight MapToDomain(Domain.Entities.UserRight efEntity, Dictionary<Type, List<object>> mappingCache, bool enableLazyLoading = false)
    {
        if (efEntity == null)
            return null;
        efEntity.LazyLoadingEnabled = enableLazyLoading;
        if (!mappingCache.ContainsKey(typeof(Domain.Entities.UserRight)))
        {
            mappingCache.Add(typeof(Domain.Entities.UserRight), new List<object>());
        }
        Domain.Entities.UserRight cachedEntity = mappingCache[typeof(Domain.Entities.UserRight)].Cast<Domain.Entities.UserRight>().SingleOrDefault(m => m.Id.Equals(efEntity.Id));
        if (cachedEntity == null)
        {
            mappingCache[typeof(Domain.Entities.UserRight)].Add(efEntity);
            // Property [M][NO][EN][AG]
            if (efEntity.User != null)
            {
                var subRepository = serviceProvider.GetRequiredService<Domain.Repositories.IUserRepository>();
                efEntity.User = subRepository.MapToDomain(efEntity.User, mappingCache, enableLazyLoading);
            }

            if (efEntity.Right != null)
            {
                var subRepository = serviceProvider.GetRequiredService<Domain.Repositories.IRightRepository>();
                efEntity.Right = subRepository.MapToDomain(efEntity.Right, mappingCache, enableLazyLoading);
            }

            // navigation

        }
        return efEntity;
    }
    protected T GetRequiredService<T>()
    {
        return this.serviceProvider.GetRequiredService<T>();
    }
    public bool LazyLoadingEnabled { get; set; } = false;
    #region Queries
    public virtual IQueryable<Domain.Entities.UserRight> Queryable
    {
        get
        {
            throw new NotImplementedException();
        }
    }
    public IQueryable<Domain.Entities.UserRight> GetQueryable()
    {
        return Queryable;
    }
    public async Task<Domain.Entities.UserRight> GetSingleById(Guid id, string includes = null, bool disableAccessRightCheck = false)
    {
        logger?.LogDebug("GetSingleById");
        using var unitOfWork = executionContext.StartUnitOfWork("UM", "UserRightRepository GetSingleById", UnitOfWorkType.Read);
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
    public async Task<Domain.Entities.UserRight> GetSingleByComposedId(string composedId, string includes = null, bool disableAccessRightCheck = false)
    {
        logger?.LogDebug("GetSingleByComposedId");
        using var unitOfWork = executionContext.StartUnitOfWork("UM", "UserRightRepository GetSingleByComposedId", UnitOfWorkType.Read);
        var queryable = Queryable.Where(u => u.Id.Equals(IdentityKeyHelper<Guid>.ReadString(composedId)));
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

    public async Task<Domain.Entities.UserRight> GetSingleWhere(Expression<Func<Domain.Entities.UserRight, bool>> expression, string includes = null, bool disableAccessRightCheck = false)
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
    public async Task<IEnumerable<Domain.Entities.UserRight>> GetListWhere(Expression<Func<Domain.Entities.UserRight, bool>> expression, string includes = null, bool disableAccessRightCheck = false)
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
    public async Task<IEnumerable<Domain.Entities.UserRight>> GetListOfQueryable(IQueryable<Domain.Entities.UserRight> queryable = null, string includes = null, bool disableAccessRightCheck = false)
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
    public async Task<IEnumerable<Domain.Entities.UserRight>> GetList(UserRightQueryFilter filter = null, string includes = null, bool disableAccessRightCheck = false)
    {
        if (filter == null)
        {
            filter = new UserRightQueryFilter() { };
        }
        logger?.LogDebug("GetList");
        using var unitOfWork = executionContext.StartUnitOfWork("UM", "UserRightRepository GetList", UnitOfWorkType.Read);
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
        return result as IEnumerable<Domain.Entities.UserRight>;
    }
    public async Task<IEnumerable<Domain.Entities.UserRight>> GetList()
    {
        return await GetList(null);
    }
    public async Task<Domain.Entities.UserRight> GetSingle(UserRightQueryFilter filter, string includes = null, bool disableAccessRightCheck = false)
    {
        logger?.LogDebug("GetUniqueByQuery");
        using var unitOfWork = executionContext.StartUnitOfWork("UM", "UserRightRepository GetUniqueByQuery", UnitOfWorkType.Read);
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
    public virtual IQueryable<Domain.Entities.UserRight> AddIncludeFilterData(string includes, IQueryable<Domain.Entities.UserRight> queryable)
    {
        throw new NotImplementedException();
    }
    #endregion
    #region Commands
    public virtual async Task<IOpsResult<Guid>> Add(Domain.Entities.UserRight entity, Guid? forcedId = null)
    {
        logger?.LogDebug("Add");
        using var unitOfWork = executionContext.StartUnitOfWork("UM", "UserRightRepository Add", UnitOfWorkType.Write);
        IOpsResult result;
        result = await EnsureAccessRights(entity, true);
        if (result.HasError) return result.ToResult<Guid>();
        foreach (var rule in entityRules)
        {
            result = await rule.OnBeforeAdd(entity, unitOfWork);
            if (result.HasError) return result.ToResult<Guid>();
        }
        var id = await Create(entity, this.executionContext.UserId, forcedId);
        entity.SetId(id);
        foreach (var rule in entityRules)
        {
            result = await rule.OnAfterAdd(id, entity, unitOfWork);
            if (result.HasError) return result.ToResult<Guid>();
        }
        unitOfWork.Complete();
        return IOpsResult.Ok<Guid>(entity.Id);
    }
    public virtual async Task<IOpsResult> Update(Domain.Entities.UserRight entity)
    {
        logger?.LogDebug("Update");
        using var unitOfWork = executionContext.StartUnitOfWork("UM", "UserRightRepository Update", UnitOfWorkType.Write);
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
    public async Task<IOpsResult> Remove(Domain.Entities.UserRight entity)
    {
        return await RemoveWithId(entity.ComposedId);
    }
    public async virtual Task<IOpsResult> RemoveWithId(string id)
    {
        logger?.LogDebug("RemoveWithId");
        using var unitOfWork = executionContext.StartUnitOfWork("UM", "UserRightRepository RemoveWithId", UnitOfWorkType.Write);
        var entity = await GetSingleByComposedId(id);
        if (entity == null)
        {
            return IOpsResult.Invalid($"could not find UserRight with Id {id}");
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
    protected async Task<IOpsResult> CascadeDeleteAggregateData(Domain.Entities.UserRight entity)
    {
        await Task.CompletedTask;

        return IOpsResult.Ok();
    }
    #endregion
    private void BindFromDictionary(Domain.Entities.UserRight entity, Dictionary<string, string> dictionary)
    {
        logger?.LogDebug("BindFromDictionary");
        if (dictionary == null)
            throw new ArgumentNullException(nameof(dictionary));

        // Property [M][R][NO][EN][AG]
        if (dictionary.ContainsKey("UserId"))
            entity.UserId = DisplayConverter.ConvertToIdentity<Guid>(dictionary["UserId"]);

        if (dictionary.ContainsKey("RightId"))
            entity.RightId = DisplayConverter.ConvertToIdentity<Guid>(dictionary["RightId"]);

    }
    #region Access Rights
    protected async Task<IQueryable<Domain.Entities.UserRight>> AddAccessRightsCheckInQuery(IQueryable<Domain.Entities.UserRight> queryable)
    {
        var result = HasOwnershipOverrideRight();
        if (result.HasError || !result.Data)
        {
            return await AddOwnershipCheck(queryable);
        }
        return queryable;
    }
    protected async Task<IOpsResult> EnsureAccessRights(Domain.Entities.UserRight entity, bool isDuringAdd)
    {
        var result = HasOwnershipOverrideRight();
        if (result.HasError || !result.Data)
        {
            if (!(await HasOwnership(entity, isDuringAdd)))
            {
                if (isDuringAdd)
                    return IOpsResult.Forbidden($"Current user cannot create a UserRight");
                return IOpsResult.Forbidden($"Current user cannot update UserRight with Id {entity.Id}");
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
    protected virtual async Task<IQueryable<Domain.Entities.UserRight>> AddOwnershipCheck(IQueryable<Domain.Entities.UserRight> query)
    {
        await Task.CompletedTask;
        return query;
    }
    protected virtual async Task<bool> HasOwnership(Domain.Entities.UserRight entity, bool isDuringAdd)
    {
        await Task.CompletedTask;
        return true;
    }
    #endregion
    #region FromDbSet
    public virtual Task<Domain.Entities.UserRight> GetSingleOfQueryable(IQueryable<Domain.Entities.UserRight> queryable, bool enableLazyLoading = false)
    {
        throw new NotImplementedException();
    }
    internal virtual async Task<IEnumerable<Domain.Entities.UserRight>> InternalGetListOfQueryable(IQueryable<Domain.Entities.UserRight> queryable, bool enableLazyLoading = false)
    {
        throw new NotImplementedException();
    }
    public Expression<Func<Domain.Entities.UserRight, bool>> CreateExpressionFromQueryFilter(IQueryFilter filter)
    {
        if (string.IsNullOrEmpty(filter.LiteralQuery))
            return null;
        QueryElementFilteringTreeVisitor<Guid, Domain.Entities.UserRight> visitor = new QueryElementFilteringTreeVisitor<Guid, Domain.Entities.UserRight>();
        var allQuery = new QueryElementParser().Parse(filter.LiteralQuery);
        var exp = visitor.Visit(allQuery);
        return Expression.Lambda<Func<Domain.Entities.UserRight, bool>>(exp, visitor.ParameterExpression);
    }
    public async Task<Guid> Create(Domain.Entities.UserRight entity, string userId, Guid? forceId)
    {
        var efEntity = await Add(entity, userId, forceId);
        return efEntity.Id;
    }
    public virtual Task<Domain.Entities.UserRight> Add(Domain.Entities.UserRight entity, string userId, Guid? forceId)
    {
        throw new NotImplementedException();
    }
    public virtual Task<Domain.Entities.UserRight> Update(Domain.Entities.UserRight entity, string userId)
    {
        throw new NotImplementedException();
    }
    public virtual Task Delete(Domain.Entities.UserRight entity)
    {
        throw new NotImplementedException();
    }
    #endregion
}
public partial class RightRepository : BaseRightRepository, IRightRepository
{
    public RightRepository(IExecutionContext executionContext, ILoggerFactory loggerFactory, IServiceProvider serviceProvider, IExtendedConfiguration configuration
        , IEnumerable<IEntityRules<Guid, Domain.Entities.Right>> entityRules

        ) : base(executionContext, loggerFactory, serviceProvider, configuration, entityRules

            )
    {
    }
}
public abstract partial class BaseRightRepository
{
    protected readonly IExecutionContext executionContext;
    protected readonly ILogger<IRightRepository> logger;
    protected readonly IServiceProvider serviceProvider;
    protected readonly IEnumerable<IEntityRules<Guid, Domain.Entities.Right>> entityRules;
    protected readonly IExtendedConfiguration configuration;
    public DatabaseInfo DatabaseInfo { get; set; }

    public BaseRightRepository(IExecutionContext executionContext, ILoggerFactory loggerFactory, IServiceProvider serviceProvider, IExtendedConfiguration configuration
        , IEnumerable<IEntityRules<Guid, Domain.Entities.Right>> entityRules

        )
    {
        this.executionContext = executionContext;
        this.logger = loggerFactory.CreateLogger<IRightRepository>();
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
    public Domain.Entities.Right MapFromDomain(Domain.Entities.Right domain, bool isNew)
    {
        if (domain == null)
            return null;
        Domain.Entities.Right efEntity;
        if (isNew)
        {
            efEntity = Domain.Entities.Right.CreateEmpty();
        }
        else
        {
            efEntity = Queryable.Where(e => e.Id.Equals(domain.Id)).SingleOrDefault();
            if (efEntity == null || domain == null)
                return null;
        }
        if (!Equals(efEntity.Id, domain.Id))
            efEntity.SetId(domain.Id);
        // Property [S][NO]
        if (!Equals(efEntity.Name, domain.Name))
            efEntity.Name = domain.Name;

        return efEntity;
    }
    public Domain.Entities.Right MapToDomain(Domain.Entities.Right efEntity, Dictionary<Type, List<object>> mappingCache, bool enableLazyLoading = false)
    {
        if (efEntity == null)
            return null;
        efEntity.LazyLoadingEnabled = enableLazyLoading;
        if (!mappingCache.ContainsKey(typeof(Domain.Entities.Right)))
        {
            mappingCache.Add(typeof(Domain.Entities.Right), new List<object>());
        }
        Domain.Entities.Right cachedEntity = mappingCache[typeof(Domain.Entities.Right)].Cast<Domain.Entities.Right>().SingleOrDefault(m => m.Id.Equals(efEntity.Id));
        if (cachedEntity == null)
        {
            mappingCache[typeof(Domain.Entities.Right)].Add(efEntity);

            // navigation

        }
        return efEntity;
    }
    protected T GetRequiredService<T>()
    {
        return this.serviceProvider.GetRequiredService<T>();
    }
    public bool LazyLoadingEnabled { get; set; } = false;
    #region Queries
    public virtual IQueryable<Domain.Entities.Right> Queryable
    {
        get
        {
            throw new NotImplementedException();
        }
    }
    public IQueryable<Domain.Entities.Right> GetQueryable()
    {
        return Queryable;
    }
    public async Task<Domain.Entities.Right> GetSingleById(Guid id, string includes = null, bool disableAccessRightCheck = false)
    {
        logger?.LogDebug("GetSingleById");
        using var unitOfWork = executionContext.StartUnitOfWork("UM", "RightRepository GetSingleById", UnitOfWorkType.Read);
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
    public async Task<Domain.Entities.Right> GetSingleByComposedId(string composedId, string includes = null, bool disableAccessRightCheck = false)
    {
        logger?.LogDebug("GetSingleByComposedId");
        using var unitOfWork = executionContext.StartUnitOfWork("UM", "RightRepository GetSingleByComposedId", UnitOfWorkType.Read);
        var queryable = Queryable.Where(u => u.Id.Equals(IdentityKeyHelper<Guid>.ReadString(composedId)));
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

    // UniqueQueryableProperty 
    public async Task<Domain.Entities.Right> GetSingleByName(String name, string includes = null, bool disableAccessRightCheck = false)
    {
        logger?.LogDebug("GetSingleByName");
        using var unitOfWork = executionContext.StartUnitOfWork("UM", "RightRepository GetSingleByName", UnitOfWorkType.Read);
        var queryable = Queryable.Where(u => u.Name.Equals(name));
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

    public async Task<Domain.Entities.Right> GetSingleWhere(Expression<Func<Domain.Entities.Right, bool>> expression, string includes = null, bool disableAccessRightCheck = false)
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
    public async Task<IEnumerable<Domain.Entities.Right>> GetListWhere(Expression<Func<Domain.Entities.Right, bool>> expression, string includes = null, bool disableAccessRightCheck = false)
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
    public async Task<IEnumerable<Domain.Entities.Right>> GetListOfQueryable(IQueryable<Domain.Entities.Right> queryable = null, string includes = null, bool disableAccessRightCheck = false)
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
    public async Task<IEnumerable<Domain.Entities.Right>> GetList(RightQueryFilter filter = null, string includes = null, bool disableAccessRightCheck = false)
    {
        if (filter == null)
        {
            filter = new RightQueryFilter() { };
        }
        logger?.LogDebug("GetList");
        using var unitOfWork = executionContext.StartUnitOfWork("UM", "RightRepository GetList", UnitOfWorkType.Read);
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
        return result as IEnumerable<Domain.Entities.Right>;
    }
    public async Task<IEnumerable<Domain.Entities.Right>> GetList()
    {
        return await GetList(null);
    }
    public async Task<Domain.Entities.Right> GetSingle(RightQueryFilter filter, string includes = null, bool disableAccessRightCheck = false)
    {
        logger?.LogDebug("GetUniqueByQuery");
        using var unitOfWork = executionContext.StartUnitOfWork("UM", "RightRepository GetUniqueByQuery", UnitOfWorkType.Read);
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
    public virtual IQueryable<Domain.Entities.Right> AddIncludeFilterData(string includes, IQueryable<Domain.Entities.Right> queryable)
    {
        throw new NotImplementedException();
    }
    #endregion
    #region Commands
    public virtual async Task<IOpsResult<Guid>> Add(Domain.Entities.Right entity, Guid? forcedId = null)
    {
        logger?.LogDebug("Add");
        using var unitOfWork = executionContext.StartUnitOfWork("UM", "RightRepository Add", UnitOfWorkType.Write);
        IOpsResult result;
        result = await EnsureAccessRights(entity, true);
        if (result.HasError) return result.ToResult<Guid>();
        foreach (var rule in entityRules)
        {
            result = await rule.OnBeforeAdd(entity, unitOfWork);
            if (result.HasError) return result.ToResult<Guid>();
        }
        var id = await Create(entity, this.executionContext.UserId, forcedId);
        entity.SetId(id);
        foreach (var rule in entityRules)
        {
            result = await rule.OnAfterAdd(id, entity, unitOfWork);
            if (result.HasError) return result.ToResult<Guid>();
        }
        unitOfWork.Complete();
        return IOpsResult.Ok<Guid>(entity.Id);
    }
    public virtual async Task<IOpsResult> Update(Domain.Entities.Right entity)
    {
        logger?.LogDebug("Update");
        using var unitOfWork = executionContext.StartUnitOfWork("UM", "RightRepository Update", UnitOfWorkType.Write);
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
    public async Task<IOpsResult> Remove(Domain.Entities.Right entity)
    {
        return await RemoveWithId(entity.ComposedId);
    }
    public async virtual Task<IOpsResult> RemoveWithId(string id)
    {
        logger?.LogDebug("RemoveWithId");
        using var unitOfWork = executionContext.StartUnitOfWork("UM", "RightRepository RemoveWithId", UnitOfWorkType.Write);
        var entity = await GetSingleByComposedId(id);
        if (entity == null)
        {
            return IOpsResult.Invalid($"could not find Right with Id {id}");
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
    protected async Task<IOpsResult> CascadeDeleteAggregateData(Domain.Entities.Right entity)
    {
        await Task.CompletedTask;

        return IOpsResult.Ok();
    }
    #endregion
    private void BindFromDictionary(Domain.Entities.Right entity, Dictionary<string, string> dictionary)
    {
        logger?.LogDebug("BindFromDictionary");
        if (dictionary == null)
            throw new ArgumentNullException(nameof(dictionary));
        // Property [S][NO]
        if (dictionary.ContainsKey("Name"))
            entity.Name = (System.String)DisplayConverter.ConvertToString(dictionary["Name"]);

    }
    #region Access Rights
    protected async Task<IQueryable<Domain.Entities.Right>> AddAccessRightsCheckInQuery(IQueryable<Domain.Entities.Right> queryable)
    {
        var result = HasOwnershipOverrideRight();
        if (result.HasError || !result.Data)
        {
            return await AddOwnershipCheck(queryable);
        }
        return queryable;
    }
    protected async Task<IOpsResult> EnsureAccessRights(Domain.Entities.Right entity, bool isDuringAdd)
    {
        var result = HasOwnershipOverrideRight();
        if (result.HasError || !result.Data)
        {
            if (!(await HasOwnership(entity, isDuringAdd)))
            {
                if (isDuringAdd)
                    return IOpsResult.Forbidden($"Current user cannot create a Right");
                return IOpsResult.Forbidden($"Current user cannot update Right with Id {entity.Id}");
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
    protected virtual async Task<IQueryable<Domain.Entities.Right>> AddOwnershipCheck(IQueryable<Domain.Entities.Right> query)
    {
        await Task.CompletedTask;
        return query;
    }
    protected virtual async Task<bool> HasOwnership(Domain.Entities.Right entity, bool isDuringAdd)
    {
        await Task.CompletedTask;
        return true;
    }
    #endregion
    #region FromDbSet
    public virtual Task<Domain.Entities.Right> GetSingleOfQueryable(IQueryable<Domain.Entities.Right> queryable, bool enableLazyLoading = false)
    {
        throw new NotImplementedException();
    }
    internal virtual async Task<IEnumerable<Domain.Entities.Right>> InternalGetListOfQueryable(IQueryable<Domain.Entities.Right> queryable, bool enableLazyLoading = false)
    {
        throw new NotImplementedException();
    }
    public Expression<Func<Domain.Entities.Right, bool>> CreateExpressionFromQueryFilter(IQueryFilter filter)
    {
        if (string.IsNullOrEmpty(filter.LiteralQuery))
            return null;
        QueryElementFilteringTreeVisitor<Guid, Domain.Entities.Right> visitor = new QueryElementFilteringTreeVisitor<Guid, Domain.Entities.Right>();
        var allQuery = new QueryElementParser().Parse(filter.LiteralQuery);
        var exp = visitor.Visit(allQuery);
        return Expression.Lambda<Func<Domain.Entities.Right, bool>>(exp, visitor.ParameterExpression);
    }
    public async Task<Guid> Create(Domain.Entities.Right entity, string userId, Guid? forceId)
    {
        var efEntity = await Add(entity, userId, forceId);
        return efEntity.Id;
    }
    public virtual Task<Domain.Entities.Right> Add(Domain.Entities.Right entity, string userId, Guid? forceId)
    {
        throw new NotImplementedException();
    }
    public virtual Task<Domain.Entities.Right> Update(Domain.Entities.Right entity, string userId)
    {
        throw new NotImplementedException();
    }
    public virtual Task Delete(Domain.Entities.Right entity)
    {
        throw new NotImplementedException();
    }
    #endregion
}
public partial class InviteRepository : BaseInviteRepository, IInviteRepository
{
    public InviteRepository(IExecutionContext executionContext, ILoggerFactory loggerFactory, IServiceProvider serviceProvider, IExtendedConfiguration configuration
        , IEnumerable<IEntityRules<Guid, Domain.AggregateRoots.Invite>> entityRules

        ) : base(executionContext, loggerFactory, serviceProvider, configuration, entityRules

            )
    {
    }
}
public abstract partial class BaseInviteRepository
{
    protected readonly IExecutionContext executionContext;
    protected readonly ILogger<IInviteRepository> logger;
    protected readonly IServiceProvider serviceProvider;
    protected readonly IEnumerable<IEntityRules<Guid, Domain.AggregateRoots.Invite>> entityRules;
    protected readonly IExtendedConfiguration configuration;
    public DatabaseInfo DatabaseInfo { get; set; }

    public BaseInviteRepository(IExecutionContext executionContext, ILoggerFactory loggerFactory, IServiceProvider serviceProvider, IExtendedConfiguration configuration
        , IEnumerable<IEntityRules<Guid, Domain.AggregateRoots.Invite>> entityRules

        )
    {
        this.executionContext = executionContext;
        this.logger = loggerFactory.CreateLogger<IInviteRepository>();
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
    public Domain.AggregateRoots.Invite MapFromDomain(Domain.AggregateRoots.Invite domain, bool isNew)
    {
        if (domain == null)
            return null;
        Domain.AggregateRoots.Invite efEntity;
        if (isNew)
        {
            efEntity = Domain.AggregateRoots.Invite.CreateEmpty();
        }
        else
        {
            efEntity = Queryable.Where(e => e.Id.Equals(domain.Id)).SingleOrDefault();
            if (efEntity == null || domain == null)
                return null;
        }
        if (!Equals(efEntity.Id, domain.Id))
            efEntity.SetId(domain.Id);
        // Property [S][NO]
        if (!Equals(efEntity.Email, domain.Email))
            efEntity.Email = domain.Email;

        if (!Equals(efEntity.CreatorName, domain.CreatorName))
            efEntity.CreatorName = domain.CreatorName;

        if (!Equals(efEntity.CreatorMessage, domain.CreatorMessage))
            efEntity.CreatorMessage = domain.CreatorMessage;

        // Property [E][NO]
        if (!Equals(efEntity.Status, domain.Status))
            efEntity.Status = domain.Status;

        // Property [M][NO][EN][AG]
        if (!Equals(efEntity.CreatorId, domain.CreatorId))
        {
            if (IsNotDefaultId(domain.CreatorId))
            {
                efEntity.CreatorId = domain.CreatorId;
            }
            else
            {
                efEntity.CreatorId = default(Guid);
            }
        }

        return efEntity;
    }
    public Domain.AggregateRoots.Invite MapToDomain(Domain.AggregateRoots.Invite efEntity, Dictionary<Type, List<object>> mappingCache, bool enableLazyLoading = false)
    {
        if (efEntity == null)
            return null;
        efEntity.LazyLoadingEnabled = enableLazyLoading;
        if (!mappingCache.ContainsKey(typeof(Domain.AggregateRoots.Invite)))
        {
            mappingCache.Add(typeof(Domain.AggregateRoots.Invite), new List<object>());
        }
        Domain.AggregateRoots.Invite cachedEntity = mappingCache[typeof(Domain.AggregateRoots.Invite)].Cast<Domain.AggregateRoots.Invite>().SingleOrDefault(m => m.Id.Equals(efEntity.Id));
        if (cachedEntity == null)
        {
            mappingCache[typeof(Domain.AggregateRoots.Invite)].Add(efEntity);
            // Property [M][NO][EN][AG]
            if (efEntity.Creator != null)
            {
                var subRepository = serviceProvider.GetRequiredService<Domain.Repositories.IUserRepository>();
                efEntity.Creator = subRepository.MapToDomain(efEntity.Creator, mappingCache, enableLazyLoading);
            }

            // navigation

        }
        return efEntity;
    }
    protected T GetRequiredService<T>()
    {
        return this.serviceProvider.GetRequiredService<T>();
    }
    public bool LazyLoadingEnabled { get; set; } = false;
    #region Queries
    public virtual IQueryable<Domain.AggregateRoots.Invite> Queryable
    {
        get
        {
            throw new NotImplementedException();
        }
    }
    public IQueryable<Domain.AggregateRoots.Invite> GetQueryable()
    {
        return Queryable;
    }
    public async Task<Domain.AggregateRoots.Invite> GetSingleById(Guid id, string includes = null, bool disableAccessRightCheck = false)
    {
        logger?.LogDebug("GetSingleById");
        using var unitOfWork = executionContext.StartUnitOfWork("UM", "InviteRepository GetSingleById", UnitOfWorkType.Read);
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
    public async Task<Domain.AggregateRoots.Invite> GetSingleByComposedId(string composedId, string includes = null, bool disableAccessRightCheck = false)
    {
        logger?.LogDebug("GetSingleByComposedId");
        using var unitOfWork = executionContext.StartUnitOfWork("UM", "InviteRepository GetSingleByComposedId", UnitOfWorkType.Read);
        var queryable = Queryable.Where(u => u.Id.Equals(IdentityKeyHelper<Guid>.ReadString(composedId)));
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
    // QueryableProperty 
    public async Task<IEnumerable<Domain.AggregateRoots.Invite>> GetListByEmail(string _email, string includes = null, bool disableAccessRightCheck = false)
    {
        logger?.LogDebug("GetListByEmail");
        using var unitOfWork = executionContext.StartUnitOfWork("UM", "InviteRepository GetListByEmail", UnitOfWorkType.Read);
        var queryable = Queryable.Where(u => u.Email.Equals(_email));
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
        return result as IEnumerable<Domain.AggregateRoots.Invite>;
    }

    public async Task<Domain.AggregateRoots.Invite> GetSingleWhere(Expression<Func<Domain.AggregateRoots.Invite, bool>> expression, string includes = null, bool disableAccessRightCheck = false)
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
    public async Task<IEnumerable<Domain.AggregateRoots.Invite>> GetListWhere(Expression<Func<Domain.AggregateRoots.Invite, bool>> expression, string includes = null, bool disableAccessRightCheck = false)
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
    public async Task<IEnumerable<Domain.AggregateRoots.Invite>> GetListOfQueryable(IQueryable<Domain.AggregateRoots.Invite> queryable = null, string includes = null, bool disableAccessRightCheck = false)
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
    public async Task<IEnumerable<Domain.AggregateRoots.Invite>> GetList(InviteQueryFilter filter = null, string includes = null, bool disableAccessRightCheck = false)
    {
        if (filter == null)
        {
            filter = new InviteQueryFilter() { };
        }
        logger?.LogDebug("GetList");
        using var unitOfWork = executionContext.StartUnitOfWork("UM", "InviteRepository GetList", UnitOfWorkType.Read);
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
        return result as IEnumerable<Domain.AggregateRoots.Invite>;
    }
    public async Task<IEnumerable<Domain.AggregateRoots.Invite>> GetList()
    {
        return await GetList(null);
    }
    public async Task<Domain.AggregateRoots.Invite> GetSingle(InviteQueryFilter filter, string includes = null, bool disableAccessRightCheck = false)
    {
        logger?.LogDebug("GetUniqueByQuery");
        using var unitOfWork = executionContext.StartUnitOfWork("UM", "InviteRepository GetUniqueByQuery", UnitOfWorkType.Read);
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
    public virtual IQueryable<Domain.AggregateRoots.Invite> AddIncludeFilterData(string includes, IQueryable<Domain.AggregateRoots.Invite> queryable)
    {
        throw new NotImplementedException();
    }
    #endregion
    #region Commands
    public virtual async Task<IOpsResult<Guid>> Add(Domain.AggregateRoots.Invite entity, Guid? forcedId = null)
    {
        logger?.LogDebug("Add");
        using var unitOfWork = executionContext.StartUnitOfWork("UM", "InviteRepository Add", UnitOfWorkType.Write);
        IOpsResult result;
        result = await EnsureAccessRights(entity, true);
        if (result.HasError) return result.ToResult<Guid>();
        foreach (var rule in entityRules)
        {
            result = await rule.OnBeforeAdd(entity, unitOfWork);
            if (result.HasError) return result.ToResult<Guid>();
        }
        var id = await Create(entity, this.executionContext.UserId, forcedId);
        entity.SetId(id);
        foreach (var rule in entityRules)
        {
            result = await rule.OnAfterAdd(id, entity, unitOfWork);
            if (result.HasError) return result.ToResult<Guid>();
        }
        unitOfWork.Complete();
        return IOpsResult.Ok<Guid>(entity.Id);
    }
    public virtual async Task<IOpsResult> Update(Domain.AggregateRoots.Invite entity)
    {
        logger?.LogDebug("Update");
        using var unitOfWork = executionContext.StartUnitOfWork("UM", "InviteRepository Update", UnitOfWorkType.Write);
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
    public async Task<IOpsResult> Remove(Domain.AggregateRoots.Invite entity)
    {
        return await RemoveWithId(entity.ComposedId);
    }
    public async virtual Task<IOpsResult> RemoveWithId(string id)
    {
        logger?.LogDebug("RemoveWithId");
        using var unitOfWork = executionContext.StartUnitOfWork("UM", "InviteRepository RemoveWithId", UnitOfWorkType.Write);
        var entity = await GetSingleByComposedId(id);
        if (entity == null)
        {
            return IOpsResult.Invalid($"could not find Invite with Id {id}");
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
    protected async Task<IOpsResult> CascadeDeleteAggregateData(Domain.AggregateRoots.Invite entity)
    {
        await Task.CompletedTask;

        return IOpsResult.Ok();
    }
    #endregion
    private void BindFromDictionary(Domain.AggregateRoots.Invite entity, Dictionary<string, string> dictionary)
    {
        logger?.LogDebug("BindFromDictionary");
        if (dictionary == null)
            throw new ArgumentNullException(nameof(dictionary));
        // Property [S][NO]
        if (dictionary.ContainsKey("Email"))
            entity.Email = (System.String)DisplayConverter.ConvertToString(dictionary["Email"]);

        if (dictionary.ContainsKey("CreatorName"))
            entity.CreatorName = (System.String)DisplayConverter.ConvertToString(dictionary["CreatorName"]);

        if (dictionary.ContainsKey("CreatorMessage"))
            entity.CreatorMessage = (System.String)DisplayConverter.ConvertToString(dictionary["CreatorMessage"]);

        // Property [E][NO]
        if (dictionary.ContainsKey("Status") && !string.IsNullOrEmpty(dictionary["Status"]))
            entity.Status = DisplayConverter.GetEnumData<SolidOps.UM.Contracts.Enums.InviteStatusEnum, Contracts.Enums.InviteStatusEnum>(dictionary["Status"]);

        // Property [M][R][NO][EN][AG]
        if (dictionary.ContainsKey("CreatorId"))
            entity.CreatorId = DisplayConverter.ConvertToIdentity<Guid>(dictionary["CreatorId"]);

    }
    #region Access Rights
    protected async Task<IQueryable<Domain.AggregateRoots.Invite>> AddAccessRightsCheckInQuery(IQueryable<Domain.AggregateRoots.Invite> queryable)
    {
        var result = HasOwnershipOverrideRight();
        if (result.HasError || !result.Data)
        {
            return await AddOwnershipCheck(queryable);
        }
        return queryable;
    }
    protected async Task<IOpsResult> EnsureAccessRights(Domain.AggregateRoots.Invite entity, bool isDuringAdd)
    {
        var result = HasOwnershipOverrideRight();
        if (result.HasError || !result.Data)
        {
            if (!(await HasOwnership(entity, isDuringAdd)))
            {
                if (isDuringAdd)
                    return IOpsResult.Forbidden($"Current user cannot create a Invite");
                return IOpsResult.Forbidden($"Current user cannot update Invite with Id {entity.Id}");
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
    protected virtual async Task<IQueryable<Domain.AggregateRoots.Invite>> AddOwnershipCheck(IQueryable<Domain.AggregateRoots.Invite> query)
    {
        await Task.CompletedTask;
        return query;
    }
    protected virtual async Task<bool> HasOwnership(Domain.AggregateRoots.Invite entity, bool isDuringAdd)
    {
        await Task.CompletedTask;
        return true;
    }
    #endregion
    #region FromDbSet
    public virtual Task<Domain.AggregateRoots.Invite> GetSingleOfQueryable(IQueryable<Domain.AggregateRoots.Invite> queryable, bool enableLazyLoading = false)
    {
        throw new NotImplementedException();
    }
    internal virtual async Task<IEnumerable<Domain.AggregateRoots.Invite>> InternalGetListOfQueryable(IQueryable<Domain.AggregateRoots.Invite> queryable, bool enableLazyLoading = false)
    {
        throw new NotImplementedException();
    }
    public Expression<Func<Domain.AggregateRoots.Invite, bool>> CreateExpressionFromQueryFilter(IQueryFilter filter)
    {
        if (string.IsNullOrEmpty(filter.LiteralQuery))
            return null;
        QueryElementFilteringTreeVisitor<Guid, Domain.AggregateRoots.Invite> visitor = new QueryElementFilteringTreeVisitor<Guid, Domain.AggregateRoots.Invite>();
        var allQuery = new QueryElementParser().Parse(filter.LiteralQuery);
        var exp = visitor.Visit(allQuery);
        return Expression.Lambda<Func<Domain.AggregateRoots.Invite, bool>>(exp, visitor.ParameterExpression);
    }
    public async Task<Guid> Create(Domain.AggregateRoots.Invite entity, string userId, Guid? forceId)
    {
        var efEntity = await Add(entity, userId, forceId);
        return efEntity.Id;
    }
    public virtual Task<Domain.AggregateRoots.Invite> Add(Domain.AggregateRoots.Invite entity, string userId, Guid? forceId)
    {
        throw new NotImplementedException();
    }
    public virtual Task<Domain.AggregateRoots.Invite> Update(Domain.AggregateRoots.Invite entity, string userId)
    {
        throw new NotImplementedException();
    }
    public virtual Task Delete(Domain.AggregateRoots.Invite entity)
    {
        throw new NotImplementedException();
    }
    #endregion
}