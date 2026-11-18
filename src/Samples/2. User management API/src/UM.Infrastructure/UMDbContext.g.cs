#nullable enable
using Microsoft.EntityFrameworkCore;
namespace SolidOps.UM.Infrastructure;
public partial class UMDbContext(DbContextOptions<UMDbContext> options) : DbContext(options)
{
    // Object [AG][EN]
    public DbSet<Domain.LocalUser> LocalUsers => Set<Domain.LocalUser>();

    public DbSet<Domain.User> Users => Set<Domain.User>();

    public DbSet<Domain.UserRight> UserRights => Set<Domain.UserRight>();

    public DbSet<Domain.Right> Rights => Set<Domain.Right>();

    public DbSet<Domain.Invite> Invites => Set<Domain.Invite>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Object [AG][EN]
        modelBuilder.Entity<Domain.LocalUser>(builder =>
        {
            builder.HasKey(x => x.Id);
            // Property [S][NO][NAR]
            // VALIDATION RULE - PROPERTY_HAS_MAX_SIZE
            builder.Property(x => x.Name).HasMaxLength(50);

            // VALIDATION RULE - PROPERTY_HAS_MAX_SIZE
            builder.Property(x => x.HashedPassword).HasMaxLength(255);

            // Property [S][NO][NAR]
            // VALIDATION RULE - PROPERTY_IS_UNIQUE
            builder.HasIndex(x => x.Name).IsUnique();

        });

        modelBuilder.Entity<Domain.User>(builder =>
        {
            builder.HasKey(x => x.Id);
            // Property [S][NO][NAR]
            // VALIDATION RULE - PROPERTY_HAS_MAX_SIZE
            builder.Property(x => x.Email).HasMaxLength(50);

            // VALIDATION RULE - PROPERTY_HAS_MAX_SIZE
            builder.Property(x => x.Provider).HasMaxLength(50);

            // Property [S][NO][NAR]
            // VALIDATION RULE - PROPERTY_IS_UNIQUE
            builder.HasIndex(x => x.Email).IsUnique();

            // Property [M][NO][NAR][VO]
            builder.Property(x => x.Preferences).HasJsonConversion();

        });

        modelBuilder.Entity<Domain.UserRight>(builder =>
        {
            builder.HasKey(x => x.Id);

            // MultipleUniqueQueryableProperty 
            builder.HasIndex(entity => new { entity.UserId, entity.RightId }).IsUnique();

        });

        modelBuilder.Entity<Domain.Right>(builder =>
        {
            builder.HasKey(x => x.Id);
            // Property [S][NO][NAR]
            // VALIDATION RULE - PROPERTY_HAS_MAX_SIZE
            builder.Property(x => x.Name).HasMaxLength(50);

            // Property [S][NO][NAR]
            // VALIDATION RULE - PROPERTY_IS_UNIQUE
            builder.HasIndex(x => x.Name).IsUnique();

        });

        modelBuilder.Entity<Domain.Invite>(builder =>
        {
            builder.HasKey(x => x.Id);
            // Property [S][NO][NAR]
            // VALIDATION RULE - PROPERTY_HAS_MAX_SIZE
            builder.Property(x => x.Email).HasMaxLength(255);

            // VALIDATION RULE - PROPERTY_HAS_MAX_SIZE
            builder.Property(x => x.CreatorName).HasMaxLength(255);

            // VALIDATION RULE - PROPERTY_HAS_MAX_SIZE
            builder.Property(x => x.CreatorMessage).HasMaxLength(255);

            // Property [S][NO][NAR]

            // Property [E][NO][NAR]
            builder.Property(x => x.Status).HasConversion<string>();

            // Property [S][CA][NAR]
            builder.Ignore(x => x.IsOpen);

        });

        OnModelCreatingPartial(modelBuilder);
        modelBuilder.ApplyDeleteRules();
    }
    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}