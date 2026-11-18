using System.Net;
using System.Net.Http.Json;
using Microsoft.Extensions.DependencyInjection;
using SolidOps.UM.Application;
using SolidOps.UM.Domain;
using SolidOps.UM.Infrastructure;

namespace SolidOps.UM.Tests;

[TestClass]
public sealed class UserManagementTests : ApiTestBase
{
    [TestMethod]
    public async Task UserGoesThroughTheWholeCrudWithItsPreferences()
    {
        var created = await PostAsync<UserDto>("/users", NewUser("ada@example.com", new Preferences { Language = "en", TimeZone = "Europe/London" }));

        var update = NewUser("ada.lovelace@example.com", new Preferences { Language = "fr", TimeZone = "Europe/Paris" });
        Assert.AreEqual(HttpStatusCode.OK, (await Client.PutAsJsonAsync($"/users/{created.Id}", update)).StatusCode);

        var read = await Client.GetFromJsonAsync<UserDto>($"/users/{created.Id}");
        Assert.AreEqual("ada.lovelace@example.com", read!.Email);
        Assert.AreEqual(new Preferences { Language = "fr", TimeZone = "Europe/Paris" }, read.Preferences);

        Assert.AreEqual(HttpStatusCode.NoContent, (await Client.DeleteAsync($"/users/{created.Id}")).StatusCode);
        Assert.AreEqual(HttpStatusCode.NotFound, (await Client.GetAsync($"/users/{created.Id}")).StatusCode);
    }

    [TestMethod]
    public async Task InvalidPreferencesAreReportedByPath()
    {
        var response = await Client.PostAsJsonAsync("/users", NewUser("bob@example.com", new Preferences { Language = "", TimeZone = "UTC" }));

        await AssertInvalidAsync(response, "Preferences.Language");
    }

    [TestMethod]
    public async Task UserIsReadWithItsRights()
    {
        var user = await PostAsync<UserDto>("/users", NewUser("grace@example.com", null));
        await WithDbContextAsync(db => db.Set<UserRight>().Add(new UserRight { UserId = user.Id, RightId = UMDbContext.AdminRightId }));

        var read = await Client.GetFromJsonAsync<UserDto>($"/users/{user.Id}");

        Assert.HasCount(1, read!.UserRights);
        Assert.AreEqual(UMDbContext.AdminRightId, read.UserRights[0].RightId);
    }

    [TestMethod]
    public async Task UserCannotHaveTheSameRightTwice()
    {
        var user = await PostAsync<UserDto>("/users", NewUser("linus@example.com", null));
        await WithDbContextAsync(db => db.Set<UserRight>().Add(new UserRight { UserId = user.Id, RightId = UMDbContext.AdminRightId }));

        await Assert.ThrowsAsync<Microsoft.EntityFrameworkCore.DbUpdateException>(() =>
            WithDbContextAsync(db => db.Set<UserRight>().Add(new UserRight { UserId = user.Id, RightId = UMDbContext.AdminRightId })));
    }

    [TestMethod]
    public async Task DeletingAUserDeletesItsRights()
    {
        var user = await PostAsync<UserDto>("/users", NewUser("ken@example.com", null));
        await WithDbContextAsync(db => db.Set<UserRight>().Add(new UserRight { UserId = user.Id, RightId = UMDbContext.ManageUsersRightId }));

        Assert.AreEqual(HttpStatusCode.NoContent, (await Client.DeleteAsync($"/users/{user.Id}")).StatusCode);

        using var scope = Factory.Services.CreateScope();
        Assert.AreEqual(0, scope.ServiceProvider.GetRequiredService<UMDbContext>().Set<UserRight>().Count());
    }

    [TestMethod]
    public async Task UserReferencedByAnInviteCannotBeDeleted()
    {
        var creator = await PostAsync<UserDto>("/users", NewUser("margaret@example.com", null));
        await PostAsync<InviteDto>("/invites", NewInvite(creator.Id));

        Assert.AreEqual(HttpStatusCode.Conflict, (await Client.DeleteAsync($"/users/{creator.Id}")).StatusCode);
    }

    [TestMethod]
    public async Task UniqueEmailIsAConflict()
    {
        await PostAsync<UserDto>("/users", NewUser("barbara@example.com", null));

        var response = await Client.PostAsJsonAsync("/users", NewUser("barbara@example.com", null));

        Assert.AreEqual(HttpStatusCode.Conflict, response.StatusCode);
    }

    [TestMethod]
    public async Task InviteOfAnUnknownCreatorIsInvalid()
    {
        var response = await Client.PostAsJsonAsync("/invites", NewInvite(Guid.NewGuid()));

        await AssertInvalidAsync(response, nameof(InviteInput.CreatorId));
    }

    [TestMethod]
    public async Task InviteIsOpenUntilUsed()
    {
        var creator = await PostAsync<UserDto>("/users", NewUser("frances@example.com", null));

        var sent = await PostAsync<InviteDto>("/invites", NewInvite(creator.Id));
        var used = await PostAsync<InviteDto>("/invites", NewInvite(creator.Id, InviteStatus.Used));

        Assert.IsTrue(sent.IsOpen);
        Assert.IsFalse(used.IsOpen);
    }

    [TestMethod]
    public async Task UsersArePagedAndSorted()
    {
        foreach (var email in new[] { "c@example.com", "a@example.com", "b@example.com" })
        {
            await PostAsync<UserDto>("/users", NewUser(email, null));
        }

        var page = await Client.GetFromJsonAsync<Page<UserDto>>("/users?orderBy=Email&descending=true&page=2&pageSize=2");

        Assert.AreEqual(3, page!.TotalCount);
        Assert.HasCount(1, page.Items);
        Assert.AreEqual("a@example.com", page.Items[0].Email);
    }

    [TestMethod]
    public async Task UnknownSortPropertyIsInvalid()
    {
        var response = await Client.GetAsync("/users?orderBy=HashedPassword");

        await AssertInvalidAsync(response, "orderBy");
    }

    private static UserInput NewUser(string email, Preferences? preferences) => new()
    {
        Email = email,
        Provider = "local",
        Preferences = preferences,
    };

    private static InviteInput NewInvite(Guid creatorId, InviteStatus status = InviteStatus.Sent) => new()
    {
        Email = "guest@example.com",
        Status = status,
        CreatorId = creatorId,
        CreatorName = "Creator",
        CreatorMessage = "Welcome",
    };

    private async Task WithDbContextAsync(Action<UMDbContext> change)
    {
        using var scope = Factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<UMDbContext>();
        change(db);
        await db.SaveChangesAsync();
    }
}
