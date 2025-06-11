using SolidOps.Burgr.Core.Descriptors;
using SolidOps.Burgr.Core.Generators;
using SolidOps.Burgr.Essential.Generators.Objects;
using SolidOps.SubZero;

namespace SolidOps.Burgr.Essential.Generators.UseCases;

public class SimpleUseCaseStepGenerator : BaseUseCaseStepGenerator, IGenerator
{
    public static string Name = "SimpleMethod";
    public override string DescriptorType => Name;

    public SimpleUseCaseStepGenerator()
    {
        TemplateParser = new SimpleMethodTemplateParser();
    }

    protected override string CheckIfApply(ModelDescriptor model, TemplateDescriptor template)
    {
        return model.Get("ReturnType") != ReturnType.Simple.ToString() ? "use case step is not simple" : null;
    }

    public override string Generate(string content, ModelDescriptor model, TemplateDescriptor template, string modelPrefix, string modelSuffix)
    {
        var conversionService = ConversionServices[template.DestinationLanguage];
        string result = base.Generate(content, model, template, modelPrefix, modelSuffix);
        if (result == string.Empty)
            return result;
        ModelDescriptor step = model;
        ModelDescriptor useCase = model.Parent;

        Type resultType = conversionService.GetSimpleType(step.Get("SimpleType"));
        string typeName = resultType.Name;
        string fullName = resultType.FullName;
        if (step.Is("List"))
        {
            typeName += "[]";
            fullName += "[]";
        }
        if (step.Is("Null"))
        {
            typeName += "?";
            fullName += "?";
        }
        result = result.Replace("_SIMPLE__TYPE_", conversionService.ConvertParameterType(typeName, fullName, resultType.IsEnum, step.NamespaceName, step.ModuleName, null, null));
        result = result.Replace("_DOSIMPLEACTION_", ConversionHelper.ConvertToPascalCase(step.Name));
        result = result.Replace("_DOSIMPLEACTIONURL_", TextHelper.GenerateSlug(step.Name));

        result = ReplaceParameters(useCase, conversionService, step, result, modelPrefix, modelSuffix, out _);

        // methods with return are always get unless post is forces
        result = step.Is("ForcePost") ? result.Replace("_VERB_", "Post") : result.Replace("_VERB_", "Get");

        result = step.Is("NoTransaction") ? result.Replace("_NOTRAN_", "WithoutTransaction") : result.Replace("_NOTRAN_", "");

        result = result.Replace("UNITOFWORKTYPE", "Read"); // Unitofwork type are always Command

        return result;
    }
}

public class SimpleMethodTemplateParser : ITemplateParser
{
    public string LoopIdentifier => "foreach STEP_IN_USECASE_WITH_SIMPLE_RETURN";
    public List<string> AdditionalLoopIdentifiers => new List<string>();

    public List<TemplateOption> Options { get; } = new List<TemplateOption>();
}
