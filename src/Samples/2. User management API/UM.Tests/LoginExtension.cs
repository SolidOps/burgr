using SolidOps.UM.Shared.Contracts.Endpoints;
using SolidOps.SubZero;
using SolidOps.UM.Contracts.DTO;
using System.Text;

namespace SolidOps.UM.Tests;

public static class LoginExtension
{
    public static async Task<(string userId, string authorization)> CreateSelfUser(this AppServiceClient session, SelfUserCreationRequestDTO dto)
    {
        string uri = "UM/self-user-creation/create-user";

        var stringContent = new StringContent(Serializer.Serialize(dto), Encoding.UTF8, "application/json");
        var response = await session.HttpClient.PostAsync(uri, stringContent);
        response.EnsureSuccessStatusCode();

        return (response.Headers.GetValues("Location").First(), response.Headers.GetValues("Authorization").First());
    }
}