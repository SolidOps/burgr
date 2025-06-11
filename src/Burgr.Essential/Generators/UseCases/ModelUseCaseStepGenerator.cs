using SolidOps.Burgr.Core;
using SolidOps.Burgr.Core.Descriptors;
using SolidOps.Burgr.Core.Generators;
using SolidOps.SubZero;

namespace SolidOps.Burgr.Essential.Generators.UseCases;

public class ModelUseCaseStepGenerator : BaseUseCaseStepGenerator, IGenerator
{
    public static string Name = "ModelMethod";
    public override string DescriptorType => Name;

    public ModelUseCaseStepGenerator()
    {
        TemplateParser = new ModelMethodTemplateParser();
    }
    protected override string CheckIfApply(ModelDescriptor model, TemplateDescriptor template)
    {
        return model.Get("ReturnType") != ReturnType.Model.ToString() ? "use case step is not model" : null;
    }

    public override string Generate(string content, ModelDescriptor model, TemplateDescriptor template, string modelPrefix, string modelSuffix)
    {
        var conversionService = ConversionServices[template.DestinationLanguage];
        string result = base.Generate(content, model, template, modelPrefix, modelSuffix);
        if (result == string.Empty)
            return result;
        ModelDescriptor step = model;
        ModelDescriptor useCase = model.Parent;

        string language = template.DestinationLanguage;

        ModelDescriptor resultType = model.GetRelated("Object");
        if (language == "JS" || language == "HTML")
        {
            result = result.Replace("_PROPERTYTYPE_", conversionService.ConvertRelatedParameterType(model, resultType, null, null, false));
            result = result.Replace("_PROPERTYINTERFACE_", conversionService.ConvertRelatedParameterType(model, resultType, null, null, false, true));
        }
        else
        {
            result = result.Replace("_PROPERTYTYPE_", conversionService.ConvertRelatedParameterType(model, resultType, null, null));
            result = resultType.FullModuleName.Contains("SolidOps.Common.")
                ? result.Replace("_PROPERTYINTERFACE_", conversionService.ConvertRelatedParameterType(model, resultType, null, null, false, false))
                : result.Replace("_PROPERTYINTERFACE_", conversionService.ConvertRelatedParameterType(model, resultType, null, null, false, true));

            result = result.Replace("DEPENDENCYNAMESPACE", Utilities.GetNamespace(resultType.FullModuleName));
        }
        result = result.Replace("_DOMODELACTION_", ConversionHelper.ConvertToPascalCase(step.Name));
        result = result.Replace("_DOMODELACTIONURL_", TextHelper.GenerateSlug(step.Name));

        result = ReplaceParameters(useCase, conversionService, step, result, modelPrefix, modelSuffix, out _);

        // methods with return are always get unless post is forces
        result = step.Is("ForcePost") ? result.Replace("_VERB_", "Post") : result.Replace("_VERB_", "Get");

        result = step.Is("NoTransaction") ? result.Replace("_NOTRAN_", "WithoutTransaction") : result.Replace("_NOTRAN_", "");

        result = result.Replace("UNITOFWORKTYPE", "Read"); // Unitofwork type are always Command

        result = DomainTypeHelper.ReplaceDomainType(result, resultType.Get("DomainType"));

        return result;
    }
}

public class ModelMethodTemplateParser : ITemplateParser
{
    public string LoopIdentifier => "foreach STEP_IN_USECASE_WITH_MODEL_RETURN";
    public List<string> AdditionalLoopIdentifiers => new List<string>();

    public List<TemplateOption> Options { get; } = new List<TemplateOption>();
}
