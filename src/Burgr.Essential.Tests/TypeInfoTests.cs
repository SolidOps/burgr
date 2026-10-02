using SolidOps.Burgr.Essential.Yaml.Model;

namespace SolidOps.Burgr.Essential.Tests;

[TestClass]
public class TypeInfoTests
{
    [TestMethod]
    public void SynchronizedNavigationIsANavigation()
    {
        var info = new TypeInfo("~>role_right[]", "Wopr");

        Assert.IsTrue(info.IsNavigation);
        Assert.IsTrue(info.IsSynchronized);
        Assert.IsTrue(info.IsArray);
        Assert.AreEqual("role_right", info.Name);
        Assert.AreEqual(TypeType.Model, info.TypeType);
    }

    [TestMethod]
    public void PlainNavigationIsNotSynchronized()
    {
        var info = new TypeInfo("->module[]", "Wopr");

        Assert.IsTrue(info.IsNavigation);
        Assert.IsFalse(info.IsSynchronized);
        Assert.AreEqual("module", info.Name);
    }

    [TestMethod]
    public void NonPersistedIsUnchanged()
    {
        var info = new TypeInfo("-thing", "Wopr");

        Assert.IsTrue(info.IsNonPersisted);
        Assert.IsFalse(info.IsNavigation);
        Assert.IsFalse(info.IsSynchronized);
    }

    [TestMethod]
    public void SynchronizedOnSimpleTypeThrowsLikePlainNavigation()
    {
        var plain = Assert.ThrowsExactly<Exception>(() => new TypeInfo("->string[]", "Wopr"));
        var synchronized = Assert.ThrowsExactly<Exception>(() => new TypeInfo("~>string[]", "Wopr"));

        Assert.AreEqual(plain.Message, synchronized.Message);
    }

    [TestMethod]
    public void SynchronizedRequiresACollection()
    {
        Assert.ThrowsExactly<Exception>(() => new TypeInfo("~>role_right", "Wopr"));
    }

    [TestMethod]
    public void SynchronizedPrefixIsReadFromYamlAsAString()
    {
        var model = new YamlDotNet.Serialization.Deserializer().Deserialize<Dictionary<string, string>>("role_rights: ~>role_right[]");

        Assert.AreEqual("~>role_right[]", model["role_rights"]);
    }
}
