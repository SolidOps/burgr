using SolidOps.UM.Shared.Domain.Configuration;
using SolidOps.UM.Shared.Domain.Entities;
using SolidOps.UM.Shared.Domain.UnitOfWork;

namespace SolidOps.UM.Shared.Infrastructure;

public abstract class BaseDataAccessFactory<T> : IDataAccessFactory
    where T : struct
{
    public IExtendedConfiguration Configuration { get; set; }
    
    public BaseDataAccessFactory(IExtendedConfiguration configuration)
    {
        this.Configuration = configuration;
    }

    public string Name { get; set; }

    public string Prefix { get; set; }

    public DatabaseInfo DatabaseInfo { get; set; }

    public abstract Task EnsureDataAccessAndMigration(IServiceProvider serviceProvider);

    public abstract Task DeleteAllModuleData(IServiceProvider serviceProvider);

    public abstract IUnitOfWork CreateUnitOfWork(string unitOfWorkName, UnitOfWorkType unitOfWorkType, IServiceProvider serviceProvider, bool withTransaction);
}
