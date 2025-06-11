using SolidOps.UM.Shared.Contracts.Results;
using SolidOps.UM.Shared.Domain.UnitOfWork;
using SolidOps.UM.Domain.Services;
using SolidOps.UM.Domain.Transients;

namespace SolidOps.UM.Application.UseCases;

public partial class AuthenticationUseCase
{
    public override async Task<IOpsResult> Login(LoginRequest authentication)
    {
        var user = await this._dependencyUserRepository.GetSingleByEmail(authentication.Login, "Lazy");
        if (user == null)
            return IOpsResult.Invalid("Bad password");

        var services = GetService<IEnumerable<IIdentityProviderService>>();
        var provider = services.Where(s => s.GetType().Name == user.Provider).First();

        var result = await provider.Validate(authentication.Login, authentication.Password);
        if(result.HasError) return result;

        if (result.Data)
        {
            executionContext.TemporaryData.Add(IExecutionContext.TEMPORARY_DATA_TOKEN, _dependencyUserRepository.CreateToken(user));
        }
        else
        {
            return IOpsResult.Invalid("Bad password");
        }
        return IOpsResult.Ok();
    }

    public override async Task<IOpsResult<bool>> NeedInitialPassword(string email)
    {
        var localUser = await _dependencyLocalUserRepository.GetSingleByName(email);
        return IOpsResult.Ok(localUser.HashedPassword == string.Empty);
    }

    public override async Task<IOpsResult> SetInitialPassword(string email, string password)
    {
        var localUser = await _dependencyLocalUserRepository.GetSingleByName(email);
        var user = await this._dependencyUserRepository.GetSingleByEmail(email, "UserRoles|UserOrganisations");
        if (localUser.HashedPassword != string.Empty)
        {
            return IOpsResult.Invalid("init already done");
        }

        var result = await _dependencyLocalUserRepository.UpdatePassword(localUser, password);
        if (result.HasError) return result;
        
        this.executionContext.SetUserId(() =>
        {
            return (user.Id.ToString(), user.Rights, null);
        });

        return IOpsResult.Ok();
    }
}
