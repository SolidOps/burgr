#nullable enable

using Microsoft.EntityFrameworkCore;

namespace MetaCorp.Template.Infrastructure;

public partial class TemplateDbContext(DbContextOptions<TemplateDbContext> options) : DbContext(options)
{
    #region foreach MODEL[AG]
    public DbSet<Domain.CLASSNAME> _CLASSNAMES_ => Set<Domain.CLASSNAME>();
    #endregion foreach MODEL

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        #region foreach MODEL[AG]
        modelBuilder.Entity<Domain.CLASSNAME>(entity =>
        {
            entity.HasKey(x => x.Id);
            #region foreach PROPERTY[S][NO][NAR]
            // VALIDATION RULE - PROPERTY_HAS_MAX_SIZE
            entity.Property(x => x._SIMPLE__PROPERTYNAME_).HasMaxLength(FIELDSIZE);
            #endregion foreach PROPERTY
            #region foreach PROPERTY[S][NO][NAR]
            // VALIDATION RULE - PROPERTY_IS_UNIQUE
            entity.HasIndex(x => x._SIMPLE__PROPERTYNAME_).IsUnique();
            #endregion foreach PROPERTY
            #region foreach PROPERTY[E][NO][NAR]
            entity.Property(x => x._PROPERTYNAME_).HasConversion<string>();
            #endregion foreach PROPERTY
            #region foreach PROPERTY[S][CA][NAR]
            entity.Ignore(x => x._PROPERTYNAME_);
            #endregion foreach PROPERTY
            #region foreach PROPERTY[E][CA][NAR]
            entity.Ignore(x => x._PROPERTYNAME_);
            #endregion foreach PROPERTY
        });
        #endregion foreach MODEL

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
