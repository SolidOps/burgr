using SolidOps.Burgr.Core;

namespace SolidOps.Burgr.Essential.Tests;

[TestClass]
public class ServiceInputTypeTests
{
    private const string Model = """
        burgr-model-version: 1.0.0

        aggregate_roots:
          product:
            properties:
              name:

        services:
          catalog:
            methods:
              publish:
                inputs:
                  status: $status?
                  tags: string[]

        enums:
          status:
            values:
              Draft: 0
              Published: 1
        """;

    [TestMethod]
    public void TestThatAnOptionalEnumInputStaysNullableAndASimpleArrayInputStaysAnArray()
    {
        var services = Generate();

        StringAssert.Contains(services, "Acme.Shop.Contracts.Enums.StatusEnum? status");
        StringAssert.Contains(services, "System.String[] tags");
    }

    private static string Generate()
    {
        string services = null!;
        DeterminismTests.InWorkDirectory(work =>
        {
            var templates = Path.Combine(work, "templates");
            DeterminismTests.CopyDirectory(Path.Combine(DeterminismTests.GetMetadata("ReferenceDirectory"), "templates"), templates);
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
