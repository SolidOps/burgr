using Microsoft.AspNetCore.Identity;
using SolidOps.Burgr.Shared.Contracts.Results;
using SolidOps.Burgr.Shared.Domain.Configuration;
using SolidOps.UM.Domain.AggregateRoots;
using SolidOps.UM.Domain.Services;
using SolidOps.UM.Domain.Transients;

namespace SolidOps.UM.Application.Services;

public partial interface ISelfUserCreationService
{
    Task<IOpsResult<Guid>> SafeCreateUser(string email, string password);
}

public partial class SelfUserCreationService
{
    public override async Task<IOpsResult<Guid>> CreateUser(SelfUserCreationRequest request)
    {
        var configuration = this.executionScope.GetRequiredService<IExtendedConfiguration>();
        if (configuration == null || !configuration.NORADConfiguration.EnableSelfUserCreation)
        {
            return IOpsResult.Invalid("Feature disabled").ToResult<Guid>();
        }

        return await SafeCreateUser(request.Email, request.Password);
    }

    public async Task<IOpsResult<Guid>> SafeCreateUser(string email, string password)
    {
        var user = User.Create(email, typeof(LocalIdentityProviderService).Name, false);
        var result = await _dependencyUserRepository.Add(user);
        if (result.HasError) return result;

        var hasherService = this.executionScope.GetRequiredService<IPasswordHasher<LocalUser>>();

        LocalUser localUser = LocalUser.Create(email, string.Empty);
        localUser.HashedPassword = hasherService.HashPassword(localUser, password);
        result = await this._dependencyLocalUserRepository.Add(localUser);
        if (result.HasError) return result;

        user = await _dependencyUserRepository.GetSingleByEmail(user.Email);
        var token = _dependencyUserRepository.CreateToken(user);

        executionScope.TemporaryData.Add(executionScope.TEMPORARY_DATA_TOKEN, token);
        return IOpsResult.Ok(user.Id);
    }

}
