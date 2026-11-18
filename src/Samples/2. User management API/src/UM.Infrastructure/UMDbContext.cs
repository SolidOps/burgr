using Microsoft.EntityFrameworkCore;
using SolidOps.UM.Domain;

namespace SolidOps.UM.Infrastructure;

public partial class UMDbContext
{
    public static readonly Guid AdminRightId = new("8e0d6a2c-34a1-4c55-9a3e-6f1d2b7c9e01");
    public static readonly Guid ManageUsersRightId = new("8e0d6a2c-34a1-4c55-9a3e-6f1d2b7c9e02");

    // the rights are fixed: no API creates them
    partial void OnModelCreatingPartial(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Right>().HasData(
            new Right { Id = AdminRightId, Name = "Admin" },
            new Right { Id = ManageUsersRightId, Name = "ManageUsers" });
    }
}
