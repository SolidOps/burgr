using Microsoft.VisualStudio.TestTools.UnitTesting;
using SolidOps.UM.Shared.Contracts.Endpoints;
using SolidOps.UM.Shared.Tests;
using SolidOps.UM.Contracts.DTO;
using SolidOps.UM.Tests.Endpoints;

namespace SolidOps.UM.Tests;

[TestClass]
public class InviteTests : BaseAppServiceTest<UMTestSetup>
{
    [TestMethod]
    public async Task TestThatRegisteredUserCanInvite()
    {
        var email = "toto@email.com";
        var inviteId = await TestSetup.RootClient.UMAPI_AddInvite(new InviteWriteDTO()
        {
            Email = email,
            CreatorName = "root",
            CreatorMessage = "je t'invite"
        });

        var invite = await TestSetup.AnonymousClient.UMFacade_Invites_CheckInvite(Guid.Parse(inviteId));

        var password = "test";
        await TestSetup.AnonymousClient.UMFacade_Invites_UseInvite(Guid.Parse(inviteId), password);

        TestSetup.PasswordPerUser[email] = password;

        using (var totoInstance = await TestSetup.NewUserTestInstance(email, UMTestSetup.UM))
        {
            Assert.IsNotNull(totoInstance);
        }

        await TestSetup.AnonymousClient.UMFacade_Invites_CheckInvite(Guid.Parse(inviteId), new AssertParameters() { 
            ExpectedStatusCode = System.Net.HttpStatusCode.BadRequest,
            ExpectedError = "invalid invite status"
        });
    }
}
