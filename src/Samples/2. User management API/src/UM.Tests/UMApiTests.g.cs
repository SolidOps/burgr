#nullable enable
using System.Net;
using System.Threading.Tasks;
namespace SolidOps.UM.Tests;
// the endpoints of every aggregate root answer: the model, the mapping and the registrations agree
[TestClass]
public partial class UMApiTests : ApiTestBase
{
    // Object [AG][R]

    [TestMethod]
    public async Task UserQueryAnswers()
    {
        var response = await Client.GetAsync("/users?orderBy=Id&descending=true");
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode, await response.Content.ReadAsStringAsync());
    }

    [TestMethod]
    public async Task UnknownUserIsNotFound()
    {
        var response = await Client.GetAsync($"/users/{UnknownId}");
        Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode, await response.Content.ReadAsStringAsync());
    }

    [TestMethod]
    public async Task InviteQueryAnswers()
    {
        var response = await Client.GetAsync("/invites?orderBy=Id&descending=true");
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode, await response.Content.ReadAsStringAsync());
    }

    [TestMethod]
    public async Task UnknownInviteIsNotFound()
    {
        var response = await Client.GetAsync($"/invites/{UnknownId}");
        Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode, await response.Content.ReadAsStringAsync());
    }

}