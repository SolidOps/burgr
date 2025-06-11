using SolidOps.Burgr.Core.Descriptors;
using SolidOps.Burgr.Core.Generators;

namespace SolidOps.Burgr.Essential.Generators.Objects;

public class DependentForGenerator : BaseBurgrGenerator, IGenerator
{
    public static string Name = "DependentFor";
    public override string DescriptorType => Name;

    public DependentForGenerator()
    {
        TemplateParser = new DependentForTemplateParser();
    }

    protected override string CheckIfApply(ModelDescriptor model, TemplateDescriptor template)
    {
        if (template.Is("Aggregate") || template.Is("Entity") || template.Is("ValueObject") || template.Is("Transient"))
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
        return null;
    }

    public override string Generate(string content, ModelDescriptor model, TemplateDescriptor template, string modelPrefix, string modelSuffix)
    {
        string result = base.Generate(content, model, template, modelPrefix, modelSuffix);
        if (result == string.Empty)
            return result;
        var related = model.GetRelated("Object");
        result = result.Replace("DEPENDENCYNAMESPACE", ConversionHelper.ConvertToPascalCase(related.FullModuleName));
        result = result.Replace("_PROPERTYFULLINTERFACE_", "I" + ConversionHelper.ConvertToPascalCase(related.Name));
        result = result.Replace("_PROPERTYNAME_", ConversionHelper.ConvertToPascalCase(model.Name));


        return result;
    }
}

public class DependentForTemplateParser : ITemplateParser
{
    public string LoopIdentifier => "foreach DEPENDENTFOR";
    public List<string> AdditionalLoopIdentifiers => new List<string>();

    public List<TemplateOption> Options { get; } = new List<TemplateOption>();

    public DependentForTemplateParser()
    {
        Options.Add(new TemplateOption() { Name = "Aggregate", Tag = "[AG]" });
        Options.Add(new TemplateOption() { Name = "Entity", Tag = "[EN]" });
        Options.Add(new TemplateOption() { Name = "Transient", Tag = "[TR]" });
        Options.Add(new TemplateOption() { Name = "ValueObject", Tag = "[VO]", });
    }
}