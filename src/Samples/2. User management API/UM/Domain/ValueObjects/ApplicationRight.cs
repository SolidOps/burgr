
using SolidOps.Burgr.Shared.Contracts.Results;
using SolidOps.Burgr.Shared.Domain.UnitOfWork;

namespace SolidOps.UM.Domain.ValueObjects;

public partial class ApplicationRight
{
    public const string SEPARATOR = "|";
    public const string CAN_MANAGE_ACCESS_RIGHTS = "ManageAccessRights";
    protected override string CalculateName()
    {
        return Application + SEPARATOR + Right;
    }

    internal static IOpsResult ValidateRights(IReadOnlyCollection<ApplicationRight> applicationRights, IUnitOfWork unitOfWork)
    {
        if (applicationRights == null)
            return IOpsResult.Ok();
        foreach (var applicationRight in applicationRights)
        {
            if (applicationRight.Application == IExecutionScope.ALLAPP)
                return IOpsResult.Invalid("cannot add access to all apps");

            if (applicationRight.Right == IExecutionScope.ALLRIGHT)
                return IOpsResult.Invalid("cannot give all rights");

            if (!unitOfWork.ExecutionScope.HasRight(applicationRight.Application, CAN_MANAGE_ACCESS_RIGHTS))
                return IOpsResult.Invalid($"cannot add access to {applicationRight.Application} because current user cannot have access to {applicationRight.Application} or does not have right {CAN_MANAGE_ACCESS_RIGHTS}");
        }

        return IOpsResult.Ok();
    }
}
