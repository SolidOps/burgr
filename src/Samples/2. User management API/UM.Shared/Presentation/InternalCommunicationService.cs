using Microsoft.Extensions.Logging;
using SolidOps.UM.Shared.Contracts.Endpoints;
using SolidOps.UM.Shared.Domain.Configuration;

namespace SolidOps.UM.Shared.Presentation;

public class InternalCommunicationService : IInternalCommunicationService
{
    private readonly ILogger<InternalCommunicationService> logger;
    private readonly IExtendedConfiguration configuration;

    public InternalCommunicationService(ILoggerFactory loggerFactory, IExtendedConfiguration configuration)
    {
        this.logger = loggerFactory.CreateLogger<InternalCommunicationService>();
        this.configuration = configuration;
    }
    
    public HttpClient GetClient(string clientName, string authorization = null)
    {
        this.logger.LogDebug($"GetClient {clientName}");
        var httpClient = new HttpClient();
        httpClient.BaseAddress = new Uri(configuration.BurgrConfiguration.Endpoints[clientName]);
        if(authorization != null)
            httpClient.DefaultRequestHeaders.Add("Authorization", authorization);
        return httpClient;
    }

    public HttpMessageHandler GetMessageHandler(string clientName)
    {
        this.logger.LogDebug($"GetMessageHandler {clientName}");
        return null;
    }
}
