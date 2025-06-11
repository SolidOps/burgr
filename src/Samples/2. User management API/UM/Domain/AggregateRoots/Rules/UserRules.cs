using Microsoft.Extensions.DependencyInjection;
using SolidOps.UM.Shared.Contracts.Results;
using SolidOps.UM.Shared.Domain.Entities;
using SolidOps.UM.Shared.Domain.UnitOfWork;
using SolidOps.UM.Domain.Repositories;
using SolidOps.UM.Domain.Services;

namespace SolidOps.UM.Domain.AggregateRoots.Rules;

public partial class UserRules
{
    protected override async Task<IOpsResult> CreateLocalUser(Guid id, User entity, ValidationStep validationStep, IUnitOfWork unitOfWork)
    {
        var result = await base.CreateLocalUser(id, entity, validationStep, unitOfWork);
        if(result.HasError)
            return result;

        if (unitOfWork.UnitOfWorkType == UnitOfWorkType.Write)
        {
            if (string.IsNullOrEmpty(entity.Provider))
            {
                entity.Provider = typeof(LocalIdentityProviderService).Name;

                var _dependencyLocalUserRepository = serviceProvider.GetRequiredService<ILocalUserRepository>();
                var lu = LocalUser.Create(entity.Email, "");
                result = await _dependencyLocalUserRepository.Add(lu);
                if (result.HasError) return result;
            }
        }

        return IOpsResult.Ok();
    }
}
