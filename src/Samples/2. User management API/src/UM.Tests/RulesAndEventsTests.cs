using System.Collections.Concurrent;
using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using SolidOps.UM.Application;
using SolidOps.UM.Domain;
using SolidOps.UM.Infrastructure;

namespace SolidOps.UM.Tests;

[TestClass]
public sealed class RulesAndEventsTests : ApiTestBase
{
    private static readonly ConcurrentQueue<InviteUsedEvent> UsedInvites = new();

    protected override void ConfigureWebHost(Microsoft.AspNetCore.Hosting.IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services => services.AddScoped<IEventHandler<InviteUsedEvent>, RecordUsedInvites>());
    }

    [TestMethod]
    public async Task UserAddedThroughTheApiGetsALocalUserWithoutPassword()
    {
        await PostAsync<UserDto>("/users", NewUser("ada@example.com"));

        Assert.IsTrue(await Client.GetFromJsonAsync<bool>("/authentication/need-initial-password?email=ada@example.com"));
    }

    [TestMethod]
    public async Task RemovingAUserRemovesItsLocalUser()
    {
        var user = await PostAsync<UserDto>("/users", NewUser("bob@example.com"));

        Assert.AreEqual(HttpStatusCode.NoContent, (await Client.DeleteAsync($"/users/{user.Id}")).StatusCode);

        Assert.AreEqual(HttpStatusCode.NotFound, (await Client.GetAsync("/authentication/need-initial-password?email=bob@example.com")).StatusCode);
    }

    [TestMethod]
    public async Task UserRightsAreSynchronizedByTheUpdate()
    {
        var input = NewUser("grace@example.com");
        input.UserRights = [new UserRightInput { RightId = UMDbContext.AdminRightId }];
        var user = await PostAsync<UserDto>("/users", input);
        var admin = user.UserRights.Single();

        input.UserRights = [new UserRightInput { Id = admin.Id, RightId = UMDbContext.AdminRightId }, new UserRightInput { RightId = UMDbContext.ManageUsersRightId }];
        var both = await PutAsync(user.Id, input);
        Assert.HasCount(2, both.UserRights);
        Assert.IsTrue(both.UserRights.Any(r => r.Id == admin.Id), "the kept child keeps its Id");

        input.UserRights = [new UserRightInput { RightId = UMDbContext.ManageUsersRightId }];
        var one = await PutAsync(user.Id, input);
        Assert.AreEqual(UMDbContext.ManageUsersRightId, one.UserRights.Single().RightId);
    }

    [TestMethod]
    public async Task UsedInviteRaisesItsEvent()
    {
        var creator = await PostAsync<UserDto>("/users", NewUser("linus@example.com"));
        var invite = await PostAsync<InviteDto>("/invites", NewInvite(creator.Id, "guest@example.com"));

        await Client.PostAsJsonAsync("/invites/use-invite", new InviteUseRequestDto { InviteId = invite.Id, Password = "secret" });

        Assert.IsTrue(UsedInvites.Any(e => e.Data.Email == "guest@example.com" && e.Context == "used"));
    }

    [TestMethod]
    public async Task InvitesAreCountedByStatus()
    {
        var creator = await PostAsync<UserDto>("/users", NewUser("ken@example.com"));
        await PostAsync<InviteDto>("/invites", NewInvite(creator.Id, "one@example.com"));
        await PostAsync<InviteDto>("/invites", NewInvite(creator.Id, "two@example.com"));

        Assert.AreEqual(2, await Client.GetFromJsonAsync<int>($"/invites/count-by-status?status={InviteStatus.Sent}"));
        Assert.AreEqual(0, await Client.GetFromJsonAsync<int>($"/invites/count-by-status?status={InviteStatus.Used}"));

        Unauthenticate();
        Assert.AreEqual(HttpStatusCode.Unauthorized, (await Client.GetAsync($"/invites/count-by-status?status={InviteStatus.Sent}")).StatusCode);
    }

    private async Task<UserDto> PutAsync(Guid id, UserInput input)
    {
        var response = await Client.PutAsJsonAsync($"/users/{id}", input);
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode, await response.Content.ReadAsStringAsync());
        return (await response.Content.ReadFromJsonAsync<UserDto>())!;
    }

    private static UserInput NewUser(string email) => new() { Email = email, Provider = "local" };

    private static InviteInput NewInvite(Guid creatorId, string email) => new()
    {
        Email = email, CreatorId = creatorId, CreatorName = "Creator", CreatorMessage = "Welcome",
    };

    private sealed class RecordUsedInvites : IEventHandler<InviteUsedEvent>
    {
        public Task HandleAsync(InviteUsedEvent @event, CancellationToken cancellationToken = default)
        {
            UsedInvites.Enqueue(@event);
            return Task.CompletedTask;
        }
    }
}
