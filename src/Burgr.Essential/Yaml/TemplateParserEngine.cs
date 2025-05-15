using SolidOps.Burgr.Core.Generators;
using SolidOps.Burgr.Core.Template;
using SolidOps.Burgr.Essential.Yaml.Template;

namespace SolidOps.Burgr.Essential.Yaml;

public class TemplateParserEngine : ITemplateParserEngine
{
    public string TemplatesDirectory { get; set; }
    public string ModuleName { get; set; }
    public string NamespaceName { get; set; }

    public string TemplateParserType { get => "Yaml"; }

    public TemplateParserEngine()
    {

    }

    public TemplateParserEngine(string templatesDirectory)
    {
        TemplatesDirectory = templatesDirectory;
    }

    public void Dispose()
    {

    }

    public void Init()
    {

    }

    public List<SourceTemplate> Parse(string templatesDirectory, List<string> templates, Dictionary<string, IGenerator> generators, string filter = null)
    {
        List<string> templateDirectoryPaths = new List<string>();
        List<string> templateZipPaths = new List<string>();

        foreach (var subDirectory in Directory.GetDirectories(templatesDirectory))
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(subDirectory);
            if(templates.Contains(directoryInfo.Name))
                templateDirectoryPaths.Add(subDirectory);
        }

        foreach (string filePath in Directory.GetFiles(templatesDirectory, "*.zip"))
        {
            FileInfo fileInfo = new(filePath);
            if (templates.Contains(fileInfo.Name.Substring(0, fileInfo.Name.Length - 4)))
            {
                templateZipPaths.Add(filePath);
            }
        }

        YamlTemplateDescriptionBuilder templateDescriptionBuilder = new(templatesDirectory, templateDirectoryPaths, templateZipPaths);
        return templateDescriptionBuilder.BuildTemplateDescription(generators, filter);
    }
}
