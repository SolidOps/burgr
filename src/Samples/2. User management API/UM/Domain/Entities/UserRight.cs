using SolidOps.UM.Shared.Contracts.Results;
using SolidOps.UM.Shared.Domain.UnitOfWork;

namespace SolidOps.UM.Domain.Entities;

public partial class UserRight
{
    public const string SEPARATOR = "|";
    public const string CAN_MANAGE_ACCESS_RIGHTS = "ManageAccessRights";
    
    internal static IOpsResult ValidateRights(IReadOnlyCollection<string> rights, IUnitOfWork unitOfWork)
    {
        if (rights == null)
            return IOpsResult.Ok();
        foreach (var right in rights)
        {
            if (right == IExecutionContext.ALLRIGHT)
                return IOpsResult.Invalid("cannot give all rights");
        }

        return IOpsResult.Ok();
    }
}
