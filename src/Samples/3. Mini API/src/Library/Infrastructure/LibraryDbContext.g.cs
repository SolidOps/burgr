#nullable enable
using Microsoft.EntityFrameworkCore;
namespace Acme.Library.Infrastructure;
public partial class LibraryDbContext(DbContextOptions<LibraryDbContext> options) : DbContext(options)
{
    // Object [AG]
    public DbSet<Domain.Author> Authors => Set<Domain.Author>();

    public DbSet<Domain.Book> Books => Set<Domain.Book>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Object [AG]
        modelBuilder.Entity<Domain.Author>(entity =>
        {
            entity.HasKey(x => x.Id);
            // Property [S][NO][NAR]
            // VALIDATION RULE - PROPERTY_HAS_MAX_SIZE
            entity.Property(x => x.Name).HasMaxLength(100);

            // VALIDATION RULE - PROPERTY_HAS_MAX_SIZE
            entity.Property(x => x.Biography).HasMaxLength(255);

            // Property [S][NO][NAR]
            // VALIDATION RULE - PROPERTY_IS_UNIQUE
            entity.HasIndex(x => x.Name).IsUnique();

        });

        modelBuilder.Entity<Domain.Book>(entity =>
        {
            entity.HasKey(x => x.Id);
            // Property [S][NO][NAR]
            // VALIDATION RULE - PROPERTY_HAS_MAX_SIZE
            entity.Property(x => x.Title).HasMaxLength(200);

            // VALIDATION RULE - PROPERTY_HAS_MAX_SIZE
            entity.Property(x => x.Isbn).HasMaxLength(20);

            // Property [S][NO][NAR]
            // VALIDATION RULE - PROPERTY_IS_UNIQUE
            entity.HasIndex(x => x.Isbn).IsUnique();

            // Property [E][NO][NAR]
            entity.Property(x => x.Status).HasConversion<string>();

            // Property [S][CA][NAR]
            entity.Ignore(x => x.IsAvailable);

        });

        OnModelCreatingPartial(modelBuilder);
    }
    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}