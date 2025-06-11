using SolidOps.Burgr.Core.Descriptors;
using SolidOps.Burgr.Core.Generators;
using SolidOps.SubZero;

namespace SolidOps.Burgr.Essential.Generators.UseCases;

public class IdentityUseCaseStepGenerator : BaseUseCaseStepGenerator, IGenerator
{
    public static string Name = "IdentityMethod";
    public override string DescriptorType => Name;

    public IdentityUseCaseStepGenerator()
    {
        TemplateParser = new IdentityMethodTemplateParser();
    }

    protected override string CheckIfApply(ModelDescriptor model, TemplateDescriptor template)
    {
        return model.Get("ReturnType") != ReturnType.Identity.ToString() ? "use case step is not identity" : null;
    }

    public override string Generate(string content, ModelDescriptor model, TemplateDescriptor template, string modelPrefix, string modelSuffix)
    {
        var conversionService = ConversionServices[template.DestinationLanguage];
        string result = base.Generate(content, model, template, modelPrefix, modelSuffix);
        if (result == string.Empty)
            return result;
        ModelDescriptor step = model;
        ModelDescriptor useCase = model.Parent;

        result = result.Replace("_DOIDENTITYACTION_", ConversionHelper.ConvertToPascalCase(step.Name));
        result = result.Replace("_DOIDENTITYACTIONURL_", TextHelper.GenerateSlug(step.Name));

        result = ReplaceParameters(useCase, conversionService, step, result, modelPrefix, modelSuffix, out _);

        result = result.Replace("_VERB_", "Post"); // void method are always post

        result = step.Is("NoTransaction") ? result.Replace("_NOTRAN_", "WithoutTransaction") : result.Replace("_NOTRAN_", "");

        result = result.Replace("UNITOFWORKTYPE", "Write"); // Unitofwork type are always Command

        return result;
    }
}

public class IdentityMethodTemplateParser : ITemplateParser
{
    public string LoopIdentifier => "foreach STEP_IN_USECASE_WITH_IDENTITY_RETURN";
    public List<string> AdditionalLoopIdentifiers => new List<string>();

    public List<TemplateOption> Options { get; } = new List<TemplateOption>();
}
