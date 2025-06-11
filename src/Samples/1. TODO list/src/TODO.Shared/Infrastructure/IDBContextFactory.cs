using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace SolidOps.TODO.Shared.Infrastructure;

public interface IDBContextFactory
{
    DbContext Create(IUserContext userContext);

    Task EnsureDataAccessAndMigration(IServiceProvider serviceProvider);

    Task DeleteAllModuleData(IServiceProvider serviceProvider);
}
