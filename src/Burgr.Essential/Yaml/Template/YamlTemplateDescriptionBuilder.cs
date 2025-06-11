using SolidOps.Burgr.Core;
using SolidOps.Burgr.Core.Generators;
using SolidOps.Burgr.Core.Template;

namespace SolidOps.Burgr.Essential.Yaml.Template;

public class YamlTemplateDescriptionBuilder
{
    private readonly string templateSpecDirectory;
    private readonly List<string> templateDirectoryPaths;
    private readonly List<string> templateZipPaths;

    public YamlTemplateDescriptionBuilder(string templateSpecDirectory, List<string> templateDirectoryPaths, List<string> templateZipPaths)
    {
        this.templateSpecDirectory = templateSpecDirectory;
        this.templateDirectoryPaths = templateDirectoryPaths;
        this.templateZipPaths = templateZipPaths;
    }

    public List<SourceTemplate> BuildTemplateDescription(Dictionary<string, IGenerator> generators, string filter)
    {
        List<SourceTemplate> sourceTemplates = new();

        foreach (var templateDirectory in templateDirectoryPaths)
        {
            Console.WriteLine(templateDirectory);
            string overrideDestination = null;
            foreach (KeyValuePair<string, string> kvp in GeneratorOptions.OverrideDestinations)
            {
                if (templateDirectory.EndsWith("." + kvp.Key))
                {
                    overrideDestination = kvp.Value;
                    break;
                }
            }

            var content = File.ReadAllText(templateDirectory + "\\burgr.yaml");
            var yamlContent = new YamlDotNet.Serialization.Deserializer().Deserialize<YamlTemplateContentV1>(content);

            foreach (var source_template in yamlContent.source_templates)
            {
                string fileContent = File.ReadAllText(templateDirectory + "\\" + source_template.Value.source);
                var sourceTemplate = new SourceTemplate(fileContent)
                {
                    FileSuffix = source_template.Value.file_suffix,
                    DestinationPath = source_template.Value.destination_path ?? GeneratorOptions.BuildingDirectory,
                    OverrideDestinationFolder = overrideDestination,
                    DestinationSuffix = source_template.Value.destination_suffix,
                    ForceSeparateDll = source_template.Value.force_separate_dll,
                    DestinationLanguage = source_template.Value.destination_language,
                    PathStyle = source_template.Value.path_style,
                    IsGereratePerModelDescription = source_template.Value.is_generate_per_model_description,
                    ChildrenLevel = source_template.Value.children_level,
                };
                if (source_template.Value.options != null)
                {
                    sourceTemplate.SourceTemplateOptions = new SourceTemplateOptions() 
                    {
                        ModelPrefix = source_template.Value.options.model_prefix,
                        ModelSuffix = source_template.Value.options.model_suffix ?? string.Empty
                    };
                }
                sourceTemplates.Add(sourceTemplate);
            }
        }

        SourceTemplateFiller stf = new(generators);
        stf.FillSourceTemplates(sourceTemplates, filter);

        return sourceTemplates;
    }
}
