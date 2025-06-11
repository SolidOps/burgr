using Microsoft.EntityFrameworkCore;
using SolidOps.UM.Shared.Domain.Entities;
using SolidOps.UM.Shared.Infrastructure;
namespace SolidOps.UM.Infrastructure.Repositories;
// Object [EN][AG]
public partial class LocalUserRepository
{
    public override IQueryable<Domain.AggregateRoots.LocalUser> Queryable
    {
        get
        {
            return DbContext.Set<Domain.AggregateRoots.LocalUser>().AsQueryable();
        }
    }
    public override async Task<Domain.AggregateRoots.LocalUser> GetSingleOfQueryable(IQueryable<Domain.AggregateRoots.LocalUser> queryable, bool enableLazyLoading = false)
    {
        var result = await queryable.ToListAsync();
        if (result.Count() == 1)
        {
            var entity = result.Single();
            return MapToDomain(entity, new Dictionary<Type, List<object>>(), enableLazyLoading);
        }
        return null;
    }
    internal override async Task<IEnumerable<Domain.AggregateRoots.LocalUser>> InternalGetListOfQueryable(IQueryable<Domain.AggregateRoots.LocalUser> queryable, bool enableLazyLoading = false)
    {
        List<Domain.AggregateRoots.LocalUser> result = await queryable.ToListAsync();
        var domainQueryResult = new List<Domain.AggregateRoots.LocalUser>();
        foreach (var entity in result)
        {
            domainQueryResult.Add(MapToDomain(entity, new Dictionary<Type, List<object>>(), enableLazyLoading));
        }
        return domainQueryResult;
    }
    public override IQueryable<Domain.AggregateRoots.LocalUser> AddIncludeFilterData(string includes, IQueryable<Domain.AggregateRoots.LocalUser> queryable)
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
    public override async Task<Domain.AggregateRoots.LocalUser> Add(Domain.AggregateRoots.LocalUser entity, string userId, Guid? forceId)
    {
        var efEntity = MapFromDomain(entity, true);
        if (forceId.HasValue)
        {
            if (!Equals(efEntity.Id, forceId.Value))
            {
                efEntity.SetId(forceId.Value);
            }
        }
        else
        {
            efEntity.SetId(IdentityKeyHelper<Guid>.NewValue(true));
        }
        var set = DbContext.Set<Domain.AggregateRoots.LocalUser>();
        await set.AddAsync(efEntity);
        entity.LazyLoader = efEntity.LazyLoader;
        entity.LazyLoadingEnabled = true;
        await DbContext.SaveChangesAsync();
        return efEntity;
    }
    public override async Task<Domain.AggregateRoots.LocalUser> Update(Domain.AggregateRoots.LocalUser entity, string userId)
    {
        var efEntity = MapFromDomain(entity, false);
        DbContext.Set<Domain.AggregateRoots.LocalUser>().Update(efEntity);
        entity.LazyLoader = efEntity.LazyLoader;
        entity.LazyLoadingEnabled = true;
        await DbContext.SaveChangesAsync();
        return efEntity;
    }
    public override async Task Delete(Domain.AggregateRoots.LocalUser entity)
    {
        var efEntity = MapFromDomain(entity, false);
        DbContext.Set<Domain.AggregateRoots.LocalUser>().Remove(efEntity);
        await DbContext.SaveChangesAsync();
    }
}
public partial class BaseLocalUserRepository
{
    protected DbContext DbContext
    {
        get
        {
            return (this.executionContext.CurrentUnitOfWork as EFUnitOfWork).DbContext;
        }
    }
}
public partial class UserRepository
{
    public override IQueryable<Domain.AggregateRoots.User> Queryable
    {
        get
        {
            return DbContext.Set<Domain.AggregateRoots.User>().AsQueryable();
        }
    }
    public override async Task<Domain.AggregateRoots.User> GetSingleOfQueryable(IQueryable<Domain.AggregateRoots.User> queryable, bool enableLazyLoading = false)
    {
        var result = await queryable.ToListAsync();
        if (result.Count() == 1)
        {
            var entity = result.Single();
            return MapToDomain(entity, new Dictionary<Type, List<object>>(), enableLazyLoading);
        }
        return null;
    }
    internal override async Task<IEnumerable<Domain.AggregateRoots.User>> InternalGetListOfQueryable(IQueryable<Domain.AggregateRoots.User> queryable, bool enableLazyLoading = false)
    {
        List<Domain.AggregateRoots.User> result = await queryable.ToListAsync();
        var domainQueryResult = new List<Domain.AggregateRoots.User>();
        foreach (var entity in result)
        {
            domainQueryResult.Add(MapToDomain(entity, new Dictionary<Type, List<object>>(), enableLazyLoading));
        }
        return domainQueryResult;
    }
    public override IQueryable<Domain.AggregateRoots.User> AddIncludeFilterData(string includes, IQueryable<Domain.AggregateRoots.User> queryable)
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
    public override async Task<Domain.AggregateRoots.User> Add(Domain.AggregateRoots.User entity, string userId, Guid? forceId)
    {
        var efEntity = MapFromDomain(entity, true);
        if (forceId.HasValue)
        {
            if (!Equals(efEntity.Id, forceId.Value))
            {
                efEntity.SetId(forceId.Value);
            }
        }
        else
        {
            efEntity.SetId(IdentityKeyHelper<Guid>.NewValue(true));
        }
        var set = DbContext.Set<Domain.AggregateRoots.User>();
        await set.AddAsync(efEntity);
        entity.LazyLoader = efEntity.LazyLoader;
        entity.LazyLoadingEnabled = true;
        await DbContext.SaveChangesAsync();
        return efEntity;
    }
    public override async Task<Domain.AggregateRoots.User> Update(Domain.AggregateRoots.User entity, string userId)
    {
        var efEntity = MapFromDomain(entity, false);
        DbContext.Set<Domain.AggregateRoots.User>().Update(efEntity);
        entity.LazyLoader = efEntity.LazyLoader;
        entity.LazyLoadingEnabled = true;
        await DbContext.SaveChangesAsync();
        return efEntity;
    }
    public override async Task Delete(Domain.AggregateRoots.User entity)
    {
        var efEntity = MapFromDomain(entity, false);
        DbContext.Set<Domain.AggregateRoots.User>().Remove(efEntity);
        await DbContext.SaveChangesAsync();
    }
}
public partial class BaseUserRepository
{
    protected DbContext DbContext
    {
        get
        {
            return (this.executionContext.CurrentUnitOfWork as EFUnitOfWork).DbContext;
        }
    }
}
public partial class UserRightRepository
{
    public override IQueryable<Domain.Entities.UserRight> Queryable
    {
        get
        {
            return DbContext.Set<Domain.Entities.UserRight>().AsQueryable();
        }
    }
    public override async Task<Domain.Entities.UserRight> GetSingleOfQueryable(IQueryable<Domain.Entities.UserRight> queryable, bool enableLazyLoading = false)
    {
        var result = await queryable.ToListAsync();
        if (result.Count() == 1)
        {
            var entity = result.Single();
            return MapToDomain(entity, new Dictionary<Type, List<object>>(), enableLazyLoading);
        }
        return null;
    }
    internal override async Task<IEnumerable<Domain.Entities.UserRight>> InternalGetListOfQueryable(IQueryable<Domain.Entities.UserRight> queryable, bool enableLazyLoading = false)
    {
        List<Domain.Entities.UserRight> result = await queryable.ToListAsync();
        var domainQueryResult = new List<Domain.Entities.UserRight>();
        foreach (var entity in result)
        {
            domainQueryResult.Add(MapToDomain(entity, new Dictionary<Type, List<object>>(), enableLazyLoading));
        }
        return domainQueryResult;
    }
    public override IQueryable<Domain.Entities.UserRight> AddIncludeFilterData(string includes, IQueryable<Domain.Entities.UserRight> queryable)
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
    public override async Task<Domain.Entities.UserRight> Add(Domain.Entities.UserRight entity, string userId, Guid? forceId)
    {
        var efEntity = MapFromDomain(entity, true);
        if (forceId.HasValue)
        {
            if (!Equals(efEntity.Id, forceId.Value))
            {
                efEntity.SetId(forceId.Value);
            }
        }
        else
        {
            efEntity.SetId(IdentityKeyHelper<Guid>.NewValue(true));
        }
        var set = DbContext.Set<Domain.Entities.UserRight>();
        await set.AddAsync(efEntity);
        entity.LazyLoader = efEntity.LazyLoader;
        entity.LazyLoadingEnabled = true;
        await DbContext.SaveChangesAsync();
        return efEntity;
    }
    public override async Task<Domain.Entities.UserRight> Update(Domain.Entities.UserRight entity, string userId)
    {
        var efEntity = MapFromDomain(entity, false);
        DbContext.Set<Domain.Entities.UserRight>().Update(efEntity);
        entity.LazyLoader = efEntity.LazyLoader;
        entity.LazyLoadingEnabled = true;
        await DbContext.SaveChangesAsync();
        return efEntity;
    }
    public override async Task Delete(Domain.Entities.UserRight entity)
    {
        var efEntity = MapFromDomain(entity, false);
        DbContext.Set<Domain.Entities.UserRight>().Remove(efEntity);
        await DbContext.SaveChangesAsync();
    }
}
public partial class BaseUserRightRepository
{
    protected DbContext DbContext
    {
        get
        {
            return (this.executionContext.CurrentUnitOfWork as EFUnitOfWork).DbContext;
        }
    }
}
public partial class RightRepository
{
    public override IQueryable<Domain.Entities.Right> Queryable
    {
        get
        {
            return DbContext.Set<Domain.Entities.Right>().AsQueryable();
        }
    }
    public override async Task<Domain.Entities.Right> GetSingleOfQueryable(IQueryable<Domain.Entities.Right> queryable, bool enableLazyLoading = false)
    {
        var result = await queryable.ToListAsync();
        if (result.Count() == 1)
        {
            var entity = result.Single();
            return MapToDomain(entity, new Dictionary<Type, List<object>>(), enableLazyLoading);
        }
        return null;
    }
    internal override async Task<IEnumerable<Domain.Entities.Right>> InternalGetListOfQueryable(IQueryable<Domain.Entities.Right> queryable, bool enableLazyLoading = false)
    {
        List<Domain.Entities.Right> result = await queryable.ToListAsync();
        var domainQueryResult = new List<Domain.Entities.Right>();
        foreach (var entity in result)
        {
            domainQueryResult.Add(MapToDomain(entity, new Dictionary<Type, List<object>>(), enableLazyLoading));
        }
        return domainQueryResult;
    }
    public override IQueryable<Domain.Entities.Right> AddIncludeFilterData(string includes, IQueryable<Domain.Entities.Right> queryable)
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
    public override async Task<Domain.Entities.Right> Add(Domain.Entities.Right entity, string userId, Guid? forceId)
    {
        var efEntity = MapFromDomain(entity, true);
        if (forceId.HasValue)
        {
            if (!Equals(efEntity.Id, forceId.Value))
            {
                efEntity.SetId(forceId.Value);
            }
        }
        else
        {
            efEntity.SetId(IdentityKeyHelper<Guid>.NewValue(true));
        }
        var set = DbContext.Set<Domain.Entities.Right>();
        await set.AddAsync(efEntity);
        entity.LazyLoader = efEntity.LazyLoader;
        entity.LazyLoadingEnabled = true;
        await DbContext.SaveChangesAsync();
        return efEntity;
    }
    public override async Task<Domain.Entities.Right> Update(Domain.Entities.Right entity, string userId)
    {
        var efEntity = MapFromDomain(entity, false);
        DbContext.Set<Domain.Entities.Right>().Update(efEntity);
        entity.LazyLoader = efEntity.LazyLoader;
        entity.LazyLoadingEnabled = true;
        await DbContext.SaveChangesAsync();
        return efEntity;
    }
    public override async Task Delete(Domain.Entities.Right entity)
    {
        var efEntity = MapFromDomain(entity, false);
        DbContext.Set<Domain.Entities.Right>().Remove(efEntity);
        await DbContext.SaveChangesAsync();
    }
}
public partial class BaseRightRepository
{
    protected DbContext DbContext
    {
        get
        {
            return (this.executionContext.CurrentUnitOfWork as EFUnitOfWork).DbContext;
        }
    }
}
public partial class InviteRepository
{
    public override IQueryable<Domain.AggregateRoots.Invite> Queryable
    {
        get
        {
            return DbContext.Set<Domain.AggregateRoots.Invite>().AsQueryable();
        }
    }
    public override async Task<Domain.AggregateRoots.Invite> GetSingleOfQueryable(IQueryable<Domain.AggregateRoots.Invite> queryable, bool enableLazyLoading = false)
    {
        var result = await queryable.ToListAsync();
        if (result.Count() == 1)
        {
            var entity = result.Single();
            return MapToDomain(entity, new Dictionary<Type, List<object>>(), enableLazyLoading);
        }
        return null;
    }
    internal override async Task<IEnumerable<Domain.AggregateRoots.Invite>> InternalGetListOfQueryable(IQueryable<Domain.AggregateRoots.Invite> queryable, bool enableLazyLoading = false)
    {
        List<Domain.AggregateRoots.Invite> result = await queryable.ToListAsync();
        var domainQueryResult = new List<Domain.AggregateRoots.Invite>();
        foreach (var entity in result)
        {
            domainQueryResult.Add(MapToDomain(entity, new Dictionary<Type, List<object>>(), enableLazyLoading));
        }
        return domainQueryResult;
    }
    public override IQueryable<Domain.AggregateRoots.Invite> AddIncludeFilterData(string includes, IQueryable<Domain.AggregateRoots.Invite> queryable)
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
    public override async Task<Domain.AggregateRoots.Invite> Add(Domain.AggregateRoots.Invite entity, string userId, Guid? forceId)
    {
        var efEntity = MapFromDomain(entity, true);
        if (forceId.HasValue)
        {
            if (!Equals(efEntity.Id, forceId.Value))
            {
                efEntity.SetId(forceId.Value);
            }
        }
        else
        {
            efEntity.SetId(IdentityKeyHelper<Guid>.NewValue(true));
        }
        var set = DbContext.Set<Domain.AggregateRoots.Invite>();
        await set.AddAsync(efEntity);
        entity.LazyLoader = efEntity.LazyLoader;
        entity.LazyLoadingEnabled = true;
        await DbContext.SaveChangesAsync();
        return efEntity;
    }
    public override async Task<Domain.AggregateRoots.Invite> Update(Domain.AggregateRoots.Invite entity, string userId)
    {
        var efEntity = MapFromDomain(entity, false);
        DbContext.Set<Domain.AggregateRoots.Invite>().Update(efEntity);
        entity.LazyLoader = efEntity.LazyLoader;
        entity.LazyLoadingEnabled = true;
        await DbContext.SaveChangesAsync();
        return efEntity;
    }
    public override async Task Delete(Domain.AggregateRoots.Invite entity)
    {
        var efEntity = MapFromDomain(entity, false);
        DbContext.Set<Domain.AggregateRoots.Invite>().Remove(efEntity);
        await DbContext.SaveChangesAsync();
    }
}
public partial class BaseInviteRepository
{
    protected DbContext DbContext
    {
        get
        {
            return (this.executionContext.CurrentUnitOfWork as EFUnitOfWork).DbContext;
        }
    }
}