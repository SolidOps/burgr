using Microsoft.EntityFrameworkCore;

namespace SolidOps.TODO.Shared;

public interface IUnitOfWork : IDisposable
{
    UnitOfWorkType UnitOfWorkType { get; set; }
    IUnitOfWork? ParentUnitOfWork { get; set; }
    IUserContext UserContext { get; }
    DbContext DbContext { get; }

    Task Complete();
}

public enum UnitOfWorkType
{
    Read = 0,
    Write = 1,
    QueryUpdate = 2
}
