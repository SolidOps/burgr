using System.Text.Json.Nodes;
using SolidOps.Burgr.Core;
using SolidOps.Burgr.Essential.Yaml;
using SolidOps.Burgr.Essential.Yaml.Model;

namespace SolidOps.Burgr.Essential.Tests;

// burgr-templates.json: defaults of burgr.json, supported model features, harness.
[TestClass]
public class TemplateManifestTests
{
    private const string MinimalConfig = """{ "ProjectName": "Shop", "NamespaceName": "Acme", "BuildingDirectory": "../output/" }""";
    private const string EssentialGenerators = """
        "Generators": [
            "SolidOps.Burgr.Essential.Generators.Enums.EnumGenerator, SolidOps.Burgr.Essential",
            "SolidOps.Burgr.Essential.Generators.Objects.ObjectGenerator, SolidOps.Burgr.Essential"
        ]
        """;
    private const string Model = """
        burgr-model-version: 1.0.0

        aggregate_roots:
          product:
            properties:
              name:
                field_size: 50
            api:
              get:
        """;
    private const string FakeGenerator = "Fake.Generator, Fake";

    [TestMethod]
    public void GeneratorsOfTheManifestReplaceTheDefaults()
    {
        DeterminismTests.InWorkDirectory(work =>
        {
            var templates = CopyMiniTemplates(work, manifest => manifest["Generators"] = new JsonArray(FakeGenerator));
            var specs = WriteSpecs(work, MinimalConfig, Model);

            var (_, output) = DeterminismTests.StartBurgr(specs, "/templateSpecDirectory:" + templates);

            StringAssert.Contains(output, "Could not load assembly Fake,");
        });
    }

    [TestMethod]
    public void GeneratorsOfBurgrJsonReplaceThoseOfTheManifest()
    {
        DeterminismTests.InWorkDirectory(work =>
        {
            var templates = CopyMiniTemplates(work, manifest => manifest["Generators"] = new JsonArray(FakeGenerator));
            var config = MinimalConfig.Replace(" }", ", " + EssentialGenerators + " }");
            var specs = WriteSpecs(work, config, Model);

            DeterminismTests.RunBurgr(specs, "/templateSpecDirectory:" + templates);

            Assert.IsTrue(File.Exists(Path.Combine(work, "output", "Shop", "Domain", "ShopModel.g.cs")));
        });
    }

    [TestMethod]
    public void WithoutGeneratorsAnywhereTheEssentialOnesAreUsed()
    {
        DeterminismTests.InWorkDirectory(work =>
        {
            var templates = CopyMiniTemplates(work, manifest => manifest.Remove("Generators"));
            var specs = WriteSpecs(work, MinimalConfig, Model);

            DeterminismTests.RunBurgr(specs, "/templateSpecDirectory:" + templates);

            Assert.IsTrue(File.Exists(Path.Combine(work, "output", "Shop", "Domain", "ShopModel.g.cs")));
        });
    }

    [TestMethod]
    public void ProjectFileOfTheSetIsCreatedForAModuleWithoutOne()
    {
        DeterminismTests.InWorkDirectory(work =>
        {
            var templates = CopyMiniTemplates(work, _ => { });
            var specs = WriteSpecs(work, MinimalConfig, Model);

            DeterminismTests.RunBurgr(specs, "/templateSpecDirectory:" + templates);

            var project = File.ReadAllText(Path.Combine(work, "output", "Shop", "Shop.csproj"));
            StringAssert.Contains(project, "<Project Sdk=\"Microsoft.NET.Sdk.Web\">");
            StringAssert.Contains(project, "<AssemblyName>Acme.Shop</AssemblyName>");
        });
    }

    [TestMethod]
    public void MissingTemplateFolderIsRefused()
    {
        DeterminismTests.InWorkDirectory(work =>
        {
            var templates = CopyMiniTemplates(work, _ => { });
            var specs = WriteSpecs(work, MinimalConfig.Replace(" }", """, "Templates": ["Domain", "Missing"] }"""), Model);

            var (_, output) = DeterminismTests.StartBurgr(specs, "/templateSpecDirectory:" + templates);

            StringAssert.Contains(output, "Templates not found in");
            StringAssert.Contains(output, ": Missing");
        });
    }

