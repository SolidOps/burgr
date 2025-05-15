using SolidOps.Burgr.Core.Descriptors;
using SolidOps.Burgr.Core.Generators;

namespace SolidOps.Burgr.Essential.Generators.Services;

public class ParameterGenerator : BaseNORADGenerator, IGenerator
{
    public static string Name = DescriptorTypes.SERVICE_METHOD_PARAMETER_DESCRIPTOR;
    public override string DescriptorType => Name;

    public ParameterGenerator()
    {
        TemplateParser = new ParameterTemplateParser();
    }

    protected override string CheckIfApply(ModelDescriptor model, TemplateDescriptor template)
    {
        return null;
    }

    public override string Generate(string content, ModelDescriptor model, TemplateDescriptor template, string modelPrefix, string modelSuffix)
    {
        string result = base.Generate(content, model, template, modelPrefix, modelSuffix);

        result = result.Replace("_PARAMETER_", model.Name);
        result = result.Replace("_PARAMTYPE_", ConversionServices[template.DestinationLanguage].ConvertParameterType(model, modelPrefix, modelSuffix, false, false));
        return result;
    }
}

public class ParameterTemplateParser : ITemplateParser
{
    public string LoopIdentifier => "foreach SERVICE_METHOD_PARAMETER";
    public List<string> AdditionalLoopIdentifiers => new List<string>();

    public List<TemplateOption> Options { get; } = new List<TemplateOption>();

    public ParameterTemplateParser()
    {
        
    }
}
