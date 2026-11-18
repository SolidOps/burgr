using Microsoft.EntityFrameworkCore;
using SolidOps.UM.Domain;

namespace SolidOps.UM.Infrastructure;

public partial class LocalUserRepository
{
    public Task<LocalUser?> FindByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        return Db.LocalUsers.FirstOrDefaultAsync(x => x.Name == name, cancellationToken);
    }
}

public partial class InviteRepository
{
    public Task<int> CountByStatusAsync(InviteStatus status, CancellationToken cancellationToken = default)
    {
        return Db.Invites.CountAsync(x => x.Status == status, cancellationToken);
    }
}

public partial class UserRepository
{
    public Task<User?> FindByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return Db.Users
            .Include(x => x.UserRights).ThenInclude(x => x.Right)
            .FirstOrDefaultAsync(x => x.Email == email, cancellationToken);
    }
}
