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
using MetaCorp.Template.Domain.Repositories;

namespace MetaCorp.Template.Infrastructure.EF;

public partial interface ITemplateEFDataAccessFactory : ITemplateDataAccessFactory, IEFDataAccessFactory
{
}

public partial class TemplateEFServiceRegistrar : IServiceRegistrar
{
    public int Priority => 2;
    public void ConfigureServices(IServiceCollection services, IExtendedConfiguration configuration)
    {
        services.AddScopedFromExisting<ITemplateEFDataAccessFactory, ITemplateDataAccessFactory>();
    }
}

public partial class TemplateContext : DbContext
{
    public string ConnectionString { get; set; }

    protected List<IDbContextConfiguration> DbContextConfigurations { get; private set; }

    private readonly ILoggerFactory loggerFactory;
    private readonly IExecutionContext executionContext;
    private readonly IInterceptor requestInterceptor;

    public TemplateContext(string connectionString, IExecutionContext executionContext, ILoggerFactory loggerFactory)
    {
        ConnectionString = connectionString;
        this.executionContext = executionContext;
        DbContextConfigurations = new List<IDbContextConfiguration>()
        {
            new TemplateConfiguration("")
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

public class TemplateDBContextFactory : IBurgrDBContextFactory
{
    public DatabaseInfo DatabaseInfo { get; set; }
    public IExtendedConfiguration Configuration { get; set; }

    public TemplateDBContextFactory(IExtendedConfiguration configuration)
    {
        this.Configuration = configuration;
        this.DatabaseInfo = configuration.BurgrConfiguration.Databases.First().Value;
    }

    public DbContext Create(IExecutionContext executionContext, ILoggerFactory loggerFactory)
    {
        return new TemplateContext(DatabaseInfo.ConnectionString.ReplaceLine(), executionContext, loggerFactory);
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


public class TemplateConfiguration : IDbContextConfiguration
{
    private readonly string prefix;

    public string Name { get; set; }

    public TemplateConfiguration(string prefix)
    {
        this.prefix = prefix;
        this.Name = "MetaCorp.Template";
    }

    public virtual void BeforeModelCreation(IEnumerable<IDbContextConfiguration> dbContextConfigurations)
    {

    }

    public virtual void OnModelCreation(ModelBuilder modelBuilder)
    {
        #region foreach MODEL[EN][AG]

        modelBuilder.Entity<Domain._DOMAINTYPE_.CLASSNAME>((entity) =>
        {
            entity.ToTable("FULLMYSQLTABLE");
            #region foreach PROPERTY[S][NO][N]
            entity.Property(p => p._SIMPLE__PROPERTYNAME_).IsRequired(false).HasColumnName("_COLUMNNAME_");
            #endregion foreach PROPERTY

            #region foreach PROPERTY[S][NO][NN]
            entity.Property(p => p._SIMPLE__PROPERTYNAME_).IsRequired().HasColumnName("_COLUMNNAME_");
            #endregion foreach PROPERTY

            #region foreach PROPERTY[E][NO][N]
            entity.Property(p => p._ENUM__PROPERTYNAME_).IsRequired(false).HasColumnName("_COLUMNNAME_");
            #endregion foreach PROPERTY

            #region foreach PROPERTY[E][NO][NN]
            entity.Property(p => p._ENUM__PROPERTYNAME_).IsRequired().HasColumnName("_COLUMNNAME_");
            #endregion foreach PROPERTY

            #region foreach PROPERTY[M][R][NO][EN][AG][N]
            entity.Property(p => p._PROPERTYNAME_Id).IsRequired(false).HasColumnName("_COLUMNNAME_");
            entity.HasOne(p => p._PROPERTYNAME_);
            #endregion foreach PROPERTY

            #region foreach PROPERTY[M][R][NO][EN][AG][NN]
            entity.Property(p => p._PROPERTYNAME_Id).IsRequired().HasColumnName("_COLUMNNAME_");
            entity.HasOne(p => p._PROPERTYNAME_);
            #endregion foreach PROPERTY

            #region foreach PROPERTY[M][R][NO][VO][N][NAR]
            entity.Property(p => p._VO__PROPERTYNAME_).IsRequired(false).HasColumnName("_COLUMNNAME_")
                    .HasConversion<ValueObjectValueComparer<Domain._DOMAINTYPE_._PROPERTYTYPE_>>();
            #endregion foreach PROPERTY

            #region foreach PROPERTY[M][R][NO][VO][N][AR]
            entity.Property(p => p._VO__PROPERTYNAME_).IsRequired(false).HasColumnName("_COLUMNNAME_")
                    .HasConversion<ValueObjectValueComparer<List<Domain._DOMAINTYPE_._PROPERTYTYPE_>>>();
            #endregion foreach PROPERTY

            #region foreach PROPERTY[M][R][NO][VO][NN][NAR]
            entity.Property(p => p._VO__PROPERTYNAME_).IsRequired().HasColumnName("_COLUMNNAME_")
                    .HasConversion<ValueObjectValueComparer<Domain._DOMAINTYPE_._PROPERTYTYPE_>>();
            #endregion foreach PROPERTY

            #region foreach PROPERTY[M][R][NO][VO][NN][AR]
            entity.Property(p => p._VO__PROPERTYNAME_).IsRequired().HasColumnName("_COLUMNNAME_")
                    .HasConversion<ValueObjectValueComparer<List<Domain._DOMAINTYPE_._PROPERTYTYPE_>>>();
            #endregion foreach PROPERTY

            // navigation
            #region foreach PROPERTY[M][R][SNA]
            entity.HasOne(p => p._NAVIGATION__PROPERTYNAME_);
            #endregion foreach PROPERTY

            #region foreach PROPERTY[M][R][LNA][EN][AG]
            entity.HasMany(p => p._NAVIGATION__NAVIGATION__PROPERTYNAME_);
            #endregion foreach PROPERTY

            // #region foreach PROPERTY[M][R][LNA][VO]
            // entity.Property("_VO__NAVIGATION__NAVIGATION__PROPERTYNAME_").HasColumnName("_COLUMNNAME_")
            //       .HasConversion<ValueObjectValueComparer<Domain._DOMAINTYPE_._PROPERTYTYPE_>>();
            // #endregion foreach PROPERTY

            #region foreach PROPERTY[S][CA]
            entity.Ignore("_PROPERTYNAME_");
            #endregion foreach PROPERTY

            #region foreach PROPERTY[E][CA]
            entity.Ignore("_PROPERTYNAME_");
            #endregion foreach PROPERTY

            #region foreach PROPERTY[M][R][CA]
            entity.Ignore("_PROPERTYNAME_");
            #endregion foreach PROPERTY

            entity.Ignore("LazyLoadingEnabled");
        });

        #endregion foreach MODEL
    }
}