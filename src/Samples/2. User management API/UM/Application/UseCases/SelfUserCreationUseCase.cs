using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using SolidOps.UM.Shared.Contracts.Results;
using SolidOps.UM.Shared.Domain.Configuration;
using SolidOps.UM.Shared.Domain.UnitOfWork;
using SolidOps.UM.Domain.AggregateRoots;
using SolidOps.UM.Domain.Services;
using SolidOps.UM.Domain.Transients;

namespace SolidOps.UM.Application.UseCases;

public partial interface ISelfUserCreationUseCase
{
    Task<IOpsResult<Guid>> SafeCreateUser(string email, string password);
}

public partial class SelfUserCreationUseCase
{
    public override async Task<IOpsResult<Guid>> CreateUser(SelfUserCreationRequest request)
    {
        var configuration = this.serviceProvider.GetRequiredService<IExtendedConfiguration>();
        if (configuration == null || !configuration.BurgrConfiguration.EnableSelfUserCreation)
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

        var hasherService = this.serviceProvider.GetRequiredService<IPasswordHasher<LocalUser>>();

        LocalUser localUser = LocalUser.Create(email, string.Empty);
        localUser.HashedPassword = hasherService.HashPassword(localUser, password);
        result = await this._dependencyLocalUserRepository.Add(localUser);
        if (result.HasError) return result;

        user = await _dependencyUserRepository.GetSingleByEmail(user.Email);
        var token = _dependencyUserRepository.CreateToken(user);

        executionContext.TemporaryData.Add(IExecutionContext.TEMPORARY_DATA_TOKEN, token);
        return IOpsResult.Ok(user.Id);
    }

}
