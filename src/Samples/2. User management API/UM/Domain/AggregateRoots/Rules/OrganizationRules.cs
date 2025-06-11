using Microsoft.Extensions.DependencyInjection;
using SolidOps.Burgr.Shared.Contracts.Results;
using SolidOps.Burgr.Shared.Domain.Entities;
using SolidOps.Burgr.Shared.Domain.UnitOfWork;
using SolidOps.UM.Domain.Entities;
using SolidOps.UM.Domain.Repositories;

namespace SolidOps.UM.Domain.AggregateRoots.Rules;

public partial class OrganizationRules
{
    protected override async Task<IOpsResult> SyncOrganizationRoles(Guid id, Organization entity, ValidationStep validationStep, IUnitOfWork unitOfWork)
    {
        var result = await base.SyncOrganizationRoles(id, entity, validationStep, unitOfWork);
        if (result.HasError) return result;

        var _dependencyRoleRepository = serviceProvider.GetRequiredService<IRoleRepository>();
        return await _dependencyRoleRepository.SyncOrganizationRoles(id, entity.Roles ?? new List<Role>());
    }
}
