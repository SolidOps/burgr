using SolidOps.Burgr.Core.Descriptors;
using SolidOps.Burgr.Core.Generators;

namespace SolidOps.Burgr.Essential.Generators.Rights;

public class RightGenerator : BaseNORADGenerator, IGenerator
{
    public static string Name = "Right";
    public override string DescriptorType => Name;

    public RightGenerator()
    {
        TemplateParser = new RightTemplateParser();
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
}
