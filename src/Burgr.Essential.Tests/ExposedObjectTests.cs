using SolidOps.Burgr.Core;
using SolidOps.Burgr.Core.Descriptors;
using SolidOps.Burgr.Core.Generators;
using SolidOps.Burgr.Essential.Generators;
using SolidOps.Burgr.Essential.Generators.Enums;
using SolidOps.Burgr.Essential.Generators.Objects;
using SolidOps.Burgr.Essential.Yaml;
using SolidOps.Burgr.Essential.Yaml.Model.Objects;

namespace SolidOps.Burgr.Essential.Tests;

[TestClass]
public class ExposedObjectTests
{
    [TestMethod]
    public void IsExposedExposesTheObjectAndWhatItReferences()
    {
        var objects = Parse("""
            burgr-model-version: 1.0.0

            aggregate_roots:
              country:
                properties:
                  name: string
              customer:
                is_exposed: true
                properties:
                  country:
                    type: country
              order:
                properties:
                  total: decimal
            """);

        Assert.IsTrue(objects["customer"].Is("Exposed"));
        Assert.IsTrue(objects["country"].Is("Exposed"));
        Assert.IsFalse(objects["order"].Is("Exposed"));
        Assert.AreEqual(0, objects["customer"].GetChildren(DescriptorTypes.RESOURCE_DEFINITION_DESCRIPTOR).Count());
    }

    private static Dictionary<string, ModelDescriptor> Parse(string yaml)
    {
        var directory = Path.Combine(Path.GetTempPath(), "burgr-tests", Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(directory);
        try
        {
            File.WriteAllText(Path.Combine(directory, "aggregates.yaml"), yaml);

            var engine = new ModelParserEngine(directory, "Test", "Tests");
            engine.BeforeParsing();
            var repository = new ModelDescriptionsRepository();
            var generators = new List<IGenerator> { new EnumGenerator(), new ObjectGenerator() };
            new ObjectModelParser().ParseModel(engine, repository, generators);
            engine.AfterParsing(repository);

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
