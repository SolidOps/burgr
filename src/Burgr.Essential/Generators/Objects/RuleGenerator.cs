using SolidOps.Burgr.Core.Descriptors;
using SolidOps.Burgr.Core.Generators;

namespace SolidOps.Burgr.Essential.Generators.Objects;

public class RuleGenerator : BaseBurgrGenerator, IGenerator
{
    public static string Name = "Rule";
    public override string DescriptorType => Name;

    public RuleGenerator()
    {
        TemplateParser = new RuleTemplateParser();
    }

    protected override string CheckIfApply(ModelDescriptor model, TemplateDescriptor template)
    {
        foreach(var attribute in template.Attributes)
        {
            if(attribute.Value == "true")
            {
                if (!model.Is(attribute.Key))
                    return "model is not " + attribute.Key;
                break;
            }
        }

        return null;
    }

    public override string Generate(string content, ModelDescriptor model, TemplateDescriptor template, string modelPrefix, string modelSuffix)
    {
        string result = base.Generate(content, model, template, modelPrefix, modelSuffix);
        if (result == string.Empty)
            return result;

        result = result.Replace("_RULE_", ConversionHelper.ConvertToPascalCase(model.Name));

        return result;
    }
}

public class RuleTemplateParser : ITemplateParser
{
    public string LoopIdentifier => "foreach RULE";

    public List<TemplateOption> Options { get; } = new List<TemplateOption>();

    public List<string> AdditionalLoopIdentifiers => new List<string>();

    public RuleTemplateParser()
    {
        Options.Add(new TemplateOption() { Name = "before_add", Tag = "[BA]" });
        Options.Add(new TemplateOption() { Name = "after_add", Tag = "[AA]" });
        Options.Add(new TemplateOption() { Name = "before_update", Tag = "[BU]" });
        Options.Add(new TemplateOption() { Name = "after_update", Tag = "[AU]" });
        Options.Add(new TemplateOption() { Name = "before_remove", Tag = "[BR]" });
        Options.Add(new TemplateOption() { Name = "after_remove", Tag = "[AR]" });
    }
}