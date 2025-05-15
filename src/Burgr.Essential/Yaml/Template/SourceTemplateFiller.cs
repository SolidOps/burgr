using SolidOps.Burgr.Core;
using SolidOps.Burgr.Core.Descriptors;
using SolidOps.Burgr.Core.Generators;
using SolidOps.Burgr.Core.Template;

namespace SolidOps.Burgr.Essential.Yaml.Template;

public class SourceTemplateFiller
{
    private readonly Dictionary<string, IGenerator> generators;

    public SourceTemplateFiller(Dictionary<string, IGenerator> generators)
    {
        this.generators = generators;
    }

    public void FillSourceTemplates(List<SourceTemplate> sourceTemplates, string filter = null)
    {
        foreach (SourceTemplate sourceTemplate in sourceTemplates)
        {
            if (filter != null && !sourceTemplate.FileSuffix.Contains(filter))
            {
                continue;
            }

            sourceTemplate.TemplateDescriptors = new List<TemplateDescriptor>();

            foreach (IGenerator generator in generators.Values)
            {
                ITemplateParser templateParser = generator.TemplateParser;
                if (templateParser != null)
                {
                    foreach (string templateContent in Utilities.GetInnerTemplates(sourceTemplate.Content, Utilities.GetLoopIdentifiers(templateParser.LoopIdentifier)))
                    {
                        TemplateDescriptor templateDescriptor = new()
                        {
                            Content = templateContent,
                            DescriptorType = generator.DescriptorType,
                            DestinationLanguage = sourceTemplate.DestinationLanguage
                        };
                        AddAttributes(generator, templateContent, templateDescriptor);
                        HandleSubTemplate(generator, templateContent, templateDescriptor);
                        sourceTemplate.TemplateDescriptors.Add(templateDescriptor);
                    }
                }
            }

        }
    }

    private void HandleSubTemplate(IGenerator generator, string templateContent, TemplateDescriptor parentDescriptor)
    {
        foreach (IGenerator subGenerator in generator.SubGenerators)
        {
            foreach (string subTemplate in Utilities.GetInnerTemplates(templateContent, Utilities.GetLoopIdentifiers(subGenerator.TemplateParser.LoopIdentifier)))
            {
                TemplateDescriptor templateDescriptor = new()
                {
                    Content = subTemplate,
                    DescriptorType = subGenerator.DescriptorType,
                    DestinationLanguage = parentDescriptor.DestinationLanguage
                };
                AddAttributes(subGenerator, subTemplate, templateDescriptor);
                HandleSubTemplate(subGenerator, subTemplate, templateDescriptor);
                parentDescriptor.AddChild(templateDescriptor);
            }

        }
    }

    private static void AddAttributes(IGenerator generator, string templateContent, TemplateDescriptor templateDescriptor)
    {
        string firstLine = templateContent.Split("\n")[0];
        foreach (TemplateOption option in generator.TemplateParser.Options)
        {
            templateDescriptor.Attributes[option.Name] = firstLine.Contains(option.Tag) ? "true" : "false";
            if (option.Tag.StartsWith("[") && option.Tag.EndsWith("]"))
            {
                var innerTag = option.Tag.Substring(1, option.Tag.Length - 2);
                var notTag = $"[!{innerTag}]";
                if (firstLine.Contains(notTag))
                {
                    templateDescriptor.Attributes["Not" + option.Name] = "true";
                }
            }
        }
        int i0 = firstLine.IndexOf("[");
        int i1 = firstLine.LastIndexOf("]");
        templateDescriptor.Headers = i0 >= 0 && i1 > i0 ? firstLine.Substring(i0, i1 - i0 + 1) : string.Empty;
    }
}
