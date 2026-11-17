using System.Globalization;
using SolidOps.Burgr.Core;

namespace SolidOps.Burgr.Essential.Tests;

// Under a Turkish culture, "I".ToLower() gives a dotless "ı": names derived from the model
// must not depend on the culture of the machine that generates.
[TestClass]
public class CultureTests
{
    private CultureInfo? previousCulture;

    [TestInitialize]
    public void UseTurkishCulture()
    {
        previousCulture = CultureInfo.CurrentCulture;
        CultureInfo.CurrentCulture = new CultureInfo("tr-TR");
    }

    [TestCleanup]
    public void RestoreCulture()
    {
        CultureInfo.CurrentCulture = previousCulture!;
    }

    [TestMethod]
    public void CamelCaseIsCultureInvariant()
    {
        Assert.AreEqual("inviteId", ConversionHelper.ConvertToCamelCase("Invite_id"));
    }

    [TestMethod]
    public void LowerFileNameIsCultureInvariant()
    {
        Assert.AreEqual("identity.service.g.ts", Utilities.GetFileName("Lab.Identity", ".service.ts", "Lower", "", ".g"));
    }
}
