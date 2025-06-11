using Microsoft.EntityFrameworkCore;

namespace SolidOps.TODO.Shared;

public sealed class UnitOfWork : IUnitOfWork
{
    public UnitOfWorkType UnitOfWorkType { get; set; }
    public IUnitOfWork? ParentUnitOfWork { get; set; }
    public IUserContext UserContext { get; private set; }
    
    public DbContext DbContext { get; set; }

    public UnitOfWork(UnitOfWorkType unitOfWorkType, IUserContext userContext, DbContext dbContext)        
    {
        DbContext = dbContext;
        UserContext = userContext;
        UnitOfWorkType = unitOfWorkType;
    }

    public async Task Complete()
    {
        if (this.ParentUnitOfWork == null)
        {
            await this.DbContext.SaveChangesAsync();
        }
    }

    public void Dispose()
    {
        UserContext.CurrentUnitOfWork = this.ParentUnitOfWork;
        if (this.ParentUnitOfWork == null)
        {
            this.DbContext.Dispose();
        }
    }
}
