using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SolidOps.UM.Shared.Domain.Configuration;
using SolidOps.UM.Shared.Domain.Entities;
using SolidOps.UM.Shared.Domain.UnitOfWork;

namespace SolidOps.UM.Shared.Infrastructure.EF;

public abstract class BaseEFDataAccessFactory<T, TContext> : BaseDataAccessFactory<T>
    where T : struct
    where TContext : DbContext
{
    public BaseEFDataAccessFactory(IExtendedConfiguration configuration) : base(configuration)
    {
        this.Configuration = configuration;
    }

    public string Name { get; set; }

    public string Prefix { get; set; }

    public DatabaseInfo DatabaseInfo { get; set; }

    public override async Task EnsureDataAccessAndMigration(IServiceProvider serviceProvider)
    {
        var dbContext = CreateOrGetDbContext(serviceProvider);
        await dbContext.Database.EnsureCreatedAsync();
        await dbContext.Database.MigrateAsync();
    }

    public override async Task DeleteAllModuleData(IServiceProvider serviceProvider)
    {
        // start DB
        var dbContext = CreateOrGetDbContext(serviceProvider);
        dbContext.Database.EnsureDeleted();
        dbContext.Database.EnsureCreated();
        await Task.CompletedTask;
    }

    public override IUnitOfWork CreateUnitOfWork(string unitOfWorkName, UnitOfWorkType unitOfWorkType, IServiceProvider serviceProvider, bool withTransaction)
    {
        TContext dbContext = CreateOrGetDbContext(serviceProvider);
        var unitOfWork = new EFUnitOfWork(unitOfWorkName, unitOfWorkType, serviceProvider, dbContext, withTransaction);
        unitOfWork.DataAccessFactory = this;
        return unitOfWork;
    }

    public abstract TContext CreateOrGetDbContext(IServiceProvider serviceProvider);

    public abstract void ConfigureModuleServices(IServiceCollection services, IConfiguration configuration);

}
