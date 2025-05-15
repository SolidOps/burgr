using SolidOps.Burgr.Core.Descriptors;
using SolidOps.Burgr.Core.Generators;

namespace SolidOps.Burgr.Essential.Generators.Common;

public class ConsumedEventGenerator : BaseNORADGenerator, IGenerator
{
    public static string Name = "ConsumedEvent";
    public override string DescriptorType => Name;

    public ConsumedEventGenerator()
    {
        TemplateParser = new ConsumedEventTemplateParser();
    }

    public override string Generate(string content, ModelDescriptor model, TemplateDescriptor template, string modelPrefix, string modelSuffix)
    {
        string result = base.Generate(content, model, template, modelPrefix, modelSuffix);
        if (result == string.Empty)
            return result;
        switch (ModelParserType)
        {
            case "Yaml":
                var typeInfo = new SolidOps.Burgr.Essential.Yaml.Model.TypeInfo(model.Name, model.ModuleName);

                result = result.Replace("CONSUMEDEVENTTYPE", $"{typeInfo.ModuleName}.Contracts.Events.{ConversionHelper.ConvertToPascalCase(typeInfo.Name)}");
                break;
            default:
                result = result.Replace("CONSUMEDEVENTTYPE", model.Name);
                break;
        }
        return result;
    }
}

public class ConsumedEventTemplateParser : ITemplateParser
{
    public string LoopIdentifier => "foreach CONSUMEDEVENT";

    public List<TemplateOption> Options { get; } = new List<TemplateOption>();

    public List<string> AdditionalLoopIdentifiers => new List<string>();
}