using Microsoft.AspNetCore.Identity;
using SolidOps.UM.Shared.Contracts.Results;
using SolidOps.UM.Domain.AggregateRoots;
using SolidOps.UM.Domain.Repositories;

namespace SolidOps.UM.Domain.Services;

public class LocalIdentityProviderService : IIdentityProviderService
{
    private readonly ILocalUserRepository localUserRepository;
    private readonly IPasswordHasher<LocalUser> hasher;

    public LocalIdentityProviderService(ILocalUserRepository localUserRepository, IPasswordHasher<LocalUser> hasher)
    {
        this.localUserRepository = localUserRepository;
        this.hasher = hasher;
    }
    public async Task<IOpsResult<bool>> Validate(string login, string password)
    {
        var localUser = await this.localUserRepository.GetSingleByName(login);
        if (localUser != null)
        {
            if(localUser.HashedPassword == string.Empty)
            {
                return IOpsResult.Invalid("A password for this user must be set first").ToResult<bool>();
            }
            var result = hasher.VerifyHashedPassword(localUser, localUser.HashedPassword, password);
            return IOpsResult.Ok(result == PasswordVerificationResult.Success);
        }
        else
        {
            return IOpsResult.Invalid("user not found").ToResult<bool>();
        }
    }
}
