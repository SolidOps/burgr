
using SolidOps.UM.Shared.Domain.Configuration;
using SolidOps.UM.Shared.Domain.Entities;

namespace SolidOps.UM.Shared.Domain.UnitOfWork;

public interface IDataAccessFactory
{
    IExtendedConfiguration Configuration { get; set; }
    public string Name { get; set; }
    Task EnsureDataAccessAndMigration(IServiceProvider serviceProvider);
    Task DeleteAllModuleData(IServiceProvider serviceProvider);

    IUnitOfWork CreateUnitOfWork(string unitOfWorkName, UnitOfWorkType unitOfWorkType, IServiceProvider serviceProvider, bool withTransaction);
    
    DatabaseInfo DatabaseInfo { get; set; }
}
