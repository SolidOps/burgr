#nullable enable

using System.Net;
using System.Threading.Tasks;

namespace _MODULE_NAMESPACE_.Tests;

// the endpoints of every aggregate root answer: the model, the mapping and the registrations agree
[TestClass]
public partial class _MODULE_ApiTests : ApiTestBase
{
    #region foreach MODEL[AG][R]
    #region to remove if NOT_GETBYQUERY
    [TestMethod]
    public async Task CLASSNAMEQueryAnswers()
    {
        var response = await Client.GetAsync("/Slug_CLASSNAMES_?orderBy=Id&descending=true");
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode, await response.Content.ReadAsStringAsync());
    }
    #endregion to remove if NOT_GETBYQUERY

    #region to remove if NOT_GETBYID
    [TestMethod]
    public async Task UnknownCLASSNAMEIsNotFound()
    {
        var response = await Client.GetAsync($"/Slug_CLASSNAMES_/{UnknownId}");
        Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode, await response.Content.ReadAsStringAsync());
    }
    #endregion to remove if NOT_GETBYID

    #endregion foreach MODEL
}
