using SolidOps.Burgr.Core;

namespace SolidOps.Burgr.Essential.Tests;

// EnumType of burgr-templates.json: the name burgr writes for an enum in a method parameter.
[TestClass]
public class EnumTypeTests
{
    private const string Model = """
        burgr-model-version: 1.0.0

        aggregate_roots:
          product:
            properties:
              name:
              status: $status

        services:
          catalog:
            methods:
              publish:
                inputs:
                  status: $status

        enums:
          status:
            values:
              Draft: 0
              Published: 1
        """;

    [TestMethod]
    public void EnumParameterFollowsNoradConventionByDefault()
    {
        var services = Generate(manifest: null);

        StringAssert.Contains(services, "Task Publish(Acme.Shop.Contracts.Enums.StatusEnum status);");
    }

    [TestMethod]
    public void EnumParameterFollowsTheEnumTypeOfTheManifest()
    {
        var services = Generate("""{ "EnumType": { "Prefix": ".Domain.", "Suffix": "" } }""");

        StringAssert.Contains(services, "Task Publish(Acme.Shop.Domain.Status status);");
    }

    private static string Generate(string? manifest)
    {
        string services = null!;
        DeterminismTests.InWorkDirectory(work =>
        {
            var templates = Path.Combine(work, "templates");
            DeterminismTests.CopyDirectory(Path.Combine(DeterminismTests.GetMetadata("ReferenceDirectory"), "templates"), templates);
            if (manifest != null)
            {
                File.WriteAllText(Path.Combine(templates, TemplateManifest.FileName), manifest);
            }
            var specs = Path.Combine(work, "specs");
            Directory.CreateDirectory(specs);
            File.WriteAllText(Path.Combine(specs, "burgr.json"), """
                {
                  "ProjectName": "Shop",
                  "NamespaceName": "Acme",
                  "BuildingDirectory": "../output/",
                  "Templates": [ "Domain" ],
                  "Generators": [
                    "SolidOps.Burgr.Essential.Generators.Enums.EnumGenerator, SolidOps.Burgr.Essential",
                    "SolidOps.Burgr.Essential.Generators.Objects.ObjectGenerator, SolidOps.Burgr.Essential",
                    "SolidOps.Burgr.Essential.Generators.Services.ServiceGenerator, SolidOps.Burgr.Essential",
                    "SolidOps.Burgr.Essential.Generators.Rights.RightGenerator, SolidOps.Burgr.Essential"
                  ]
                }
                """);
            File.WriteAllText(Path.Combine(specs, "model.yaml"), Model);

            DeterminismTests.RunBurgr(specs, "/templateSpecDirectory:" + templates);

            services = File.ReadAllText(Path.Combine(work, "output", "Shop", "Application", "ShopServices.g.cs"));
        });
        return services;
    }
}
