using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SolidOps.UM.Shared.Domain.Configuration;
using SolidOps.UM.Shared.Domain.CrossCutting;
using SolidOps.UM.Shared.Domain.UnitOfWork;
using SolidOps.UM.Shared.Infrastructure;
using SolidOps.UM.Shared.Infrastructure.EF;
using SolidOps.SubZero;
using SolidOps.UM.Domain.Repositories;
namespace SolidOps.UM.Infrastructure.EF;
public partial interface IUMEFDataAccessFactory : IUMDataAccessFactory, IEFDataAccessFactory
{
}
public partial class UMEFServiceRegistrar : IServiceRegistrar
{
    public int Priority => 2;
    public void ConfigureServices(IServiceCollection services, IExtendedConfiguration configuration)
    {
        services.AddScopedFromExisting<IUMEFDataAccessFactory, IUMDataAccessFactory>();
    }
}
public partial class UMContext : DbContext
{
    public string ConnectionString { get; set; }
    protected List<IDbContextConfiguration> DbContextConfigurations { get; private set; }
    private readonly ILoggerFactory loggerFactory;
    private readonly IExecutionContext executionContext;
    private readonly IInterceptor requestInterceptor;
    public UMContext(string connectionString, IExecutionContext executionContext, ILoggerFactory loggerFactory)
    {
        ConnectionString = connectionString;
        this.executionContext = executionContext;
        DbContextConfigurations = new List<IDbContextConfiguration>()
        {
            new UMConfiguration("")
        };
        AddConfigurations();
        this.loggerFactory = loggerFactory;
        this.ChangeTracker.StateChanged += ChangeTracker_StateChanged;
    }
    private void ChangeTracker_StateChanged(object sender, Microsoft.EntityFrameworkCore.ChangeTracking.EntityStateChangedEventArgs e)
    {
        if (e.NewState == EntityState.Deleted)
        {
        }
    }
    partial void AddConfigurations();
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        this.DbContextConfigurations.ToList().ForEach(m => m.BeforeModelCreation(this.DbContextConfigurations));
        this.DbContextConfigurations.ToList().ForEach(m => m.OnModelCreation(modelBuilder));
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        ConfigureDb(optionsBuilder);
        optionsBuilder.ConfigureWarnings((builder) =>
        {
            builder.Ignore(RelationalEventId.AmbientTransactionWarning);
            builder.Ignore(CoreEventId.InvalidIncludePathError);
        });
        optionsBuilder.UseLoggerFactory(loggerFactory);
    }
    protected virtual void ConfigureDb(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite(ConnectionString);
    }
    public override void Dispose()
    {
        base.Dispose();
        GC.Collect();
        GC.WaitForPendingFinalizers();
    }
}
public class UMDBContextFactory : IBurgrDBContextFactory
{
    public DatabaseInfo DatabaseInfo { get; set; }
    public IExtendedConfiguration Configuration { get; set; }
    public UMDBContextFactory(IExtendedConfiguration configuration)
    {
        this.Configuration = configuration;
        this.DatabaseInfo = configuration.BurgrConfiguration.Databases.First().Value;
    }
    public DbContext Create(IExecutionContext executionContext, ILoggerFactory loggerFactory)
    {
        return new UMContext(DatabaseInfo.ConnectionString.ReplaceLine(), executionContext, loggerFactory);
    }
    public async Task EnsureDataAccessAndMigration(IServiceProvider serviceProvider)
    {
        var executionContext = serviceProvider.GetRequiredService<IExecutionContext>();
        var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
        var dbContext = Create(executionContext, loggerFactory);
        await dbContext.Database.EnsureCreatedAsync();
        await dbContext.Database.MigrateAsync();
    }
    public async Task DeleteAllModuleData(IServiceProvider serviceProvider)
    {
        var executionContext = serviceProvider.GetRequiredService<IExecutionContext>();
        var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
        var dbContext = Create(executionContext, loggerFactory);
        await dbContext.Database.EnsureDeletedAsync();
        await dbContext.Database.EnsureCreatedAsync();
    }
}
public class UMConfiguration : IDbContextConfiguration
{
    private readonly string prefix;
    public string Name { get; set; }
    public UMConfiguration(string prefix)
    {
        this.prefix = prefix;
        this.Name = "SolidOps.UM";
    }
    public virtual void BeforeModelCreation(IEnumerable<IDbContextConfiguration> dbContextConfigurations)
    {
    }
    public virtual void OnModelCreation(ModelBuilder modelBuilder)
    {
        // Object [EN][AG]
        modelBuilder.Entity<Domain.AggregateRoots.LocalUser>((entity) =>
        {
            entity.ToTable("um_local_users");

            // Property [S][NO][NN]
            entity.Property(p => p.Name).IsRequired().HasColumnName("name");

            entity.Property(p => p.HashedPassword).IsRequired().HasColumnName("hashed_password");

            // navigation

            // 

            entity.Ignore("LazyLoadingEnabled");
        });

        modelBuilder.Entity<Domain.AggregateRoots.User>((entity) =>
        {
            entity.ToTable("um_users");

            // Property [S][NO][NN]
            entity.Property(p => p.Email).IsRequired().HasColumnName("email");

            entity.Property(p => p.Provider).IsRequired().HasColumnName("provider");

            entity.Property(p => p.TechnicalUser).IsRequired().HasColumnName("technical_user");

            // navigation

            // Property [M][R][LNA][EN][AG]
            entity.HasMany(p => p.UserRights);

            // 
            // Property [S][CA]
            entity.Ignore("Rights");

            entity.Ignore("LazyLoadingEnabled");
        });

        modelBuilder.Entity<Domain.Entities.UserRight>((entity) =>
        {
            entity.ToTable("um_user_rights");

            // Property [M][R][NO][EN][AG][NN]
            entity.Property(p => p.UserId).IsRequired().HasColumnName("user_id");
            entity.HasOne(p => p.User);

            entity.Property(p => p.RightId).IsRequired().HasColumnName("right_id");
            entity.HasOne(p => p.Right);

            // navigation

            // 

            entity.Ignore("LazyLoadingEnabled");
        });

        modelBuilder.Entity<Domain.Entities.Right>((entity) =>
        {
            entity.ToTable("um_rights");

            // Property [S][NO][NN]
            entity.Property(p => p.Name).IsRequired().HasColumnName("name");

            // navigation

            // 

            entity.Ignore("LazyLoadingEnabled");
        });

        modelBuilder.Entity<Domain.AggregateRoots.Invite>((entity) =>
        {
            entity.ToTable("um_invites");

            // Property [S][NO][NN]
            entity.Property(p => p.Email).IsRequired().HasColumnName("email");

            entity.Property(p => p.CreatorName).IsRequired().HasColumnName("creator_name");

            entity.Property(p => p.CreatorMessage).IsRequired().HasColumnName("creator_message");

            // Property [E][NO][NN]
            entity.Property(p => p.Status).IsRequired().HasColumnName("status");

            // Property [M][R][NO][EN][AG][NN]
            entity.Property(p => p.CreatorId).IsRequired().HasColumnName("creator_id");
            entity.HasOne(p => p.Creator);

            // navigation

            // 

            entity.Ignore("LazyLoadingEnabled");
        });

    }
}