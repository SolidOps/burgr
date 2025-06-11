using Microsoft.VisualStudio.TestTools.UnitTesting;
using SolidOps.Burgr.Shared.Contracts.Endpoints;
using SolidOps.Burgr.Shared.Infrastructure.Queries;
using SolidOps.Burgr.Shared.Tests;
using SolidOps.UM.Contracts.DTO;
using SolidOps.UM.Tests.Endpoints;

namespace SolidOps.UM.Tests;

[TestClass]
public class ETagTests : BaseAppServiceTest<UMTestSetup>
{
    [TestMethod]
    public async Task TestThatETagWorksWithoutFilters()
    {
        // init data
        var org1Id = await TestSetup.RootClient.UMAPI_AddOrganization(new OrganizationWriteDTO() { Name = "ORG 1" });
        var org2Id = await TestSetup.RootClient.UMAPI_AddOrganization(new OrganizationWriteDTO() { Name = "ORG 2" });
        await TestSetup.RootClient.UMAPI_AddOrganization(new OrganizationWriteDTO() { Name = "ORG 3" });

        // query should return ok
        var organizations = await TestSetup.RootClient.UMAPI_GetOrganizations(filter: null, assertParameters: new AssertParameters()
        {
            ExpectedStatusCode = System.Net.HttpStatusCode.OK,
        });
        Assert.AreEqual(3, organizations.Count());

        // query should return not modified
        organizations = await TestSetup.RootClient.UMAPI_GetOrganizations(filter: null, assertParameters: new AssertParameters()
        {
            ExpectedStatusCode = System.Net.HttpStatusCode.NotModified,
        });
        Assert.AreEqual(3, organizations.Count());

        // add new org should reset etag => return ok
        await TestSetup.RootClient.UMAPI_AddOrganization(new OrganizationWriteDTO() { Name = "ORG 4" });
        organizations = await TestSetup.RootClient.UMAPI_GetOrganizations(filter: null, assertParameters: new AssertParameters()
        {
            ExpectedStatusCode = System.Net.HttpStatusCode.OK,
        });
        Assert.AreEqual(4, organizations.Count());
        
        // get should return ok
        var organization1 = await TestSetup.RootClient.UMAPI_GetOrganization(org1Id, assertParameters: new AssertParameters()
        {
            ExpectedStatusCode = System.Net.HttpStatusCode.OK,
        });
        Assert.IsNotNull(organization1);

        var organization2 = await TestSetup.RootClient.UMAPI_GetOrganization(org2Id, assertParameters: new AssertParameters()
        {
            ExpectedStatusCode = System.Net.HttpStatusCode.OK,
        });
        Assert.IsNotNull(organization2);

        // get again should return not modified
        organization1 = await TestSetup.RootClient.UMAPI_GetOrganization(org1Id, assertParameters: new AssertParameters()
        {
            ExpectedStatusCode = System.Net.HttpStatusCode.NotModified,
        });
        Assert.IsNotNull(organization1);
        Assert.AreEqual("ORG 1", organization1.Name);

        organization2 = await TestSetup.RootClient.UMAPI_GetOrganization(org2Id, assertParameters: new AssertParameters()
        {
            ExpectedStatusCode = System.Net.HttpStatusCode.NotModified,
        });
        Assert.IsNotNull(organization2);
        Assert.AreEqual("ORG 2", organization2.Name);

        // update one org should reset org1 only and query
        await TestSetup.RootClient.UMAPI_PatchOrganization(org1Id, new OrganizationPatchDTO() { Name = "Modified" });
        organization1 = await TestSetup.RootClient.UMAPI_GetOrganization(org1Id, assertParameters: new AssertParameters()
        {
            ExpectedStatusCode = System.Net.HttpStatusCode.OK,
        });
        Assert.IsNotNull(organization1);
        Assert.AreEqual("Modified", organization1.Name);

        organization2 = await TestSetup.RootClient.UMAPI_GetOrganization(org2Id, assertParameters: new AssertParameters()
        {
            ExpectedStatusCode = System.Net.HttpStatusCode.NotModified,
        });
        Assert.IsNotNull(organization2);
        Assert.AreEqual("ORG 2", organization2.Name);

        organizations = await TestSetup.RootClient.UMAPI_GetOrganizations(filter: null, assertParameters: new AssertParameters()
        {
            ExpectedStatusCode = System.Net.HttpStatusCode.OK,
        });
        Assert.AreEqual(4, organizations.Count());

        // get or query again should return not modified
        organization1 = await TestSetup.RootClient.UMAPI_GetOrganization(org1Id, assertParameters: new AssertParameters()
        {
            ExpectedStatusCode = System.Net.HttpStatusCode.NotModified,
        });
        Assert.IsNotNull(organization1);
        Assert.AreEqual("Modified", organization1.Name);
        
        organizations = await TestSetup.RootClient.UMAPI_GetOrganizations(filter: null, assertParameters: new AssertParameters()
        {
            ExpectedStatusCode = System.Net.HttpStatusCode.NotModified,
        });
        Assert.AreEqual(4, organizations.Count());

    }

