using SolidOps.UM.Shared.Contracts.Endpoints;
using SolidOps.UM.Shared.Contracts.Results;
using SolidOps.UM.Contracts.Endpoints;

namespace SolidOps.UM.Contracts;

public class ApplicationToken : IApplicationToken
{
    private string token;
    private readonly IInternalCommunicationService internalCommunicationService;

    public string Token => token;

    public ApplicationToken(IInternalCommunicationService internalCommunicationService)
    {
        this.internalCommunicationService = internalCommunicationService;
    }

    public async Task<IOpsResult> Login(string appName, string password)
    {
        var client = UMServiceAccess.CreateClient(internalCommunicationService, null);
        var needResult = await client.UMFacade_ServerStatus_NeedTechUserPasswordUpdate(appName);
        if(needResult.HasError) return needResult;
        if (needResult.Data) // need tech user password update
        {
            var result = await client.UMFacade_Authentication_SetInitialPassword(appName, password);
            if (result.HasError) return result;
        }
        var loginResult = await client.Login(appName, password);
        if (loginResult.HasError) return loginResult;
        IEnumerable<string> values;
        if (client.HttpClient.DefaultRequestHeaders.TryGetValues("Authorization", out values))
        {
            token = values.First();
        }
        return IOpsResult.Ok();
    }
}
