using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SolidOps.UM.Shared.Domain.UnitOfWork;
using System.Threading.Tasks;

namespace SolidOps.UM.Shared.Infrastructure.EF;

public interface IBurgrDBContextFactory
{
    DbContext Create(IExecutionContext executionContext, ILoggerFactory loggerFactory);

    Task EnsureDataAccessAndMigration(IServiceProvider serviceProvider);

    Task DeleteAllModuleData(IServiceProvider serviceProvider);
}

