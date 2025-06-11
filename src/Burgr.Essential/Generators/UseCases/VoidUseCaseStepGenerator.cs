using SolidOps.Burgr.Core.Descriptors;
using SolidOps.Burgr.Core.Generators;
using SolidOps.SubZero;

namespace SolidOps.Burgr.Essential.Generators.UseCases;

public class VoidUseCaseStepGenerator : BaseUseCaseStepGenerator, IGenerator
{
    public static string Name = "VoidMethod";
    public override string DescriptorType => Name;

    public VoidUseCaseStepGenerator()
    {
        TemplateParser = new VoidMethodTemplateParser();
    }

    protected override string CheckIfApply(ModelDescriptor model, TemplateDescriptor template)
    {
        if (model.Get("ReturnType") != ReturnType.Void.ToString())
            return "use case step is not void";

        if (template.Is("Component") && !model.Is("Component"))
            return "model is not component";
        
        return null;
    }

    public override string Generate(string content, ModelDescriptor model, TemplateDescriptor template, string modelPrefix, string modelSuffix)
    {
        var conversionService = ConversionServices[template.DestinationLanguage];
        string result = base.Generate(content, model, template, modelPrefix, modelSuffix);
        if (result == string.Empty)
            return result;
        ModelDescriptor step = model;
        ModelDescriptor useCase = model.Parent;

        result = result.Replace("_DOVOIDACTION_", ConversionHelper.ConvertToPascalCase(step.Name));
        result = result.Replace("_DOVOIDACTIONURL_", TextHelper.GenerateSlug(step.Name));

        result = ReplaceParameters(useCase, conversionService, step, result, modelPrefix, modelSuffix, out _);

        result = result.Replace("_VERB_", "Post"); // void method are always post

        result = step.Is("NoTransaction") ? result.Replace("_NOTRAN_", "WithoutTransaction") : result.Replace("_NOTRAN_", "");

        result = result.Replace("UNITOFWORKTYPE", "Write"); // Unitofwork type are always Command

        return result;
    }
}

public class VoidMethodTemplateParser : ITemplateParser
{
    public string LoopIdentifier => "foreach STEP_IN_USECASE_WITH_VOID_RETURN";
    public List<string> AdditionalLoopIdentifiers => new List<string>();

    public List<TemplateOption> Options { get; } = new List<TemplateOption>();

    public VoidMethodTemplateParser()
    {
        Options.Add(new TemplateOption() { Name = "Component", Tag = "[C]" });
    }
}
