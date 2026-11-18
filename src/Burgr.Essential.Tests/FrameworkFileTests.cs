namespace SolidOps.Burgr.Essential.Tests;

// file_kind: framework in burgr.yaml: a model-independent file, written for every module with
// FrameworkFileSuffix, rewritten at each generation like the generated files.
[TestClass]
public class FrameworkFileTests
{
    private const string MinimalConfig = """{ "ProjectName": "Shop", "NamespaceName": "Acme", "BuildingDirectory": "../output/" }""";
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
    private const string EntityTemplate = """
        namespace MetaCorp.Template.Domain;

        public abstract class Entity
        {
            public Guid Id { get; set; }
        }
        """;
    private const string ValueObjectTemplate = """
        namespace MetaCorp.Template.Domain;

        public abstract record ValueObject;
        """;

    [TestMethod]
    public void FrameworkFileIsWrittenForTheModuleWithItsOwnSuffix()
    {
        DeterminismTests.InWorkDirectory(work =>
        {
            var templates = CopyMiniTemplates(work);
            AddFrameworkTemplate(templates, "entity", "Entity.cs", EntityTemplate);
            var specs = WriteSpecs(work, MinimalConfig);

            DeterminismTests.RunBurgr(specs, "/templateSpecDirectory:" + templates);

            var domain = Path.Combine(work, "output", "Shop", "Domain");
            StringAssert.Contains(File.ReadAllText(Path.Combine(domain, "Entity.f.cs")), "namespace Acme.Shop.Domain;");
            Assert.IsTrue(File.Exists(Path.Combine(domain, "ShopModel.g.cs")), "generated files are still written");
        });
    }

    [TestMethod]
    public void FrameworkFileSuffixOfBurgrJsonIsUsed()
    {
        DeterminismTests.InWorkDirectory(work =>
        {
            var templates = CopyMiniTemplates(work);
            AddFrameworkTemplate(templates, "entity", "Entity.cs", EntityTemplate);
            var specs = WriteSpecs(work, MinimalConfig.Replace(" }", """, "FrameworkFileSuffix": ".fw" }"""));

            DeterminismTests.RunBurgr(specs, "/templateSpecDirectory:" + templates);

            Assert.IsTrue(File.Exists(Path.Combine(work, "output", "Shop", "Domain", "Entity.fw.cs")));
        });
    }

    [TestMethod]
    public void FrameworkFileIsRewrittenAndDeletedOnceNoLongerProduced()
    {
        DeterminismTests.InWorkDirectory(work =>
        {
            var templates = CopyMiniTemplates(work);
            AddFrameworkTemplate(templates, "entity", "Entity.cs", EntityTemplate);
            var domainYaml = Path.Combine(templates, "Domain", "burgr.yaml");
            var withoutValueObject = File.ReadAllText(domainYaml);
            AddFrameworkTemplate(templates, "value_object", "ValueObject.cs", ValueObjectTemplate);
            var specs = WriteSpecs(work, MinimalConfig);
            DeterminismTests.RunBurgr(specs, "/templateSpecDirectory:" + templates);

            var domain = Path.Combine(work, "output", "Shop", "Domain");
            File.WriteAllText(Path.Combine(domain, "Entity.f.cs"), "edited by hand");
            File.WriteAllText(Path.Combine(domain, "Product.cs"), "hand-written");
            File.WriteAllText(domainYaml, withoutValueObject);
            var output = DeterminismTests.RunBurgr(specs, "/templateSpecDirectory:" + templates);

            StringAssert.Contains(File.ReadAllText(Path.Combine(domain, "Entity.f.cs")), "public abstract class Entity");
            Assert.IsFalse(File.Exists(Path.Combine(domain, "ValueObject.f.cs")));
            StringAssert.Contains(output, "Delete generated file");
            Assert.AreEqual("hand-written", File.ReadAllText(Path.Combine(domain, "Product.cs")));
        });
    }

    [TestMethod]
    public void FrameworkTemplateWithAModelRegionIsRefused()
    {
        DeterminismTests.InWorkDirectory(work =>
        {
            var templates = CopyMiniTemplates(work);
            AddFrameworkTemplate(templates, "entity", "Entity.cs", EntityTemplate + """

                #region foreach MODEL
                public partial class _CLASSNAME_ : Entity { }
                #endregion foreach MODEL
                """);
            var specs = WriteSpecs(work, MinimalConfig);

            var (_, output) = DeterminismTests.StartBurgr(specs, "/templateSpecDirectory:" + templates);

            StringAssert.Contains(output, "Framework template 'Entity.cs' contains model regions");
            Assert.IsFalse(File.Exists(Path.Combine(work, "output", "Shop", "Domain", "Entity.f.cs")));
        });
    }

    [TestMethod]
    public void UnknownFileKindIsRefused()
    {
        DeterminismTests.InWorkDirectory(work =>
        {
            var templates = CopyMiniTemplates(work);
            AddFrameworkTemplate(templates, "entity", "Entity.cs", EntityTemplate, "runtime");
            var specs = WriteSpecs(work, MinimalConfig);

            var (_, output) = DeterminismTests.StartBurgr(specs, "/templateSpecDirectory:" + templates);

            StringAssert.Contains(output, "Unknown file_kind 'runtime' for 'entity'");
        });
    }

    [TestMethod]
    public void FrameworkSuffixEqualToTheGeneratedOneIsRefused()
    {
        DeterminismTests.InWorkDirectory(work =>
        {
            var templates = CopyMiniTemplates(work);
            var specs = WriteSpecs(work, MinimalConfig.Replace(" }", """, "FrameworkFileSuffix": ".g" }"""));

            var (_, output) = DeterminismTests.StartBurgr(specs, "/templateSpecDirectory:" + templates);

            StringAssert.Contains(output, "FrameworkFileSuffix and GeneratedFileSuffix are both '.g'");
        });
    }

    private static string CopyMiniTemplates(string work)
    {
        var templates = Path.Combine(work, "templates");
        DeterminismTests.CopyDirectory(Path.Combine(DeterminismTests.GetMetadata("TemplatesDirectory"), "Mini", "templates"), templates);
        return templates;
    }

    private static void AddFrameworkTemplate(string templates, string name, string fileName, string content, string fileKind = "framework")
    {
        File.WriteAllText(Path.Combine(templates, "Domain", fileName + ".tpl"), content);
        File.AppendAllText(Path.Combine(templates, "Domain", "burgr.yaml"), $"""

              {name}:
                source: {fileName}.tpl
                file_suffix: {fileName}
                destination_suffix: Domain
                force_separate_dll: false
                destination_language: CS
                file_kind: {fileKind}

            """);
    }

    private static string WriteSpecs(string work, string config)
    {
        var specs = Path.Combine(work, "specs");
        Directory.CreateDirectory(specs);
        File.WriteAllText(Path.Combine(specs, "burgr.json"), config);
        File.WriteAllText(Path.Combine(specs, "model.yaml"), Model);
        return specs;
    }
}
