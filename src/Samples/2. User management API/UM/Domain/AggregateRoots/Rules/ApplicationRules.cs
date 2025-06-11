using Microsoft.Extensions.DependencyInjection;
using SolidOps.Burgr.Shared.Contracts.Results;
using SolidOps.Burgr.Shared.Domain.Entities;
using SolidOps.Burgr.Shared.Domain.UnitOfWork;
using SolidOps.UM.Domain.Entities;
using SolidOps.UM.Domain.Repositories;

namespace SolidOps.UM.Domain.AggregateRoots.Rules;

public partial class ApplicationRules
{
    protected override async Task<IOpsResult> CreateApplicationEnvironments(Guid id, Application entity, ValidationStep validationStep, IUnitOfWork unitOfWork)
    {
        var result = await base.CreateApplicationEnvironments(id, entity, validationStep, unitOfWork);
        if (result.HasError) return result;

        if (unitOfWork.UnitOfWorkType == UnitOfWorkType.Write)
        {
            if (entity.ApplicationEnvironments != null)
            {
                foreach (var appEnv in entity.ApplicationEnvironments)
                {
                    if (appEnv.EnvironmentId != Guid.Empty)
                    {
                        var _dependencyApplicationEnvironmentRepository = serviceProvider.GetRequiredService<IApplicationEnvironmentRepository>();
                        var applicationEnvironment = ApplicationEnvironment.Create(appEnv.ConfigurationContent, appEnv.EnvironmentId, id);
                        result = await _dependencyApplicationEnvironmentRepository.Add(applicationEnvironment);
                        if (result.HasError) return result;
                    }
                }
            }
        }

        return IOpsResult.Ok();
    }


}
