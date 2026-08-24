using SolidOps.Burgr.Core.Descriptors;
using SolidOps.Burgr.Core.Generators;

namespace SolidOps.Burgr.Essential.Generators.Rights;

public class RightGenerator : BaseBurgrGenerator, IGenerator
{
    public static string Name = "Right";
    public const string Wildcard = "*";
    public override string DescriptorType => Name;

    public RightGenerator()
    {
        TemplateParser = new RightTemplateParser();
    }

    // the wildcard right is not a usable identifier, so a template emitting declarations
    // has to be able to skip it while one emitting values still sees it
    protected override string CheckIfApply(ModelDescriptor model, TemplateDescriptor template)
    {
        if (template.Is("NotWildcard") && model.Name == Wildcard)
            return "wildcard right";

        return null;
    }

    public override string Generate(string content, ModelDescriptor model, TemplateDescriptor template, string modelPrefix, string modelSuffix)
    {
        string result = base.Generate(content, model, template, modelPrefix, modelSuffix);
        if (result == string.Empty)
            return result;
        result = result.Replace("_RIGHT_", ConversionHelper.ConvertToPascalCase(model.Name));

        return result;
    }
}

public class RightTemplateParser : ITemplateParser
{
    public string LoopIdentifier => "foreach RIGHT";
    public List<string> AdditionalLoopIdentifiers => new List<string>();

    public List<TemplateOption> Options { get; } = new List<TemplateOption>();

    public RightTemplateParser()
    {
        Options.Add(new TemplateOption() { Name = "NotWildcard", Tag = "[NW]" });
    }
}
