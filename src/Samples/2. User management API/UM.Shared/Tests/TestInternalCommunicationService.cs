using Microsoft.Extensions.Logging;
using SolidOps.UM.Shared.Contracts.Endpoints;

namespace SolidOps.UM.Shared.Tests;

public class TestInternalCommunicationService : IInternalCommunicationService
{
    private readonly Dictionary<string, AppService> factories;
    private readonly ILogger<TestInternalCommunicationService> logger;

    public TestInternalCommunicationService(Dictionary<string, AppService> factories, ILoggerFactory loggerFactory)
    {
        this.factories = factories;
        this.logger = loggerFactory.CreateLogger<TestInternalCommunicationService>();
    }

    public HttpClient GetClient(string clientName, string authorization = null)
    {
        this.logger.LogDebug($"GetClient {clientName}");
        var httpClient = factories[clientName].CreateHttpClient();
        if (authorization != null)
            httpClient.DefaultRequestHeaders.Add("Authorization", authorization);

        return httpClient;
    }

    public HttpMessageHandler GetMessageHandler(string clientName)
    {
        this.logger.LogDebug($"GetMessageHandler {clientName}");
        return factories[clientName].CreateHandler();
    }
}
