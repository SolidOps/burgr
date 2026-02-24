using SolidOps.Burgr.Core.Descriptors;
using SolidOps.Burgr.Core.Generators;
using SolidOps.SubZero;

namespace SolidOps.Burgr.Essential.Generators.Services;

public class IdentityServiceMethodGenerator : BaseServiceMethodGenerator, IGenerator
{
    public static string Name = "IdentityMethod";
    public override string DescriptorType => Name;

    public IdentityServiceMethodGenerator()
    {
        TemplateParser = new IdentityMethodTemplateParser();
    }

    protected override string CheckIfApply(ModelDescriptor model, TemplateDescriptor template)
    {
        var result = base.CheckIfApply(model, template);
        if (result != null)
        {
            return result;
        }

        return model.Get("ReturnType") != ReturnType.Identity.ToString() ? "service method is not identity" : null;
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

        result = result.Replace("_DOIDENTITYACTION_", ConversionHelper.ConvertToPascalCase(method.Name));
        result = result.Replace("_DOIDENTITYACTIONURL_", TextHelper.GenerateSlug(method.Name));

        result = ReplaceParameters(service, conversionService, method, result, modelPrefix, modelSuffix, out _);

        result = result.Replace("_VERB_", "Post"); // void method are always post

        result = method.Is("NoTransaction") ? result.Replace("_NOTRAN_", "WithoutTransaction") : result.Replace("_NOTRAN_", "");

        result = result.Replace("UNITOFWORKTYPE", "Write"); // Unitofwork type are always Write

        return result;
    }
}

public class IdentityMethodTemplateParser : ITemplateParser
{
    public string LoopIdentifier => "foreach METHOD_IN_SERVICE_WITH_IDENTITY_RETURN";
    public List<string> AdditionalLoopIdentifiers => new List<string>();

    public List<TemplateOption> Options { get; } = new List<TemplateOption>();

    public IdentityMethodTemplateParser()
    {
        Options.Add(new TemplateOption() { Name = "Anonymous", Tag = "[A]" });
        Options.Add(new TemplateOption() { Name = "NonAnonymous", Tag = "[-A]" });
        Options.Add(new TemplateOption() { Name = "Component", Tag = "[C]" });
        Options.Add(new TemplateOption() { Name = "External", Tag = "[EXT]" });
    }
}
