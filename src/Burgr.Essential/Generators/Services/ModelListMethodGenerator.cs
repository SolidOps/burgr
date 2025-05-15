using SolidOps.Burgr.Core;
using SolidOps.Burgr.Core.Descriptors;
using SolidOps.Burgr.Core.Generators;
using SolidOps.SubZero;

namespace SolidOps.Burgr.Essential.Generators.Services;

public class ModelListMethodGenerator : BaseMethodGenerator, IGenerator
{
    public static string Name = "ModelListMethod";
    public override string DescriptorType => Name;

    public ModelListMethodGenerator()
    {
        TemplateParser = new ModelListMethodTemplateParser();
    }

    protected override string CheckIfApply(ModelDescriptor method, TemplateDescriptor template)
    {
        return method.Get("ReturnType") != ReturnType.ModelList.ToString() ? "service method is not model list" : null;
    }

    public override string Generate(string content, ModelDescriptor model, TemplateDescriptor template, string modelPrefix, string modelSuffix)
    {
        var conversionService = ConversionServices[template.DestinationLanguage];
        string result = base.Generate(content, model, template, modelPrefix, modelSuffix);
        if (result == string.Empty)
            return result;
        ModelDescriptor method = model;
        ModelDescriptor service = model.Parent;

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

            result = result.Replace("DEPENDENCYNAMESPACE", Utilities.GetNamespace(method.FullModuleName));
        }
        result = result.Replace("_DOMODELLISTACTION_", ConversionHelper.ConvertToPascalCase(method.Name));
        result = result.Replace("_DOMODELLISTACTIONURL_", TextHelper.GenerateSlug(method.Name));
        // result = SetResourceCall(language, method, result);
        result = ReplaceParameters(service, conversionService, method, result, modelPrefix, modelSuffix, out _);

        // methods with return are always get unless post is forces
        result = method.Is("ForcePost") ? result.Replace("_VERB_", "Post") : result.Replace("_VERB_", "Get");

        result = method.Is("NoTransaction") ? result.Replace("_NOTRAN_", "WithoutTransaction") : result.Replace("_NOTRAN_", "");

        result = result.Replace("UNITOFWORKTYPE", "Read"); // Unitofwork type are always Command
        result = result.Replace("METHODRIGHT", method.Get("MethodMandatoryRight"));
        result = result.Replace("METHODOWNERSHIPOVERRIDERIGHT", method.Get("MethodOwnershipOverrideRight"));

        result = DomainTypeHelper.ReplaceDomainType(result, resultType.Get("DomainType"));

        return result;
    }
}

public class ModelListMethodTemplateParser : ITemplateParser
{
    public string LoopIdentifier => "foreach METHOD_IN_SERVICE_WITH_MODEL_LIST_RETURN";
    public List<string> AdditionalLoopIdentifiers => new List<string>();

    public List<TemplateOption> Options { get; } = new List<TemplateOption>();
}
