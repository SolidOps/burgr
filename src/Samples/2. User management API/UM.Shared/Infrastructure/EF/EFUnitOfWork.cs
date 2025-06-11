using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using SolidOps.UM.Shared.Domain.UnitOfWork;

namespace SolidOps.UM.Shared.Infrastructure;

public class EFUnitOfWork : BaseUnitOfWork, IUnitOfWork
{
    public DbContext DbContext { get; set; }
    private bool _isComplete = false;
    private IDbContextTransaction DbContextTransaction { get; set; }
    public EFUnitOfWork(string name, UnitOfWorkType unitOfWorkType, IServiceProvider serviceProvider, DbContext dbContext, bool withTransaction)
        : base(name, unitOfWorkType, serviceProvider)
    {
        this.DbContext = dbContext;
        this.IsTransactionnal = withTransaction;
        if (withTransaction)
        {
            if (this.DbContext.Database.CurrentTransaction == null)
            {
                DbContextTransaction = this.DbContext.Database.BeginTransaction();
            }
            else
            {
                DbContextTransaction = this.DbContext.Database.CurrentTransaction;
            }
        }
    }

    public override void Complete()
    {
        base.Complete();
        _isComplete = true;
    }

    public override void Dispose()
    {
        base.Dispose();
        //if (this.ParentUnitOfWork == null)
        //{
        //    if (_isComplete)
        //    {
        //        this.DbContext.SaveChanges();
        //    }
        //}

        if (this.ParentUnitOfWork == null || (this.IsTransactionnal && !this.ParentUnitOfWork.IsTransactionnal))
        {
            if (DbContextTransaction != null)
            {
                if (_isComplete)
                {
                    DbContextTransaction.Commit();
                }
                else
                {
                    DbContextTransaction.Rollback();
                }
                DbContextTransaction.Dispose();
            }
            this.DbContext.Dispose();
        }
    }
}
