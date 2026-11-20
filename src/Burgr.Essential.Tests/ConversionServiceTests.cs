using SolidOps.Burgr.Essential.Generators.ConversionServices;

namespace SolidOps.Burgr.Essential.Tests;

[TestClass]
public class ConversionServiceTests
{
    [TestMethod]
    public void TestThatALongConvertsToANumberInDiagramsAndTypeScript()
    {
        Assert.AreEqual("number", PUMLConversionService.ConvertToPUMLPropertyType(typeof(long), null));
        Assert.AreEqual("number", PUMLConversionService.ConvertToPUMLPropertyType(typeof(long?), null));
        Assert.AreEqual("number", JSConversionService.ConvertToJSPropertyType(typeof(long)));
        Assert.AreEqual("number[]", JSConversionService.ConvertToJSPropertyType(typeof(long[])));
    }
}
