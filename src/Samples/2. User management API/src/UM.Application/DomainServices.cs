using Microsoft.Extensions.DependencyInjection;
using SolidOps.UM.Domain;

namespace SolidOps.UM.Application;

public partial class AuthenticationService
{
    public async Task<Result<string>> LoginAsync(LoginRequestDto request, CancellationToken cancellationToken = default)
    {
        var localUser = await LocalUserRepository.FindByNameAsync(request.Login, cancellationToken);
        if (localUser is null || !Passwords.Verify(request.Password, localUser.HashedPassword))
        {
            return Error.Forbidden("Unknown login or wrong password");
        }

        var user = await UserRepository.FindByEmailAsync(request.Login, cancellationToken);
        var rights = user?.UserRights.Select(r => r.Right!.Name) ?? [];
        return Services.GetRequiredService<ITokenIssuer>().Issue(localUser.Name, rights);
    }

    public async Task<Result> SetInitialPasswordAsync(LoginRequestDto request, CancellationToken cancellationToken = default)
    {
        var localUser = await LocalUserRepository.FindByNameAsync(request.Login, cancellationToken);
        if (localUser is null)
        {
            return Error.NotFound();
        }
        if (localUser.HashedPassword.Length > 0)
        {
            return Error.Conflict("The password is already set");
        }

        localUser.HashedPassword = Passwords.Hash(request.Password);
        return await UnitOfWork.SaveChangesAsync(cancellationToken) is { } error ? error : Result.Success();
    }

    public async Task<Result<bool>> NeedInitialPasswordAsync(string email, CancellationToken cancellationToken = default)
    {
        var localUser = await LocalUserRepository.FindByNameAsync(email, cancellationToken);
        return localUser is null ? Error.NotFound() : localUser.HashedPassword.Length == 0;
    }
}

public partial class InvitesService
{
    public async Task<Result<InviteResultDto>> CheckInviteAsync(Guid inviteId, CancellationToken cancellationToken = default)
    {
        var invite = await InviteRepository.FindAsync(inviteId, cancellationToken);
        if (invite is null || !invite.IsOpen)
        {
            return Error.NotFound();
        }
        return new InviteResultDto { Email = invite.Email, Creator = invite.CreatorName, Message = invite.CreatorMessage };
    }

    public async Task<Result> UseInviteAsync(InviteUseRequestDto request, CancellationToken cancellationToken = default)
    {
        var invite = await InviteRepository.FindAsync(request.InviteId, cancellationToken);
        if (invite is null || !invite.IsOpen)
        {
            return Error.NotFound();
        }

        invite.Status = InviteStatus.Used;
        var created = await Accounts.CreateAsync(Services, UserRepository, LocalUserRepository, UnitOfWork, invite.Email, request.Password, [], cancellationToken);
        if (!created.IsSuccess)
        {
            return created.Error!;
        }

        var result = new InviteResultDto { Email = invite.Email, Creator = invite.CreatorName, Message = invite.CreatorMessage };
        await Services.GetRequiredService<IEventPublisher>().PublishAsync(new InviteUsedEvent("used", result), cancellationToken);
        return Result.Success();
    }

    public async Task<Result<int>> CountByStatusAsync(InviteStatus status, CancellationToken cancellationToken = default)
    {
        return await InviteRepository.CountByStatusAsync(status, cancellationToken);
    }
}

public partial class SelfUserCreationService
{
    public Task<Result<Guid>> CreateUserAsync(SelfUserCreationRequestDto request, CancellationToken cancellationToken = default)
    {
        return Accounts.CreateAsync(Services, UserRepository, LocalUserRepository, UnitOfWork, request.Email, request.Password, [], cancellationToken);
    }
}

public partial class ServerStatusService
{
    public async Task<Result<bool>> NeedTechUserPasswordUpdateAsync(string techUser, CancellationToken cancellationToken = default)
    {
        var localUser = await LocalUserRepository.FindByNameAsync(techUser, cancellationToken);
        return localUser is null ? Error.NotFound() : localUser.HashedPassword.Length == 0;
    }
}

public partial class TokenValidationService
{
    public Task<Result<string>> ValidateAsync(CancellationToken cancellationToken = default)
    {
        return Task.FromResult<Result<string>>(CurrentUser.Name!);
    }
}

public partial class UserCreationService
{
    public async Task<Result<Guid>> CreateUserAsync(UserCreationInfoDto userCreationInfo, CancellationToken cancellationToken = default)
    {
        var errors = new Dictionary<string, string[]>();
        for (var i = 0; i < userCreationInfo.Rights.Count; i++)
        {
            if (!await UnitOfWork.ExistsAsync<Right>(userCreationInfo.Rights[i].RightId, cancellationToken))
            {
                errors[$"Rights[{i}].RightId"] = ["Right not found"];
            }
        }
        if (errors.Count > 0)
        {
            return Error.Invalid(errors);
        }

        var rightIds = userCreationInfo.Rights.Select(r => r.RightId);
        return await Accounts.CreateAsync(Services, UserRepository, LocalUserRepository, UnitOfWork, userCreationInfo.UserEmail, userCreationInfo.Password, rightIds, cancellationToken);
    }
}
