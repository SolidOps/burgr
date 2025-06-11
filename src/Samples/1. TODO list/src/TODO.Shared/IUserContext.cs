namespace SolidOps.TODO.Shared;

public interface IUserContext
{
    IUnitOfWork? CurrentUnitOfWork { get; set; }
    IUnitOfWork StartUnitOfWork(UnitOfWorkType unitOfWorkType);
}
