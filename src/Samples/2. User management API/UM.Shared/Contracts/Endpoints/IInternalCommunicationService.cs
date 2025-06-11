namespace SolidOps.UM.Shared.Contracts.Endpoints;

public interface IInternalCommunicationService
{
    HttpClient GetClient(string clientName, string authorization = null);
    HttpMessageHandler GetMessageHandler(string clientName);
}
