using SolidOps.UM.Shared.Contracts.Endpoints;
using SolidOps.UM.Shared.Contracts.Results;
using SolidOps.SubZero;
using SolidOps.UM.Contracts.DTO;
using System.Text;

namespace SolidOps.UM.Contracts;

public static class LoginExtension
{
    public static async Task<IOpsResult> Login(this AppServiceClient session, string username, string password)
    {
        string uri = "um/authentication/login";
        LoginRequestDTO dto = new LoginRequestDTO();
        dto.Login = username;
        dto.Password = password;

        var stringContent = new StringContent(Serializer.Serialize(dto), Encoding.UTF8, "application/json");
        var response = await session.HttpClient.PostAsync(uri, stringContent);
        if (!response.IsSuccessStatusCode)
        {
            return IOpsResult.Invalid("Login failed");
        }

        session.HttpClient.DefaultRequestHeaders.Add("Authorization", response.Headers.GetValues("Authorization"));

        return IOpsResult.Ok();
    }
}
