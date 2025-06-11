using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using SolidOps.UM.Shared.Contracts.Results;
using SolidOps.UM.Domain.AggregateRoots;

namespace SolidOps.UM.Infrastructure.Repositories;

public partial class LocalUserRepository
{
    public async Task<IOpsResult> UpdatePassword(LocalUser entity, string password)
    {
        var hasherService = this.serviceProvider.GetRequiredService<IPasswordHasher<LocalUser>>();
        entity.HashedPassword = hasherService.HashPassword(entity, password);
        return await Update(entity);
    }
}
