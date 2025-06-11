using SolidOps.Burgr.Shared.Contracts.Results;
using SolidOps.Burgr.Shared.Domain.Entities;
using SolidOps.Burgr.Shared.Domain.UnitOfWork;
using SolidOps.UM.Domain.ValueObjects;

namespace SolidOps.UM.Domain.AggregateRoots;

public partial class Organization
{
    public override IOpsResult PerformAdditionalValidation(ValidationStep validationStep, IUnitOfWork unitOfWork)
    {
        var result = base.PerformAdditionalValidation(validationStep, unitOfWork);
        if (result.HasError) return result;

        return ApplicationRight.ValidateRights(ApplicationRights, unitOfWork);
    }

}
