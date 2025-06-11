using SolidOps.UM.Shared.Contracts.Results;
using SolidOps.UM.Shared.Domain.Entities;
using SolidOps.UM.Shared.Domain.UnitOfWork;

namespace SolidOps.UM.Domain.AggregateRoots;

public partial class Invite
{
    public override IOpsResult PerformAdditionalValidation(ValidationStep validationStep, IUnitOfWork unitOfWork)
    {
        var result = base.PerformAdditionalValidation(validationStep, unitOfWork);
        if (result.HasError) return result;

        if (!User.IsValidEmail(Email))
            return IOpsResult.Invalid("email is not valid");

        if(validationStep == ValidationStep.Creation)
        {
            this.CreatorId = Guid.Parse(unitOfWork.ExecutionScope.UserId);
        }
        return IOpsResult.Ok();
    }
}
