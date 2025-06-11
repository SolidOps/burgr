using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SolidOps.Burgr.Shared.Contracts.Endpoints;
using SolidOps.Burgr.Shared.Domain.UnitOfWork;
using SolidOps.Burgr.Shared.Tests;
using SolidOps.UM.Contracts.DTO;
using SolidOps.UM.Domain.Repositories;
using SolidOps.UM.Tests.Endpoints;

namespace SolidOps.UM.Tests;

[TestClass]
public class OrganizationTests : BaseAppServiceTest<UMTestSetup>
{
    [TestMethod]
    public async Task TestThatOnlyRootCanCreateOrganization()
    {
        var organization = await TestSetup.RootClient.UMAPI_AddOrganizationAndGet(new OrganizationWriteDTO() { Name = UMTestSetup.Organization.DUNDER_MIFFLIN });
        Assert.IsNotNull(organization);

        await TestSetup.RootClient.UMAPI_AddOrganization(new OrganizationWriteDTO() { Name = UMTestSetup.Organization.DUNDER_MIFFLIN }, new AssertParameters()
        {
            ExpectedStatusCode = System.Net.HttpStatusCode.BadRequest,
            ExpectedError = "Organization with this Name already exists"
        });

        var jimId = await TestSetup.CreateUserWithRoot(organization.Id, UMTestSetup.User.JIM);
        using (var instance = await TestSetup.NewUserTestInstance(UMTestSetup.User.JIM, UMTestSetup.UM))
        {
            var client = instance.PickClient(UMTestSetup.UM);
            await client.UMAPI_AddOrganization(new OrganizationWriteDTO() { Name = "my own organization" }, new AssertParameters()
            {
                ExpectedStatusCode = System.Net.HttpStatusCode.Forbidden,
                ExpectedError = "Missing mandatory right (UM, *)"
            });
        }
    }

    [TestMethod]
    public async Task TestThatAnyoneCannotCreateOrganizationWithRootRightOrRootApp()
    {
        await TestSetup.RootClient.UMAPI_AddOrganization(new OrganizationWriteDTO()
        {
            Name = UMTestSetup.Organization.DUNDER_MIFFLIN,
            ApplicationRights = new List<ApplicationRightDTO>(){
            new ApplicationRightDTO() { Application = IExecutionScope.ALLAPP, Right = "SellPaper" }
        }
        }, new AssertParameters()
        {
            ExpectedStatusCode = System.Net.HttpStatusCode.BadRequest,
            ExpectedError = "cannot add access to all apps"
        });

        await TestSetup.RootClient.UMAPI_AddOrganization(new OrganizationWriteDTO()
        {
            Name = UMTestSetup.Organization.DUNDER_MIFFLIN,
            ApplicationRights = new List<ApplicationRightDTO>(){
            new ApplicationRightDTO() { Application = "Paper App", Right = IExecutionScope.ALLRIGHT }
        }
        }, new AssertParameters()
        {
            ExpectedStatusCode = System.Net.HttpStatusCode.BadRequest,
            ExpectedError = "cannot give all rights"
        });
    }

    [TestMethod]
    public async Task TestThatRootCanCreateOrganizationWithCustomAppAndCustomRight()
    {
        var organization = await TestSetup.RootClient.UMAPI_AddOrganization(new OrganizationWriteDTO()
        {
            Name = UMTestSetup.Organization.DUNDER_MIFFLIN,
            ApplicationRights = new List<ApplicationRightDTO>(){
            new ApplicationRightDTO() { Application = "Paper App", Right = "SellPaper" }
        }
        });
        Assert.IsNotNull(organization);
    }

    [TestMethod]
    public async Task TestThatRootCanDeleteOrganizationAndAssociatedRoles()
    {
        var organization = await TestSetup.RootClient.UMAPI_AddOrganizationAndGet(new OrganizationWriteDTO() { Name = UMTestSetup.Organization.DUNDER_MIFFLIN, Roles = new List<RoleWriteDTO>() { new RoleWriteDTO() { Name = "Manager" } } });

        var organizations = await TestSetup.RootClient.UMAPI_GetOrganizations(null, new List<string>() { "Roles" });
        Assert.AreEqual(1, organizations.Count());
        Assert.AreEqual(1, organizations.Single().Roles.Count());

        var repository = TestSetup.AppServices[UMTestSetup.UM].Single().GetServiceScope().ServiceProvider.GetService<IRoleRepository>();
        var roles = await repository.GetList();
        Assert.AreEqual(1, roles.Count());

        await TestSetup.RootClient.UMAPI_RemoveOrganization(organization.Id);

        repository = TestSetup.AppServices[UMTestSetup.UM].Single().GetServiceScope().ServiceProvider.GetService<IRoleRepository>();
        roles = await repository.GetList();
        Assert.AreEqual(0, roles.Count());
    }


    [TestMethod]
    public async Task TestThatRootCanOnlyUpdateName()
    {
        var organization = await TestSetup.RootClient.UMAPI_AddOrganizationAndGet(new OrganizationWriteDTO()
        {
            Name = UMTestSetup.Organization.DUNDER_MIFFLIN,
            CreationYear = 10,
            Address = new AddressDTO()
            {
                Address1 = "9 Wernham Hogg street",
                City = "Scranton",
                Country = "Pennsylvania",
                Postcode = "18447"
            },
            Roles = new List<RoleWriteDTO>() { new RoleWriteDTO() { Name = "Manager" } }
        });

        Assert.AreEqual(10, organization.CreationYear);

        var updateDTO = new OrganizationPatchDTO()
        {
            Name = "new name"
        };

        await TestSetup.RootClient.UMAPI_PatchOrganization(organization.Id, updateDTO);

        organization = await TestSetup.RootClient.UMAPI_GetOrganization(organization.Id);

        Assert.AreEqual("new name", organization.Name);
        Assert.AreEqual(10, organization.CreationYear);
    }
}
