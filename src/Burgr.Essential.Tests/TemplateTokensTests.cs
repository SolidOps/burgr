using System.Text.Json.Nodes;
using SolidOps.Burgr.Core;

namespace SolidOps.Burgr.Essential.Tests;

// Tokens of burgr-templates.json: names of the module tokens, replaced in the whole file.
// The Mini set declares _MODULE_NAMESPACE_, _MODULE_ and _MODULE_SLUG_; the samples and the
// reference case cover the historical MetaCorp.Template / Template / SlugTemplate.
[TestClass]
public class TemplateTokensTests
{
    private const string MinimalConfig = """{ "ProjectName": "Shop", "NamespaceName": "Acme", "BuildingDirectory": "../output/" }""";

    [TestMethod]
    public void ModelNamesContainingTheHistoricalTokenAreKept()
    {
        DeterminismTests.InWorkDirectory(work =>
        {
            var templates = CopyMiniTemplates(work, _ => { });
            var specs = WriteSpecs(work, """
                burgr-model-version: 1.0.0

                aggregate_roots:
                  email_template:
                    properties:
                      template:
                        field_size: 50
                    api:
                      get:
                """);

            DeterminismTests.RunBurgr(specs, "/templateSpecDirectory:" + templates);

            var model = File.ReadAllText(Path.Combine(work, "output", "Shop", "Domain", "ShopModel.g.cs"));
            StringAssert.Contains(model, "class EmailTemplate");
            StringAssert.Contains(model, " Template { get; set; }");
            StringAssert.Contains(model, "namespace Acme.Shop.Domain");
        });
    }

    [TestMethod]
    public void TokensWithTheSameNameAreRefused()
    {
        DeterminismTests.InWorkDirectory(work =>
        {
            var templates = CopyMiniTemplates(work, manifest => manifest["Tokens"]!["Module"] = "_MODULE_NAMESPACE_");
            var specs = WriteSpecs(work, """
                burgr-model-version: 1.0.0

                aggregate_roots:
                  product:
                    properties:
                      name:
                """);

            var (_, output) = DeterminismTests.StartBurgr(specs, "/templateSpecDirectory:" + templates);

            StringAssert.Contains(output, "ModuleNamespace, Module and ModuleSlug must be set and different");
        });
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

    private static string WriteSpecs(string work, string model)
    {
        var specs = Path.Combine(work, "specs");
        Directory.CreateDirectory(specs);
        File.WriteAllText(Path.Combine(specs, "burgr.json"), MinimalConfig);
        File.WriteAllText(Path.Combine(specs, "model.yaml"), model);
        return specs;
    }
}
