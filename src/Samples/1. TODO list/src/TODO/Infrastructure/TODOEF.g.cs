using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SolidOps.TODO.Shared.Domain;
using SolidOps.TODO.Shared.Infrastructure;
using SolidOps.SubZero;
using SolidOps.TODO.Shared;
using SolidOps.TODO.Domain.Repositories;
namespace SolidOps.TODO.Infrastructure;
public partial class TODOContext : DbContext
{
    public string ConnectionString { get; set; }
    protected List<IDbContextConfiguration> DbContextConfigurations { get; private set; }
    private readonly IUserContext userContext;
    private readonly IInterceptor requestInterceptor;
    public TODOContext(string connectionString, IUserContext userContext)
    {
        ConnectionString = connectionString;
        this.userContext = userContext;
        DbContextConfigurations = new List<IDbContextConfiguration>()
        {
            new TODOConfiguration("")
        };
        AddConfigurations();
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
public class TODODBContextFactory : IDBContextFactory
{
    public IConfiguration Configuration { get; set; }
    public TODODBContextFactory(IConfiguration configuration)
    {
        this.Configuration = configuration;
    }
    public DbContext Create(IUserContext userContext)
    {
        var connectionString = Configuration["ConnectionString"];
        return new TODOContext(connectionString.ReplaceLine(), userContext);
    }
    public async Task EnsureDataAccessAndMigration(IServiceProvider serviceProvider)
    {
        var userContext = serviceProvider.GetRequiredService<IUserContext>();
        var dbContext = Create(userContext);
        await dbContext.Database.EnsureCreatedAsync();
        await dbContext.Database.MigrateAsync();
    }
    public async Task DeleteAllModuleData(IServiceProvider serviceProvider)
    {
        var userContext = serviceProvider.GetRequiredService<IUserContext>();
        var dbContext = Create(userContext);
        await dbContext.Database.EnsureDeletedAsync();
        await dbContext.Database.EnsureCreatedAsync();
    }
}
public class TODOConfiguration : IDbContextConfiguration
{
    private readonly string prefix;
    public string Name { get; set; }
    public TODOConfiguration(string prefix)
    {
        this.prefix = prefix;
        this.Name = "SolidOps.TODO";
    }
    public virtual void BeforeModelCreation(IEnumerable<IDbContextConfiguration> dbContextConfigurations)
    {
    }
    public virtual void OnModelCreation(ModelBuilder modelBuilder)
    {
        // Object [EN][AG]
        modelBuilder.Entity<Domain.AggregateRoots.Item>((entity) =>
        {
            entity.ToTable("tod_items");

            // Property [S][NO][NN]
            entity.Property(p => p.Name).IsRequired().HasColumnName("name");

            entity.Property(p => p.DueDate).IsRequired().HasColumnName("due_date");

            // Property [E][NO][NN]
            entity.Property(p => p.Status).IsRequired().HasColumnName("status");

            // navigation

            // 
            // Property [S][CA]
            entity.Ignore("RemainingDays");

        });

    }
}