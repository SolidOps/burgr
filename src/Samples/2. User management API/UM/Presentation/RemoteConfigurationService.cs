using SolidOps.Burgr.Shared.Contracts.Endpoints;
using SolidOps.Burgr.Shared.Contracts.Results;
using SolidOps.Burgr.Shared.Domain.Configuration;
using SolidOps.SubZero;
using SolidOps.UM.Contracts.Endpoints;

namespace SolidOps.UM.API;

internal class RemoteConfigurationService : IRemoteConfigurationService
{
    private readonly IInternalCommunicationService internalCommunicationService;

    public RemoteConfigurationService(IInternalCommunicationService internalCommunicationService)
    {
        this.internalCommunicationService = internalCommunicationService;
    }
    public async Task<IOpsResult<Dictionary<string, string>>> GetRemoteConfiguration(string application, string environment, string token)
    {
        var client = UMServiceAccess.CreateClient(internalCommunicationService, token);
        var result = await client.UMFacade_Configurations_GetConfiguration(application, environment);
        if (result.HasError) return result.ToResult<Dictionary<string, string>>();

        return IOpsResult.Ok(Serializer.Deserialize<Dictionary<string, string>>(result.Data));
    }
}
