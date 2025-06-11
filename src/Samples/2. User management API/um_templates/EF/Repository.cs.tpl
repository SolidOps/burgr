using Microsoft.EntityFrameworkCore;
using SolidOps.UM.Shared.Domain.Entities;
using SolidOps.UM.Shared.Infrastructure;

namespace MetaCorp.Template.Infrastructure.Repositories;

#region foreach MODEL[EN][AG]
public partial class CLASSNAMERepository
{
    public override IQueryable<Domain._DOMAINTYPE_.CLASSNAME> Queryable
    {
        get
        {
            return DbContext.Set<Domain._DOMAINTYPE_.CLASSNAME>().AsQueryable();
        }
    }

    public override async Task<Domain._DOMAINTYPE_.CLASSNAME> GetSingleOfQueryable(IQueryable<Domain._DOMAINTYPE_.CLASSNAME> queryable, bool enableLazyLoading = false)
    {
        var result = await queryable.ToListAsync();
        if (result.Count() == 1)
        {
            var entity = result.Single();
            return MapToDomain(entity, new Dictionary<Type, List<object>>(), enableLazyLoading);
        }
        return null;
    }

    internal override async Task<IEnumerable<Domain._DOMAINTYPE_.CLASSNAME>> InternalGetListOfQueryable(IQueryable<Domain._DOMAINTYPE_.CLASSNAME> queryable, bool enableLazyLoading = false)
    {
        List<Domain._DOMAINTYPE_.CLASSNAME> result = await queryable.ToListAsync();

        var domainQueryResult = new List<Domain._DOMAINTYPE_.CLASSNAME>();
        foreach (var entity in result)
        {
            domainQueryResult.Add(MapToDomain(entity, new Dictionary<Type, List<object>>(), enableLazyLoading));
        }
        return domainQueryResult;
    }

    public override IQueryable<Domain._DOMAINTYPE_.CLASSNAME> AddIncludeFilterData(string includes, IQueryable<Domain._DOMAINTYPE_.CLASSNAME> queryable)
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

    public override async Task<Domain._DOMAINTYPE_.CLASSNAME> Add(Domain._DOMAINTYPE_.CLASSNAME entity, string userId, _IDENTITY_KEY_TYPE_? forceId)
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
            efEntity.SetId(IdentityKeyHelper<_IDENTITY_KEY_TYPE_>.NewValue(true));
        }
        var set = DbContext.Set<Domain._DOMAINTYPE_.CLASSNAME>();
        await set.AddAsync(efEntity);
        entity.LazyLoader = efEntity.LazyLoader;
        entity.LazyLoadingEnabled = true;
        await DbContext.SaveChangesAsync();
        return efEntity;
    }

    public override async Task<Domain._DOMAINTYPE_.CLASSNAME> Update(Domain._DOMAINTYPE_.CLASSNAME entity, string userId)
    {
        var efEntity = MapFromDomain(entity, false);
        DbContext.Set<Domain._DOMAINTYPE_.CLASSNAME>().Update(efEntity);
        entity.LazyLoader = efEntity.LazyLoader;
        entity.LazyLoadingEnabled = true;
        await DbContext.SaveChangesAsync();
        return efEntity;
    }

    public override async Task Delete(Domain._DOMAINTYPE_.CLASSNAME entity)
    {
        var efEntity = MapFromDomain(entity, false);
        DbContext.Set<Domain._DOMAINTYPE_.CLASSNAME>().Remove(efEntity);
        await DbContext.SaveChangesAsync();
    }
}

public partial class BaseCLASSNAMERepository
{
    protected DbContext DbContext
    {
        get
        {
            return (this.executionContext.CurrentUnitOfWork as EFUnitOfWork).DbContext;
        }
    }
}
#endregion foreach MODEL