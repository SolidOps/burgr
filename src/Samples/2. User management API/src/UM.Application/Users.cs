using SolidOps.UM.Domain;

namespace SolidOps.UM.Application;

public partial class UserService
{
    // a user added through the API logs in with a local user of the same name, whose password it sets later
    private async partial Task<Error?> CreateLocalUserAsync(User aggregate, CancellationToken cancellationToken)
    {
        if (await LocalUserRepository.FindByNameAsync(aggregate.Email, cancellationToken) is null)
        {
            LocalUserRepository.Add(new LocalUser { Name = aggregate.Email, HashedPassword = "" });
        }
        return null;
    }
}

public partial class UserFactory
{
    public Task<Result<User>> CreateLocalAsync(string email, CancellationToken cancellationToken = default)
    {
        return Task.FromResult<Result<User>>(new User { Email = email, Provider = Accounts.LocalProvider });
    }
}

public sealed class RemoveLocalUserOfARemovedUser(ILocalUserRepository localUsers, IUnitOfWork unitOfWork) : IEventHandler<UserEvent>
{
    public async Task HandleAsync(UserEvent @event, CancellationToken cancellationToken = default)
    {
        if (@event.Context != EventContext.Remove || await localUsers.FindByNameAsync(@event.Data.Email, cancellationToken) is not { } localUser)
        {
            return;
        }

        localUsers.Remove(localUser);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
