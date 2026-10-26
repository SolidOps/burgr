using SolidOps.Burgr.Core;
using SolidOps.Burgr.Core.Descriptors;
using SolidOps.Burgr.Core.Generators;
using SolidOps.Burgr.Essential.Generators.Enums;
using SolidOps.Burgr.Essential.Generators.Objects;
using SolidOps.Burgr.Essential.Yaml;
using SolidOps.Burgr.Essential.Yaml.Model.Objects;

namespace SolidOps.Burgr.Essential.Tests;

[TestClass]
public class CompositeKeyTests
{
    private const string Header = """
        burgr-model-version: 1.0.0

        aggregate_roots:
          category:
            properties:
              name: string
          product:
            properties:
              name: string

        """;

    [TestMethod]
    public void KeyMarksTheObjectAndItsKeyProperties()
    {
        var objects = Parse("""
              category_product:
                key: [category, product]
                properties:
                  category:
                    type: category
                    special_type: DBAlias
                    db_column_name: id_category
                  product:
                    type: product
                  position: int
            """);

        var model = objects["category_product"];
        Assert.IsTrue(model.Is("ComposedId"));
        Assert.AreEqual("id_category|product_id", model.Get("IdColumnName"));

        var properties = model.GetChildren(PropertyGenerator.Name).ToDictionary(p => p.Name);
        Assert.IsTrue(properties["category"].Is("Key"));
        Assert.AreEqual("0", properties["category"].Get("KeyIndex"));
        Assert.IsTrue(properties["product"].Is("Key"));
        Assert.AreEqual("1", properties["product"].Get("KeyIndex"));
        Assert.IsFalse(properties["position"].Is("Key"));
    }

    [TestMethod]
    public void KeyCanMixReferencesAndSimpleTypes()
    {
        var objects = Parse("""
              product_opening:
                key: [product, day]
                properties:
                  product:
                    type: product
                  day: datetime
            """);

        Assert.IsTrue(objects["product_opening"].Is("ComposedId"));
    }

    [TestMethod]
    public void ObjectWithoutKeyIsUnchanged()
    {
        var objects = Parse("");

        Assert.IsFalse(objects["category"].Is("ComposedId"));
        Assert.IsFalse(objects["category"].Is("ExplicitIdColumnName"));
        Assert.AreEqual("id", objects["category"].Get("IdColumnName"));
    }

    [TestMethod]
    public void IdColumnNameIsFlaggedWhenDeclared()
    {
        var objects = Parse("""
              tax:
                id_column_name: id_tax
                properties:
                  rate: decimal
            """);

        Assert.IsTrue(objects["tax"].Is("ExplicitIdColumnName"));
        Assert.AreEqual("id_tax", objects["tax"].Get("IdColumnName"));
    }

    [TestMethod]
    [DataRow("key: [category]", "at least two")]
    [DataRow("key: [category, category]", "twice")]
    [DataRow("key: [category, missing]", "is not a property")]
    [DataRow("key: [product, category]", "declaration order")]
    [DataRow("id_column_name: id_x\n    key: [category, product]", "cannot be used together")]
    public void InvalidKeyIsRejected(string keyLine, string expectedMessage)
    {
        AssertRejected($"""
              category_product:
                {keyLine}
                properties:
                  category:
                    type: category
                  product:
                    type: product
            """, expectedMessage);
    }

    [TestMethod]
    [DataRow("type: category?", "nullable")]
    [DataRow("type: -string", "persisted")]
    public void InvalidKeyPropertyIsRejected(string productType, string expectedMessage)
    {
        AssertRejected($"""
              category_product:
                key: [category, product]
                properties:
                  category:
                    type: category
                  product:
                    {productType}
            """, expectedMessage);
    }

    [TestMethod]
    public void UniqueConstraintIsRejectedOnAnObjectWithAKey()
    {
        AssertRejected("""
              category_product:
                key: [category, product]
                properties:
                  category:
                    type: category
                  product:
                    type: product
                  code:
                    type: string
                    is_unique: true
            """, "not supported");
    }

    [TestMethod]
    public void ObjectWithAKeyCannotBeReferenced()
    {
        AssertRejected("""
              category_product:
                key: [category, product]
                properties:
                  category:
                    type: category
                  product:
                    type: product
              promotion:
                properties:
                  category_product:
                    type: category_product
            """, "cannot be referenced");
    }

    private static void AssertRejected(string objects, string expectedMessage)
    {
        var exception = Assert.ThrowsExactly<Exception>(() => Parse(objects));
        StringAssert.Contains(exception.Message, expectedMessage);
    }

    private static Dictionary<string, ModelDescriptor> Parse(string objects)
    {
        var directory = Path.Combine(Path.GetTempPath(), "burgr-tests", Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(directory);
        try
        {
            var yaml = Header + objects;
            File.WriteAllText(Path.Combine(directory, "aggregates.yaml"), yaml);

            var engine = new ModelParserEngine(directory, "Test", "Tests");
            engine.BeforeParsing();
            var repository = new ModelDescriptionsRepository();
            var generators = new List<IGenerator> { new EnumGenerator(), new ObjectGenerator() };
            new ObjectModelParser().ParseModel(engine, repository, generators);

            return repository.modelDescriptions.Values
                .SelectMany(d => d.ModelDescriptors)
                .Where(m => m.DescriptorType == ObjectGenerator.Name)
                .ToDictionary(m => m.Name);
        }
        finally
        {
            Directory.Delete(directory, true);
        }
    }
}
