using Microsoft.EntityFrameworkCore;

namespace SolidOps.TODO.Shared.Infrastructure;

public interface IDbContextConfiguration
{
    string Name { get; set; }
    void BeforeModelCreation(IEnumerable<IDbContextConfiguration> configurations);
    void OnModelCreation(ModelBuilder modelBuilder);

}
