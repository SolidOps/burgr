using Microsoft.VisualStudio.TestTools.UnitTesting;
using SolidOps.UM.Shared.Contracts.Endpoints;
using SolidOps.UM.Shared.Tests;
using SolidOps.UM.Contracts.DTO;
using SolidOps.UM.Tests.Endpoints;

namespace SolidOps.UM.Tests;

[TestClass]
public class UserTests : BaseAppServiceTest<UMTestSetup>
{
    [TestMethod]
    public async Task TestThatAnyoneCanCreateAUser()
    {
        var password = Guid.NewGuid().ToString();
        var userId = await TestSetup.AnonymousClient.UMFacade_SelfUserCreation_CreateUser(new SelfUserCreationRequestDTO()
        {
            Email = UMTestSetup.User.JIM,
            Password = password
        });

        var user = await TestSetup.RootClient.UMAPI_GetUser(userId);
        Assert.IsNotNull(user);

        await TestSetup.AnonymousClient.UMFacade_SelfUserCreation_CreateUser(new SelfUserCreationRequestDTO()
        {
            Email = "jim",
            Password = password
        }, new AssertParameters()
        {
            ExpectedStatusCode = System.Net.HttpStatusCode.BadRequest,
            ExpectedError = "email is not valid"
        });

        await TestSetup.AnonymousClient.UMFacade_SelfUserCreation_CreateUser(new SelfUserCreationRequestDTO()
        {
            Email = UMTestSetup.User.JIM,
            Password = password
        }, new AssertParameters()
        {
            ExpectedStatusCode = System.Net.HttpStatusCode.BadRequest,
            ExpectedError = "User with this Email already exists"
        });
    }

    [TestMethod]
    public async Task TestThatRootCanGiveAccessForAUserToAnApp()
    {
        var password = Guid.NewGuid().ToString();
        await TestSetup.AnonymousClient.UMFacade_SelfUserCreation_CreateUser(new SelfUserCreationRequestDTO()
        {
            Email = UMTestSetup.User.JIM,
            Password = password
        });
    }

    [TestMethod]
    public async Task TestThatAnyoneCanUpdateHisHerUser()
    {
        var jimId = await TestSetup.CreateUserWithRoot(UMTestSetup.User.JIM);
        var dwightId = await TestSetup.CreateUserWithRoot(UMTestSetup.User.DWIGHT);

        using (var jim = await TestSetup.NewUserTestInstance(UMTestSetup.User.JIM, UMTestSetup.UM))
        {
            var jimUM = jim.PickClient(UMTestSetup.UM);
            await jimUM.UMAPI_PatchUser(jimId, new UserPatchDTO()
            {
                Email = UMTestSetup.User.ANGELA
            });

            await jimUM.UMAPI_PatchUser(dwightId, new UserPatchDTO()
            {
                Email = UMTestSetup.User.MICHAEL
            }, new AssertParameters() { ExpectedStatusCode = System.Net.HttpStatusCode.Forbidden, ExpectedError = $"Current user cannot update User with Id {dwightId}" });
        }
    }

    
}
