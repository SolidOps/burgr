using SolidOps.UM.Shared.Contracts.Results;
using SolidOps.UM.Shared.Domain.UnitOfWork;

namespace SolidOps.UM.Shared.Domain.Entities;

public enum ValidationStep
{
    Creation,
    Update,
    Delete
}

public interface IEntityOfDomain<T>
    where T : struct
{
    T Id { get; }
}

public interface IDomainObject
{
    IOpsResult Validate(ValidationStep validationStep, IUnitOfWork unitOfWork);
    string RepairInput(string json);
    void RepairAfterException(Exception exception, string json);
}

public interface IDomainEntity<T> : IDomainObject, IEntityOfDomain<T>
    where T : struct
{
    string GetPublicId();

    string ComposedId { get; }

    void SetId(T id, List<T> ids = null);
}
