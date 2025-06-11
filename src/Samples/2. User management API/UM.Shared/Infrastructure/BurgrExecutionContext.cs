using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SolidOps.UM.Shared.Domain.UnitOfWork;
using SolidOps.UM.Shared.Infrastructure.EF;

namespace SolidOps.UM.Shared.Infrastructure;

public class BurgrExecutionContext : IExecutionContext
{
    #region Private fields
    private string userId;
    private List<string> rights;
    private string authorization;
    private Func<(string userId, List<string> rights, string authorization)> callback;
    #endregion

    #region Protected
    protected readonly IServiceProvider serviceProvider;
    protected readonly IEnumerable<IDataAccessFactory> dataAccessFactories;
    #endregion

    public Dictionary<string, object> TemporaryData { get; set; }
    public List<string> MandatoryRights { get; set; }
    public List<string> OwnershipOverrideRights { get; set; }

    public bool EnableDiagnostic { get; protected set; } = false;


    public BurgrExecutionContext(ILoggerFactory loggerFactory, IServiceProvider serviceProvider, IEnumerable<IDataAccessFactory> dataAccessFactories)
    {
        this.serviceProvider = serviceProvider;
        this.dataAccessFactories = dataAccessFactories;
        TemporaryData = new Dictionary<string, object>();
        MandatoryRights = new List<string>();
        OwnershipOverrideRights = new List<string>();
    }

    public string Authorization
    {
        get
        {
            if (string.IsNullOrEmpty(this.authorization) && callback != null)
            {
                var result = callback();
                this.userId = result.userId;
                this.rights = result.rights;
                this.authorization = result.authorization;
            }
            return this.authorization;
        }
    }

    public string UserId
    {
        get
        {
            if (string.IsNullOrEmpty(this.userId) && callback != null)
            {
                var result = callback();
                this.userId = result.userId;
                this.rights = result.rights;
                this.authorization = result.authorization;
            }
            return this.userId;
        }
    }
    public List<string> Rights
    {
        get
        {
            if (this.rights == null && callback != null)
            {
                var result = callback();
                this.userId = result.userId;
                this.rights = result.rights;
                this.authorization = result.authorization;
            }
            return this.rights;
        }
    }
    public void SetUserId(Func<(string userId, List<string> rights, string authorization)> callback)
    {
        this.callback = callback;
    }

    public IUnitOfWork StartUnitOfWork(string moduleName, string unitOfWorkName, UnitOfWorkType unitOfWorkType)
    {
        UnitOfWorkType uowt;
        if (CurrentUnitOfWork != null)
        {
            if (CurrentUnitOfWork.UnitOfWorkType == UnitOfWorkType.Read && (unitOfWorkType == UnitOfWorkType.Write || unitOfWorkType == UnitOfWorkType.QueryUpdate))
                throw new Exception("Initial unit Of Work type is Read and new unit of work type is Write");
            uowt = CurrentUnitOfWork.UnitOfWorkType;
        }
        else
        {
            if ((int)unitOfWorkType > 3)
                throw new Exception("UnitOfWorkType must be either Write Or Read Or QueryUpdate Or Legacy");
            uowt = (UnitOfWorkType)(int)unitOfWorkType;
        }
        DbContext dbContext = CreateOrGetDbContext(serviceProvider);
        var unitOfWork = new EFUnitOfWork(unitOfWorkName, uowt, serviceProvider, dbContext, false);

        if (CurrentUnitOfWork != null)
        {
            unitOfWork.ParentUnitOfWork = CurrentUnitOfWork;
        }
        CurrentUnitOfWork = unitOfWork;
        return unitOfWork;
    }

    public DbContext CreateOrGetDbContext(IServiceProvider serviceProvider)
    {
        if (CurrentUnitOfWork is EFUnitOfWork currentUnitOfWork
            && currentUnitOfWork.DbContext != null
            && currentUnitOfWork.IsTransactionnal
            )
        {
            return currentUnitOfWork.DbContext;
        }
        var dbContextFactory = serviceProvider.GetRequiredService<IBurgrDBContextFactory>();
        var executionContext = serviceProvider.GetRequiredService<IExecutionContext>();
        var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
        return dbContextFactory.Create(executionContext, loggerFactory);
    }

    public IUnitOfWork CurrentUnitOfWork { get; set; }

    public bool HasRight(string right)
    {
        if (this.Rights == null)
            return false;
        
        if (this.Rights.Contains(IExecutionContext.WILDCARD))
            return true;
        
        return this.Rights.Contains(right);
    }

    public virtual IDataAccessFactory GetDataAccessFactory(string moduleName)
    {
        return this.dataAccessFactories.Where(dataAccessFactory => dataAccessFactory.Name == moduleName + "Module").Single();
    }

    void IDisposable.Dispose()
    {
        GC.SuppressFinalize(this);
    }
}
