using SolidOps.Burgr.Core;
using SolidOps.Burgr.Core.Descriptors;
using SolidOps.Burgr.Core.Generators;

namespace SolidOps.Burgr.Essential.Generators.Common;

public class DependencyGenerator : BaseNORADGenerator, IGenerator
{
    public static string Name = "Dependency";
    public override string DescriptorType => Name;

    public DependencyGenerator()
    {
        TemplateParser = new DependencyTemplateParser();
    }

    protected override string CheckIfApply(ModelDescriptor model, TemplateDescriptor template)
    {
        if (template.Is("Aggregate") || template.Is("Entity") || template.Is("Transient") || template.Is("ValueObject"))
        {
            var related = model.GetRelated("Object");
            if (!template.Is("Aggregate") && related.Get("DomainType") == "Aggregate")
            {
                return "Template not aggregate root";
            }

            if (!template.Is("Entity") && related.Get("DomainType") == "Entity")
            {
                return "Template not entity";
            }

            if (!template.Is("ValueObject") && related.Get("DomainType") == "ValueObject")
            {
                return "Template not ValueObject";
            }

            if (!template.Is("Transient") && related.Get("DomainType") == "Transient")
            {
                return "Template not Transient";
            }
        }
        else
        {
            return null;
        }

        return null;
    }

    public override string Generate(string content, ModelDescriptor model, TemplateDescriptor template, string modelPrefix, string modelSuffix)
    {
        string result = base.Generate(content, model, template, modelPrefix, modelSuffix);
        if (result == string.Empty)
            return result;

        var related = model.GetRelated("Object");
        result = result.Replace("DEPENDENCYTYPE", ConversionHelper.ConvertToPascalCase(related.Name));
        result = result.Replace("DEPENDENCYNAMESPACE", ConversionHelper.ConvertToPascalCase(related.FullModuleName));

        return result;
    }
}

public class DependencyTemplateParser : ITemplateParser
{
    public string LoopIdentifier => "foreach DEPENDENCY";

    public List<TemplateOption> Options { get; } = new List<TemplateOption>();

    public List<string> AdditionalLoopIdentifiers => new List<string>();

    public DependencyTemplateParser()
    {
        Options.Add(new TemplateOption() { Name = "Aggregate", Tag = "[AG]" });
        Options.Add(new TemplateOption() { Name = "Entity", Tag = "[EN]" });
        Options.Add(new TemplateOption() { Name = "Transient", Tag = "[TR]" });
        Options.Add(new TemplateOption() { Name = "ValueObject", Tag = "[VO]", });
    }
}