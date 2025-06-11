using SolidOps.Burgr.Core;
using SolidOps.Burgr.Core.Descriptors;
using SolidOps.Burgr.Core.Generators;

namespace SolidOps.Burgr.Essential.Generators.Enums;

public class EnumGenerator : BaseBurgrGenerator, IGenerator
{
    public static string Name = "Enum";
    public override string DescriptorType => Name;

    public EnumGenerator()
    {
        TemplateParser = new EnumTemplateParser();
    }

    protected override IModelParser InstantiateModelParser(string modelParserType)
    {
        return new Yaml.Model.Enums.EnumModelParser();
    }

    public override string Generate(string content, ModelDescriptor model, TemplateDescriptor template, string modelPrefix, string modelSuffix)
    {
        string result = base.Generate(content, model, template, modelPrefix, modelSuffix);
        if (result == string.Empty)
            return result;
        bool firstDone = false;
        string enumValues = string.Empty;
        foreach (ModelDescriptor enumValue in model.GetChildren())
        {
            if (firstDone)
            {
                enumValues += "," + Utilities.SingleNewLine;
            }

            enumValues += enumValue.Name + " = " + enumValue.Get("Key");

            if (!firstDone)
            {
                firstDone = true;
            }
        }

        result = result.Replace("VALUES", enumValues);
        result = result.Replace("_ENUMTYPE_", ConversionHelper.ConvertToPascalCase(model.Get("EnumType")));

        return result;
    }
}

public class EnumTemplateParser : ITemplateParser
{
    public string LoopIdentifier => "foreach ENUMTYPE";
    public List<string> AdditionalLoopIdentifiers => new List<string>();

    public List<TemplateOption> Options { get; } = new List<TemplateOption>();
}
