#nullable enable

using Microsoft.EntityFrameworkCore;

namespace _MODULE_NAMESPACE_.Infrastructure;

public partial class _MODULE_DbContext(DbContextOptions<_MODULE_DbContext> options) : DbContext(options)
{
    #region foreach MODEL[AG][EN]
    public DbSet<Domain.CLASSNAME> _CLASSNAMES_ => Set<Domain.CLASSNAME>();
    #endregion foreach MODEL

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        #region foreach MODEL[AG][EN]
        modelBuilder.Entity<Domain.CLASSNAME>(builder =>
        {
            builder.HasKey(x => x.Id);
            #region foreach PROPERTY[S][NO][NAR]
            // VALIDATION RULE - PROPERTY_HAS_MAX_SIZE
            builder.Property(x => x._SIMPLE__PROPERTYNAME_).HasMaxLength(FIELDSIZE);
            #endregion foreach PROPERTY
            #region foreach PROPERTY[S][NO][NAR]
            // VALIDATION RULE - PROPERTY_IS_UNIQUE
            builder.HasIndex(x => x._SIMPLE__PROPERTYNAME_).IsUnique();
            #endregion foreach PROPERTY
            #region foreach MULTIPLE_UNIQUE_QUERYABLE_PROPERTY
            builder.HasIndex(entity => new { _CONSTRAINT_VALUES_ }).IsUnique();
            #endregion foreach MULTIPLE_UNIQUE_QUERYABLE_PROPERTY
            #region foreach PROPERTY[E][NO][NAR]
            builder.Property(x => x._PROPERTYNAME_).HasConversion<string>();
            #endregion foreach PROPERTY
            #region foreach PROPERTY[M][NO][NAR][VO]
            builder.Property(x => x._PROPERTYNAME_).HasJsonConversion();
            #endregion foreach PROPERTY
            #region foreach PROPERTY[M][NO][AR][VO]
            builder.Property(x => x._PROPERTYNAME_).HasJsonConversion();
            #endregion foreach PROPERTY
            #region foreach PROPERTY[S][CA][NAR]
            builder.Ignore(x => x._PROPERTYNAME_);
            #endregion foreach PROPERTY
            #region foreach PROPERTY[E][CA][NAR]
            builder.Ignore(x => x._PROPERTYNAME_);
            #endregion foreach PROPERTY
        });
        #endregion foreach MODEL

        OnModelCreatingPartial(modelBuilder);
        modelBuilder.ApplyDeleteRules();
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
