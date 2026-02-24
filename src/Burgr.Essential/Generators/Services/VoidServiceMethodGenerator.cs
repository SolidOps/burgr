using SolidOps.Burgr.Core.Descriptors;
using SolidOps.Burgr.Core.Generators;
using SolidOps.SubZero;

namespace SolidOps.Burgr.Essential.Generators.Services;

public class VoidServiceMethodGenerator : BaseServiceMethodGenerator, IGenerator
{
    public static string Name = "VoidMethod";
    public override string DescriptorType => Name;

    public VoidServiceMethodGenerator()
    {
        TemplateParser = new VoidMethodTemplateParser();
    }

    protected override string CheckIfApply(ModelDescriptor model, TemplateDescriptor template)
    {
        var result = base.CheckIfApply(model, template);
        if (result != null)
        {
            return result;
        }

        if (model.Get("ReturnType") != ReturnType.Void.ToString())
        {
            return "service method is not void";
        }

        if (template.Is("Component") && !model.Is("Component"))
        {
            return "model is not component";
        }

        return null;
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

        result = result.Replace("_DOVOIDACTION_", ConversionHelper.ConvertToPascalCase(method.Name));
        result = result.Replace("_DOVOIDACTIONURL_", TextHelper.GenerateSlug(method.Name));

        result = ReplaceParameters(service, conversionService, method, result, modelPrefix, modelSuffix, out _);

        result = result.Replace("_VERB_", "Post"); // void method are always post

        result = method.Is("NoTransaction") ? result.Replace("_NOTRAN_", "WithoutTransaction") : result.Replace("_NOTRAN_", "");

        result = result.Replace("UNITOFWORKTYPE", "Write");

        return result;
    }
}

public class VoidMethodTemplateParser : ITemplateParser
{
    public string LoopIdentifier => "foreach METHOD_IN_SERVICE_WITH_VOID_RETURN";
    public List<string> AdditionalLoopIdentifiers => new List<string>();

    public List<TemplateOption> Options { get; } = new List<TemplateOption>();

    public VoidMethodTemplateParser()
    {
        Options.Add(new TemplateOption() { Name = "Component", Tag = "[C]" });
        Options.Add(new TemplateOption() { Name = "External", Tag = "[EXT]" });
        Options.Add(new TemplateOption() { Name = "Anonymous", Tag = "[A]" });
        Options.Add(new TemplateOption() { Name = "NonAnonymous", Tag = "[-A]" });
    }
}