    [TestMethod]
    public void UnsupportedFeatureIsRefusedWithItsName()
    {
        DeterminismTests.InWorkDirectory(work =>
        {
            var templates = CopyMiniTemplates(work, _ => { });
            var specs = WriteSpecs(work, MinimalConfig, Model + """

                services:
                  pricing:
                    methods:
                      compute:
                        result: int
                """);

            var (_, output) = DeterminismTests.StartBurgr(specs, "/templateSpecDirectory:" + templates);

            StringAssert.Contains(output, "The model uses features this template set does not support: services.");
            Assert.IsFalse(Directory.Exists(Path.Combine(work, "output")), "nothing is generated");
        });
    }

    [TestMethod]
    public void DevelopmentBuildWithoutTemplateSourceAsksForOne()
    {
        DeterminismTests.InWorkDirectory(work =>
        {
            var specs = WriteSpecs(work, MinimalConfig, Model);

            var (_, output) = DeterminismTests.StartBurgr(specs);

            StringAssert.Contains(output, "No template source");
            StringAssert.Contains(output, "/templateSpecDirectory:");
        });
    }

    [TestMethod]
    public void HarnessIsInstalledWithoutOverwritingAFile()
    {
        DeterminismTests.InWorkDirectory(work =>
        {
            var templates = CopyMiniTemplates(work, _ => { });
            var target = Path.Combine(work, "repository");
            Directory.CreateDirectory(target);
            File.WriteAllText(Path.Combine(target, "AGENTS.md"), "mine");

            var written = TemplateManifest.Read(templates).InstallHarness(templates, target);

            CollectionAssert.AreEqual(new[] { "CLAUDE.md", ".claude/settings.json", ".claude/hooks/block-generated-files.sh" }, written);
            Assert.AreEqual("mine", File.ReadAllText(Path.Combine(target, "AGENTS.md")));
        });
    }

    [TestMethod]
    public void UsedModelFeaturesAreDetected()
    {
        var content = new YamlDotNet.Serialization.Deserializer().Deserialize<YamlModelContentV1>("""
            aggregate_roots:
              order:
                key: [shop, number]
                properties:
                  shop: string
                  number: int
                  customer: customer
                  lines: ->line[]
                  total: =decimal
                  tags: string[]
                  reference: Acme.Catalog.product
                api:
                  get:
                    mandatory_right: read_orders
              customer:
                properties:
                  email:
                    multiple_unique_constraint_with: shop
                  shop: string
            entities:
              line:
                properties:
                  order: order
            enums:
              status:
                values:
                  Open: 0
            """);

        var features = ModelFeatures.Detect(new Dictionary<string, YamlModelContentV1> { [string.Empty] = content });

        CollectionAssert.AreEqual(new[]
        {
            "aggregate_roots", "calculated_properties", "composite_keys", "composite_unique_constraints", "entities", "enums",
            "external_references", "navigations", "primitive_collections", "references", "rights",
        }, features.ToArray());
    }

    private static string CopyMiniTemplates(string work, Action<JsonObject> changeManifest)
    {
        var templates = Path.Combine(work, "templates");
        DeterminismTests.CopyDirectory(Path.Combine(DeterminismTests.GetMetadata("TemplatesDirectory"), "Mini", "templates"), templates);
        var manifestPath = Path.Combine(templates, TemplateManifest.FileName);
        var manifest = JsonNode.Parse(File.ReadAllText(manifestPath))!.AsObject();
        changeManifest(manifest);
        File.WriteAllText(manifestPath, manifest.ToJsonString());
        return templates;
    }

    private static string WriteSpecs(string work, string config, string model)
    {
        var specs = Path.Combine(work, "specs");
        Directory.CreateDirectory(specs);
        File.WriteAllText(Path.Combine(specs, "burgr.json"), config);
        File.WriteAllText(Path.Combine(specs, "model.yaml"), model);
        return specs;
    }
}
