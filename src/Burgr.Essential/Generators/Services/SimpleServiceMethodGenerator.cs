using SolidOps.Burgr.Core.Descriptors;
using SolidOps.Burgr.Core.Generators;
using SolidOps.SubZero;

namespace SolidOps.Burgr.Essential.Generators.Services;

public class SimpleServiceMethodGenerator : BaseServiceMethodGenerator, IGenerator
{
    public static string Name = "SimpleMethod";
    public override string DescriptorType => Name;

    public SimpleServiceMethodGenerator()
    {
        TemplateParser = new SimpleMethodTemplateParser();
    }

    protected override string CheckIfApply(ModelDescriptor model, TemplateDescriptor template)
    {
        var result = base.CheckIfApply(model, template);
        if (result != null)
        {
            return result;
        }

        return model.Get("ReturnType") != ReturnType.Simple.ToString() ? "service method is not simple" : null;
    }

    public override string Generate(string content, ModelDescriptor model, TemplateDescriptor template, string modelPrefix, string modelSuffix)
    {
        var conversionService = ConversionServices[template.DestinationLanguage];
        string result = base.Generate(content, model, template, modelPrefix, modelSuffix);
        if (result == string.Empty)
        {
            return result;
        }

        ModelDescriptor method = model;
        ModelDescriptor service = model.Parent;

        Type resultType = conversionService.GetSimpleType(method.Get("SimpleType"));
        string typeName = resultType.Name;
        string fullName = resultType.FullName;
        if (method.Is("List"))
        {
            typeName += "[]";
            fullName += "[]";
        }
        if (method.Is("Null"))
        {
            typeName += "?";
            fullName += "?";
        }
        result = result.Replace("_SIMPLE__TYPE_", conversionService.ConvertParameterType(typeName, fullName, resultType.IsEnum, false, method.NamespaceName, method.ModuleName, null, null));
        result = result.Replace("_DOSIMPLEACTION_", ConversionHelper.ConvertToPascalCase(method.Name));
        result = result.Replace("_DOSIMPLEACTIONURL_", TextHelper.GenerateSlug(method.Name));

        result = ReplaceParameters(service, conversionService, method, result, modelPrefix, modelSuffix, out _);

        // methods with return are always get unless post is forces
        result = method.Is("ForcePost") ? result.Replace("_VERB_", "Post") : result.Replace("_VERB_", "Get");

        result = method.Is("NoTransaction") ? result.Replace("_NOTRAN_", "WithoutTransaction") : result.Replace("_NOTRAN_", "");

        if (method.Is("AllowWrite"))
        {
            result = result.Replace("UNITOFWORKTYPE", "Write");
        }
        else
        {
            result = result.Replace("UNITOFWORKTYPE", "Read");
        }

        return result;
    }
}

public class SimpleMethodTemplateParser : ITemplateParser
{
    public string LoopIdentifier => "foreach METHOD_IN_SERVICE_WITH_SIMPLE_RETURN";
    public List<string> AdditionalLoopIdentifiers => new List<string>();

    public List<TemplateOption> Options { get; } = new List<TemplateOption>();

    public SimpleMethodTemplateParser()
    {
        Options.Add(new TemplateOption() { Name = "Component", Tag = "[C]" });
        Options.Add(new TemplateOption() { Name = "External", Tag = "[EXT]" });
        Options.Add(new TemplateOption() { Name = "Anonymous", Tag = "[A]" });
        Options.Add(new TemplateOption() { Name = "NonAnonymous", Tag = "[-A]" });
    }
}