    [TestMethod]
    public async Task TestThatETagWorksWithFilters()
    {
        // init data
        var org1Id = await TestSetup.RootClient.UMAPI_AddOrganization(new OrganizationWriteDTO() { Name = "ORG 1" });
        var org2Id = await TestSetup.RootClient.UMAPI_AddOrganization(new OrganizationWriteDTO() { Name = "ORG 2" });
        await TestSetup.RootClient.UMAPI_AddOrganization(new OrganizationWriteDTO() { Name = "Another org" });

        // query should return ok
        var organizations = await TestSetup.RootClient.UMAPI_GetOrganizations(filter: null, assertParameters: new AssertParameters()
        {
            ExpectedStatusCode = System.Net.HttpStatusCode.OK,
        });
        Assert.AreEqual(3, organizations.Count());

        // query should return not modified
        organizations = await TestSetup.RootClient.UMAPI_GetOrganizations(filter: null, assertParameters: new AssertParameters()
        {
            ExpectedStatusCode = System.Net.HttpStatusCode.NotModified,
        });
        Assert.AreEqual(3, organizations.Count());


        // use filter
        var filter = new OrganizationQueryFilterDTO()
        {
            Filter = $"Name ~= org{FilterHelper.JOKERCHAR}"
        };
        organizations = await TestSetup.RootClient.UMAPI_GetOrganizations(filter: filter, assertParameters: new AssertParameters()
        {
            ExpectedStatusCode = System.Net.HttpStatusCode.OK,
        });
        Assert.AreEqual(2, organizations.Count());

        // assert etag works
        organizations = await TestSetup.RootClient.UMAPI_GetOrganizations(filter: filter, assertParameters: new AssertParameters()
        {
            ExpectedStatusCode = System.Net.HttpStatusCode.NotModified,
        });
        Assert.AreEqual(2, organizations.Count());

        filter = new OrganizationQueryFilterDTO()
        {
            Filter = $"Name ~= {FilterHelper.JOKERCHAR}1"
        };
        organizations = await TestSetup.RootClient.UMAPI_GetOrganizations(filter: filter, assertParameters: new AssertParameters()
        {
            ExpectedStatusCode = System.Net.HttpStatusCode.OK,
        });
        Assert.AreEqual(1, organizations.Count());

        organizations = await TestSetup.RootClient.UMAPI_GetOrganizations(filter: filter, assertParameters: new AssertParameters()
        {
            ExpectedStatusCode = System.Net.HttpStatusCode.NotModified,
        });
        Assert.AreEqual(1, organizations.Count());

        filter = new OrganizationQueryFilterDTO()
        {
            Filter = $"Name ~= org{FilterHelper.JOKERCHAR}"
        };
        organizations = await TestSetup.RootClient.UMAPI_GetOrganizations(filter: filter, assertParameters: new AssertParameters()
        {
            ExpectedStatusCode = System.Net.HttpStatusCode.NotModified,
        });
        Assert.AreEqual(2, organizations.Count());

        filter = new OrganizationQueryFilterDTO()
        {
            Filter = $"Name ~= {FilterHelper.JOKERCHAR}org{FilterHelper.JOKERCHAR}"
        };
        organizations = await TestSetup.RootClient.UMAPI_GetOrganizations(filter: filter, assertParameters: new AssertParameters()
        {
            ExpectedStatusCode = System.Net.HttpStatusCode.OK,
        });
        Assert.AreEqual(3, organizations.Count());

        organizations = await TestSetup.RootClient.UMAPI_GetOrganizations(filter: filter, assertParameters: new AssertParameters()
        {
            ExpectedStatusCode = System.Net.HttpStatusCode.NotModified,
        });
        Assert.AreEqual(3, organizations.Count());
    }
}
