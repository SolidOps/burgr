using Microsoft.VisualStudio.TestTools.UnitTesting;
using SolidOps.Burgr.Shared.Contracts.DTO;
using SolidOps.Burgr.Shared.Tests;
using SolidOps.UM.Contracts.DTO;
using SolidOps.UM.Tests.Endpoints;

namespace SolidOps.UM.Tests;

[TestClass]
public class ContinuationIdTests : BaseAppServiceTest<UMTestSetup>
{
    [TestMethod]
    public async Task TestContinuationIdWorks()
    {
        // init data
        var count = 5;
        foreach (var i in Enumerable.Range(1, count))
        {
            var orgId = await TestSetup.RootClient.UMAPI_AddOrganization(new OrganizationWriteDTO() { Name = $"ORG {i}" });
            await TestSetup.RootClient.UMFacade_UserCreation_CreateUser(new UserCreationInfoDTO()
            {
                OrganizationId = orgId,
                UserEmail = $"contact@org{i}.com",
                Password = Guid.NewGuid().ToString(),
                RoleName = "contact"
            });
        }

        var organisations = await TestSetup.RootClient.UMAPI_GetOrganizations();
        Assert.AreEqual(count, organisations.Count());

        var maxResults = 2;
        var remaining = count;
        var filter = new OrganizationQueryFilterDTO()
        {
            MaxResults = maxResults
        };
        
        organisations = await TestSetup.RootClient.UMAPI_GetOrganizations(filter);
        Assert.AreEqual(maxResults, organisations.Count());
        Assert.AreEqual("ORG 1", organisations.ElementAt(0).Name);
        Assert.AreEqual("ORG 2", organisations.ElementAt(1).Name);            

        organisations = await TestSetup.RootClient.UMAPI_GetOrganizations(filter);
        Assert.AreEqual(maxResults, organisations.Count());
        Assert.AreEqual("ORG 3", organisations.ElementAt(0).Name);
        Assert.AreEqual("ORG 4", organisations.ElementAt(1).Name);
    }

    [TestMethod]
    public async Task TestContinuationIdWorksWithTwiceRows()
    {
        // init data
        var count = 5;
        foreach (var i in Enumerable.Range(1, count))
        {
            var orgId = await TestSetup.RootClient.UMAPI_AddOrganization(new OrganizationWriteDTO() { Name = $"ORG {i}" });
            await TestSetup.RootClient.UMFacade_UserCreation_CreateUser(new UserCreationInfoDTO()
            {
                OrganizationId = orgId,
                UserEmail = $"admin@org{i}.com",
                Password = Guid.NewGuid().ToString(),
                RoleName = "admin"
            });
            await TestSetup.RootClient.UMFacade_UserCreation_CreateUser(new UserCreationInfoDTO()
            {
                OrganizationId = orgId,
                UserEmail = $"contact@org{i}.com",
                Password = Guid.NewGuid().ToString(),
                RoleName = "contact"
            });
        }

        var organisations = await TestSetup.RootClient.UMAPI_GetOrganizations();
        Assert.AreEqual(count, organisations.Count());

        var maxResults = 2;
        var filter = new OrganizationQueryFilterDTO()
        {
            MaxResults = maxResults,
            OrderBy = new List<OrderByClause>() { new OrderByClause("Name", OrderByWay.Ascending) }
        };
        organisations = await TestSetup.RootClient.UMAPI_GetOrganizations(filter, new List<string>() { "Roles" });
        Assert.AreEqual(maxResults, organisations.Count());
        Assert.AreEqual("ORG 1", organisations.ElementAt(0).Name);
        Assert.AreEqual("ORG 2", organisations.ElementAt(1).Name);

        organisations = await TestSetup.RootClient.UMAPI_GetOrganizations(filter, new List<string>() { "Roles" });
        Assert.AreEqual(maxResults, organisations.Count());
        Assert.AreEqual("ORG 3", organisations.ElementAt(0).Name);
        Assert.AreEqual("ORG 4", organisations.ElementAt(1).Name);
    }

    [TestMethod]
    public async Task TestOrderBy()
    {
        // init data
        var count = 5;
        foreach (var i in Enumerable.Range(1, count))
        {
            var orgId = await TestSetup.RootClient.UMAPI_AddOrganization(new OrganizationWriteDTO() { Name = $"ORG {i}" });
            await TestSetup.RootClient.UMFacade_UserCreation_CreateUser(new UserCreationInfoDTO()
            {
                OrganizationId = orgId,
                UserEmail = $"admin@org{i}.com",
                Password = Guid.NewGuid().ToString(),
                RoleName = "admin"
            });
            await TestSetup.RootClient.UMFacade_UserCreation_CreateUser(new UserCreationInfoDTO()
            {
                OrganizationId = orgId,
                UserEmail = $"contact@org{i}.com",
                Password = Guid.NewGuid().ToString(),
                RoleName = "contact"
            });
        }

        var organisations = await TestSetup.RootClient.UMAPI_GetOrganizations();
        Assert.AreEqual(count, organisations.Count());

        var filter = new OrganizationQueryFilterDTO()
        {
            OrderBy = new List<OrderByClause>() { new OrderByClause("Name", OrderByWay.Ascending) }
        };
        organisations = await TestSetup.RootClient.UMAPI_GetOrganizations(filter, new List<string>() { "Roles" });
        Assert.AreEqual(count, organisations.Count());
        Assert.AreEqual("ORG 1", organisations.ElementAt(0).Name);
        Assert.AreEqual("ORG 2", organisations.ElementAt(1).Name);
        Assert.AreEqual("ORG 3", organisations.ElementAt(2).Name);
        Assert.AreEqual("ORG 4", organisations.ElementAt(3).Name);
        Assert.AreEqual("ORG 5", organisations.ElementAt(4).Name);

        filter = new OrganizationQueryFilterDTO()
        {
            OrderBy = new List<OrderByClause>() { new OrderByClause("Name", OrderByWay.Descending) }
        };
        organisations = await TestSetup.RootClient.UMAPI_GetOrganizations(filter, new List<string>() { "Roles" });
        Assert.AreEqual(count, organisations.Count());
        Assert.AreEqual("ORG 5", organisations.ElementAt(0).Name);
        Assert.AreEqual("ORG 4", organisations.ElementAt(1).Name);
        Assert.AreEqual("ORG 3", organisations.ElementAt(2).Name);
        Assert.AreEqual("ORG 2", organisations.ElementAt(3).Name);
        Assert.AreEqual("ORG 1", organisations.ElementAt(4).Name);
    }

}
