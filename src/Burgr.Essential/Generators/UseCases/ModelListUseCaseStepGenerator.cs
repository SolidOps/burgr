using SolidOps.Burgr.Core;
using SolidOps.Burgr.Core.Descriptors;
using SolidOps.Burgr.Core.Generators;
using SolidOps.SubZero;

namespace SolidOps.Burgr.Essential.Generators.UseCases;

public class ModelListUseCaseStepGenerator : BaseUseCaseStepGenerator, IGenerator
{
    public static string Name = "ModelListMethod";
    public override string DescriptorType => Name;

    public ModelListUseCaseStepGenerator()
    {
        TemplateParser = new ModelListMethodTemplateParser();
    }

    protected override string CheckIfApply(ModelDescriptor step, TemplateDescriptor template)
    {
        return step.Get("ReturnType") != ReturnType.ModelList.ToString() ? "use case step is not model list" : null;
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
            result = result.Replace("_PROPERTYTYPE_", conversionService.ConvertRelatedParameterType(model, resultType, null, null));
            result = result.Replace("_PROPERTYINTERFACE_", conversionService.ConvertRelatedParameterType(model, resultType, null, null, false, true));
        }
        else
        {
            result = result.Replace("_PROPERTYTYPE_", conversionService.ConvertRelatedParameterType(model, resultType, null, null));
            result = result.Replace("_PROPERTYINTERFACE_", conversionService.ConvertRelatedParameterType(model, resultType, null, null, false, true));

            result = result.Replace("DEPENDENCYNAMESPACE", Utilities.GetNamespace(step.FullModuleName));
        }
        result = result.Replace("_DOMODELLISTACTION_", ConversionHelper.ConvertToPascalCase(step.Name));
        result = result.Replace("_DOMODELLISTACTIONURL_", TextHelper.GenerateSlug(step.Name));

        result = ReplaceParameters(useCase, conversionService, step, result, modelPrefix, modelSuffix, out _);

        // methods with return are always get unless post is forces
        result = step.Is("ForcePost") ? result.Replace("_VERB_", "Post") : result.Replace("_VERB_", "Get");

        result = step.Is("NoTransaction") ? result.Replace("_NOTRAN_", "WithoutTransaction") : result.Replace("_NOTRAN_", "");

        result = result.Replace("UNITOFWORKTYPE", "Read"); // Unitofwork type are always Command

        result = DomainTypeHelper.ReplaceDomainType(result, resultType.Get("DomainType"));

        return result;
    }
}

public class ModelListMethodTemplateParser : ITemplateParser
{
    public string LoopIdentifier => "foreach STEP_IN_USECASE_WITH_MODEL_LIST_RETURN";
    public List<string> AdditionalLoopIdentifiers => new List<string>();

    public List<TemplateOption> Options { get; } = new List<TemplateOption>();
}
