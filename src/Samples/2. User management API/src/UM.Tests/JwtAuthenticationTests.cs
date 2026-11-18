using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using SolidOps.UM.Application;

namespace SolidOps.UM.Tests;

// the JWT authentication of Program.cs, instead of the test authentication
[TestClass]
public sealed class JwtAuthenticationTests : ApiTestBase
{
    protected override bool UseTestAuthentication => false;

    [TestMethod]
    public async Task TokenOfALoginAuthenticatesTheNextRequests()
    {
        await Client.PostAsJsonAsync("/self-user-creation/create-user", new SelfUserCreationRequestDto { Email = "margaret@example.com", Password = "secret" });
        var login = await Client.PostAsJsonAsync("/authentication/login", new LoginRequestDto { Login = "margaret@example.com", Password = "secret" });
        var token = await login.Content.ReadFromJsonAsync<string>();

        Assert.AreEqual(HttpStatusCode.Unauthorized, (await Client.GetAsync("/token-validation/validate")).StatusCode);

        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        Assert.AreEqual("margaret@example.com", await Client.GetFromJsonAsync<string>("/token-validation/validate"));
        Assert.AreEqual(HttpStatusCode.Forbidden, (await Client.PostAsJsonAsync("/users", new UserInput { Email = "x@example.com", Provider = "local" })).StatusCode);
    }
}
