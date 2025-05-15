using SolidOps.Burgr.Core;
using SolidOps.Burgr.Core.Descriptors;
using SolidOps.Burgr.Core.Generators;
using SolidOps.SubZero;

namespace SolidOps.Burgr.Essential.Generators.Services;

public class ModelMethodGenerator : BaseMethodGenerator, IGenerator
{
    public static string Name = "ModelMethod";
    public override string DescriptorType => Name;

    public ModelMethodGenerator()
    {
        TemplateParser = new ModelMethodTemplateParser();
    }
    protected override string CheckIfApply(ModelDescriptor model, TemplateDescriptor template)
    {
        return model.Get("ReturnType") != ReturnType.Model.ToString() ? "service method is not model" : null;
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
        result = result.Replace("_DOMODELACTION_", ConversionHelper.ConvertToPascalCase(method.Name));
        result = result.Replace("_DOMODELACTIONURL_", TextHelper.GenerateSlug(method.Name));
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

public class ModelMethodTemplateParser : ITemplateParser
{
    public string LoopIdentifier => "foreach METHOD_IN_SERVICE_WITH_MODEL_RETURN";
    public List<string> AdditionalLoopIdentifiers => new List<string>();

    public List<TemplateOption> Options { get; } = new List<TemplateOption>();
}
