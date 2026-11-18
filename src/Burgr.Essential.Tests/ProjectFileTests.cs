namespace SolidOps.Burgr.Essential.Tests;

// project_file in the burgr.yaml of a template folder: the project file written for the projects
// its templates write to, before the ProjectFile of the manifest and burgr's own.
[TestClass]
public class ProjectFileTests
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
    private const string DomainProject = """
        <Project Sdk="Microsoft.NET.Sdk">
          <PropertyGroup>
            <AssemblyName>MetaCorp.Template</AssemblyName>
          </PropertyGroup>
          <ItemGroup>
            <ProjectReference Include="../Template.Contracts/Template.Contracts.csproj" />
          </ItemGroup>
        </Project>
        """;

    [TestMethod]
    public void ProjectFileOfTheFolderIsWrittenForItsSeparateProject()
    {
        DeterminismTests.InWorkDirectory(work =>
        {
            var templates = CopyMiniTemplates(work);
            SeparateProject(templates, "Domain", "Domain", "Domain.csproj", DomainProject);
            var specs = WriteSpecs(work);

            DeterminismTests.RunBurgr(specs, "/templateSpecDirectory:" + templates);

            var domainProject = File.ReadAllText(Path.Combine(work, "output", "Shop.Domain", "Shop.Domain.csproj"));
            StringAssert.Contains(domainProject, "<AssemblyName>Acme.Shop.Domain</AssemblyName>");
            StringAssert.Contains(domainProject, """<ProjectReference Include="../Shop.Contracts/Shop.Contracts.csproj" />""");
            Assert.IsTrue(File.Exists(Path.Combine(work, "output", "Shop.Domain", "ShopModel.g.cs")));
            StringAssert.Contains(File.ReadAllText(Path.Combine(work, "output", "Shop", "Shop.csproj")), "Microsoft.NET.Sdk.Web", "the other folders keep the ProjectFile of the manifest");
        });
    }

    [TestMethod]
    public void DifferentProjectFilesForTheSameProjectAreRefused()
    {
        DeterminismTests.InWorkDirectory(work =>
        {
            var templates = CopyMiniTemplates(work);
            SeparateProject(templates, "Domain", "Domain", "Domain.csproj", DomainProject);
            SeparateProject(templates, "Contracts", "Domain", "Other.csproj", "<Project Sdk=\"Microsoft.NET.Sdk\" />");
            var specs = WriteSpecs(work);

            var (_, output) = DeterminismTests.StartBurgr(specs, "/templateSpecDirectory:" + templates);

            StringAssert.Contains(output, "Two template folders declare a different project_file for ");
            StringAssert.Contains(output, "Shop.Domain.csproj");
        });
    }

    [TestMethod]
    public void MissingProjectFileIsRefused()
    {
        DeterminismTests.InWorkDirectory(work =>
        {
            var templates = CopyMiniTemplates(work);
            File.AppendAllText(Path.Combine(templates, "Domain", "burgr.yaml"), "\nproject_file: Missing.csproj\n");
            var specs = WriteSpecs(work);

            var (_, output) = DeterminismTests.StartBurgr(specs, "/templateSpecDirectory:" + templates);

            StringAssert.Contains(output, "project_file 'Missing.csproj' not found in");
        });
    }

    private static string CopyMiniTemplates(string work)
    {
        var templates = Path.Combine(work, "templates");
        DeterminismTests.CopyDirectory(Path.Combine(DeterminismTests.GetMetadata("TemplatesDirectory"), "Mini", "templates"), templates);
        return templates;
    }

    // the templates of folder write to the separate project <module>.<destinationSuffix>, with projectFile
    private static void SeparateProject(string templates, string folder, string destinationSuffix, string projectFile, string content)
    {
        var yamlPath = Path.Combine(templates, folder, "burgr.yaml");
        var yaml = File.ReadAllText(yamlPath)
            .Replace("force_separate_dll: false", "force_separate_dll: true")
            .Replace("destination_suffix: " + folder, "destination_suffix: " + destinationSuffix);
        File.WriteAllText(yamlPath, yaml + $"\nproject_file: {projectFile}\n");
        File.WriteAllText(Path.Combine(templates, folder, projectFile), content);
    }

    private static string WriteSpecs(string work)
    {
        var specs = Path.Combine(work, "specs");
        Directory.CreateDirectory(specs);
        File.WriteAllText(Path.Combine(specs, "burgr.json"), MinimalConfig);
        File.WriteAllText(Path.Combine(specs, "model.yaml"), Model);
        return specs;
    }
}
