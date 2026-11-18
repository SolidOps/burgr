using System.Net;
using System.Net.Http.Json;
using Microsoft.Extensions.DependencyInjection;
using SolidOps.UM.Application;
using SolidOps.UM.Infrastructure;

namespace SolidOps.UM.Tests;

[TestClass]
public sealed class ServicesTests : ApiTestBase
{
    [TestMethod]
    public async Task UserCreatedByThemselvesCanLogIn()
    {
        Unauthenticate();
        var created = await Client.PostAsJsonAsync("/self-user-creation/create-user", new SelfUserCreationRequestDto { Email = "ada@example.com", Password = "secret" });
        Assert.AreEqual(HttpStatusCode.OK, created.StatusCode, await created.Content.ReadAsStringAsync());

        var login = await Client.PostAsJsonAsync("/authentication/login", new LoginRequestDto { Login = "ada@example.com", Password = "secret" });
        Assert.AreEqual(HttpStatusCode.OK, login.StatusCode);
        Assert.IsFalse(string.IsNullOrEmpty(await login.Content.ReadFromJsonAsync<string>()));

        var wrongPassword = await Client.PostAsJsonAsync("/authentication/login", new LoginRequestDto { Login = "ada@example.com", Password = "wrong" });
        Assert.AreEqual(HttpStatusCode.Forbidden, wrongPassword.StatusCode);
    }

    [TestMethod]
    public async Task RequestBodyIsValidatedBeforeTheService()
    {
        Unauthenticate();
        var response = await Client.PostAsJsonAsync("/self-user-creation/create-user", new SelfUserCreationRequestDto { Email = "bob@example.com", Password = "" });

        await AssertInvalidAsync(response, nameof(SelfUserCreationRequestDto.Password));
    }

    [TestMethod]
    public async Task InviteIsCheckedThenUsedOnce()
    {
        var creator = await PostAsync<UserDto>("/users", new UserInput { Email = "grace@example.com", Provider = "local" });
        var invite = await PostAsync<InviteDto>("/invites", new InviteInput
        {
            Email = "guest@example.com", CreatorId = creator.Id, CreatorName = "Grace", CreatorMessage = "Welcome",
        });
        Unauthenticate();

        var check = await Client.GetFromJsonAsync<InviteResultDto>($"/invites/check-invite?inviteId={invite.Id}");
        Assert.AreEqual("guest@example.com", check!.Email);
        Assert.AreEqual("Grace", check.Creator);

        var use = new InviteUseRequestDto { InviteId = invite.Id, Password = "secret" };
        Assert.AreEqual(HttpStatusCode.NoContent, (await Client.PostAsJsonAsync("/invites/use-invite", use)).StatusCode);
        Assert.AreEqual(HttpStatusCode.NotFound, (await Client.PostAsJsonAsync("/invites/use-invite", use)).StatusCode);
        Assert.AreEqual(HttpStatusCode.NotFound, (await Client.GetAsync($"/invites/check-invite?inviteId={invite.Id}")).StatusCode);
    }

    [TestMethod]
    public async Task UserCreationRequiresItsRight()
    {
        var info = new UserCreationInfoDto
        {
            UserEmail = "linus@example.com",
            Password = "secret",
            Rights = [new UserRightDto { RightId = UMDbContext.ManageUsersRightId }],
        };

        AuthenticateAs(UMRights.ManageUsers);
        Assert.AreEqual(HttpStatusCode.Forbidden, (await Client.PostAsJsonAsync("/user-creation/create-user", info)).StatusCode);

        AuthenticateAs(UMRights.CreateUser);
        var created = await Client.PostAsJsonAsync("/user-creation/create-user", info);
        Assert.AreEqual(HttpStatusCode.OK, created.StatusCode, await created.Content.ReadAsStringAsync());

        AuthenticateAs(UMRights.ManageUsers);
        var user = await Client.GetFromJsonAsync<UserDto>($"/users/{await created.Content.ReadFromJsonAsync<Guid>()}");
        Assert.AreEqual(UMDbContext.ManageUsersRightId, user!.UserRights.Single().RightId);
    }

    [TestMethod]
    public async Task UnknownRightOfANewUserIsInvalid()
    {
        var info = new UserCreationInfoDto { UserEmail = "ken@example.com", Password = "secret", Rights = [new UserRightDto { RightId = Guid.NewGuid() }] };

        await AssertInvalidAsync(await Client.PostAsJsonAsync("/user-creation/create-user", info), "Rights[0].RightId");
    }

    [TestMethod]
    public async Task ManagingUsersRequiresItsRight()
    {
        AuthenticateAs();
        Assert.AreEqual(HttpStatusCode.OK, (await Client.GetAsync("/users")).StatusCode);
        Assert.AreEqual(HttpStatusCode.Forbidden, (await Client.PostAsJsonAsync("/users", new UserInput { Email = "a@example.com", Provider = "local" })).StatusCode);

        AuthenticateAs(UMRights.ManageUsers);
        Assert.AreEqual(HttpStatusCode.Created, (await Client.PostAsJsonAsync("/users", new UserInput { Email = "a@example.com", Provider = "local" })).StatusCode);
    }

    [TestMethod]
    public async Task InitialPasswordIsSetOnce()
    {
        await WithLocalUserWithoutPasswordAsync("tech");
        Unauthenticate();

        Assert.IsTrue(await Client.GetFromJsonAsync<bool>("/server-status/need-tech-user-password-update?techUser=tech"));
        var request = new LoginRequestDto { Login = "tech", Password = "secret" };
        Assert.AreEqual(HttpStatusCode.NoContent, (await Client.PostAsJsonAsync("/authentication/set-initial-password", request)).StatusCode);
        Assert.AreEqual(HttpStatusCode.Conflict, (await Client.PostAsJsonAsync("/authentication/set-initial-password", request)).StatusCode);
        Assert.IsFalse(await Client.GetFromJsonAsync<bool>("/authentication/need-initial-password?email=tech"));
    }

    [TestMethod]
    public async Task TokenValidationReturnsTheCurrentUser()
    {
        Assert.AreEqual("test-user", await Client.GetFromJsonAsync<string>("/token-validation/validate"));

        Unauthenticate();
        Assert.AreEqual(HttpStatusCode.Unauthorized, (await Client.GetAsync("/token-validation/validate")).StatusCode);
    }

    private async Task WithLocalUserWithoutPasswordAsync(string name)
    {
        using var scope = Factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<UMDbContext>();
        db.LocalUsers.Add(new Domain.LocalUser { Name = name, HashedPassword = "" });
        await db.SaveChangesAsync();
    }
}
