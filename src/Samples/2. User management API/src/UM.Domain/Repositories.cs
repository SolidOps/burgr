namespace SolidOps.UM.Domain;

public partial interface ILocalUserRepository
{
    Task<LocalUser?> FindByNameAsync(string name, CancellationToken cancellationToken = default);
}

public partial interface IInviteRepository
{
    Task<int> CountByStatusAsync(InviteStatus status, CancellationToken cancellationToken = default);
}

public partial interface IUserRepository
{
    /// <summary>The user with its rights and their names.</summary>
    Task<User?> FindByEmailAsync(string email, CancellationToken cancellationToken = default);
}
