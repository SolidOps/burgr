using SolidOps.Burgr.Essential.Yaml.Model.Services;

namespace SolidOps.Burgr.Essential.Tests;

[TestClass]
public class ServiceReturnTypeTests
{
    [TestMethod]
    public void AnAggregateOfAnotherModuleIsAModel()
    {
        Assert.AreEqual(ReturnType.Model, ServiceModelParser.GetReturnType("SolidOps.PSS31.PSS.order", "SolidOps.PSS31.P31"));
        Assert.AreEqual(ReturnType.ModelList, ServiceModelParser.GetReturnType("SolidOps.PSS31.PSS.order[]", "SolidOps.PSS31.P31"));
    }

    [TestMethod]
    public void ASimpleTypeStaysSimple()
    {
        Assert.AreEqual(ReturnType.Simple, ServiceModelParser.GetReturnType("long[]", "SolidOps.PSS31.P31"));
    }
}
