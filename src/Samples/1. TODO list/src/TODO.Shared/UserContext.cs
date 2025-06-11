using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SolidOps.TODO.Shared.Infrastructure;
using System;

namespace SolidOps.TODO.Shared;

public class UserContext : IUserContext
{
    private readonly IDBContextFactory dbContextFactory;

    public IUnitOfWork CurrentUnitOfWork { get; set; }

    public UserContext(IDBContextFactory dBContextFactory)
    {
        this.dbContextFactory = dBContextFactory;
    }

    public IUnitOfWork StartUnitOfWork(UnitOfWorkType unitOfWorkType)
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
        DbContext dbContext = CreateOrGetDbContext();
        var unitOfWork = new UnitOfWork(uowt, this, dbContext);

        if (CurrentUnitOfWork != null)
        {
            unitOfWork.ParentUnitOfWork = CurrentUnitOfWork;
        }
        CurrentUnitOfWork = unitOfWork;
        return unitOfWork;
    }

    public DbContext CreateOrGetDbContext()
    {
        if (CurrentUnitOfWork?.DbContext != null)
        {
            return CurrentUnitOfWork.DbContext;
        }
        return dbContextFactory.Create(this);
    }
}
