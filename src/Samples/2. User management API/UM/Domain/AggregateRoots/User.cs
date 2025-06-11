using SolidOps.UM.Shared.Contracts.Results;
using SolidOps.UM.Shared.Domain.Entities;
using SolidOps.UM.Shared.Domain.UnitOfWork;
using SolidOps.UM.Domain.Entities;

namespace SolidOps.UM.Domain.AggregateRoots;

public partial class User
{
    public override IOpsResult PerformAdditionalValidation(ValidationStep validationStep, IUnitOfWork unitOfWork)
    {
        var result = base.PerformAdditionalValidation(validationStep, unitOfWork);
        if (result.HasError) return result;

        bool isTechnicalUser = this.TechnicalUser;

        if (!isTechnicalUser && !IsValidEmail(Email))
            return IOpsResult.Invalid("email is not valid");

        if (validationStep == ValidationStep.Creation)
        {
            if (!isTechnicalUser)
            {
                var rights = UserRights?.Select(ur => ur.Right?.Name).Distinct().ToList();
                result = UserRight.ValidateRights(rights, unitOfWork);
                if (result.HasError) return result;
            }
        }
        else
        {
            if (isTechnicalUser)
            {
                return IOpsResult.Invalid("Update on tech user is not possible");
            }
            var rights = UserRights?.Select(ur => ur.Right?.Name).Distinct().ToList();
            result = UserRight.ValidateRights(rights, unitOfWork);
            if (result.HasError) return result;
        }

        this.TechnicalUser = isTechnicalUser;

        return IOpsResult.Ok();
    }

    public static bool IsValidEmail(string email)
    {
        var trimmedEmail = email.Trim();

        if (trimmedEmail.EndsWith("."))
        {
            return false;
        }
        if (!trimmedEmail.Contains("@"))
        {
            return false;
        }
        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == trimmedEmail;
        }
        catch
        {
            return false;
        }
    }

    protected override List<string> CalculateRights()
    {
        List<string> rights = new List<string>();

        if (this.UserRights != null)
        {
            foreach (var right in this.UserRights.Select(ur => ur.Right))
            {
                if (!rights.Contains(right.Name))
                {
                    rights.Add(right.Name);
                }
            }
        }

        return rights;
    }
}
