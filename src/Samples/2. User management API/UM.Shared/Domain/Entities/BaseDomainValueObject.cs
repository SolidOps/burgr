using SolidOps.UM.Shared.Contracts.Results;
using SolidOps.UM.Shared.Domain.UnitOfWork;

namespace SolidOps.UM.Shared.Domain.Entities;

public abstract class BaseDomainValueObject : IDomainObject
{
    public virtual IOpsResult Validate(ValidationStep validationStep, IUnitOfWork unitOfWork)
    {
        return new OkResult();
    }

    public virtual IOpsResult PerformAdditionalValidation(ValidationStep validationStep, IUnitOfWork unitOfWork)
    {
        return IOpsResult.Ok();
    }

    public string RepairInput(string json)
    {
        return json;
    }

    public void RepairAfterException(Exception exception, string json)
    {
        throw exception;
    }
}
