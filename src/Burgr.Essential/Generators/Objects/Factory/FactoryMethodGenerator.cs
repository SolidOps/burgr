using SolidOps.Burgr.Core;
using SolidOps.Burgr.Core.Descriptors;
using SolidOps.Burgr.Core.Generators;
using SolidOps.SubZero;

namespace SolidOps.Burgr.Essential.Generators.Objects.Factory;

public class FactoryMethodGenerator : BaseFactoryMethodGenerator, IGenerator
{
    public static string Name = "FactoryMethod";
    public override string DescriptorType => Name;

    public FactoryMethodGenerator()
    {
        TemplateParser = new ModelMethodTemplateParser();
    }
    protected override string CheckIfApply(ModelDescriptor model, TemplateDescriptor template)
    {
        var result = base.CheckIfApply(model, template);
        if (result != null)
            return result;

        return model.Get("ReturnType") != ReturnType.Model.ToString() ? "factory method is not model" : null;
    }

    public override string Generate(string content, ModelDescriptor model, TemplateDescriptor template, string modelPrefix, string modelSuffix)
    {
        var conversionService = ConversionServices[template.DestinationLanguage];
        string result = base.Generate(content, model, template, modelPrefix, modelSuffix);
        if (result == string.Empty)
            return result;
        ModelDescriptor method = model;
        ModelDescriptor factory = model.Parent;

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

        result = ReplaceParameters(factory, conversionService, method, result, modelPrefix, modelSuffix, out _);

        result = DomainTypeHelper.ReplaceDomainType(result, resultType.Get("DomainType"));

        result = result.Replace("FACTORYNAME", ConversionHelper.ConvertToPascalCase(method.Name));

        return result;
    }
}

public class ModelMethodTemplateParser : ITemplateParser
{
    public string LoopIdentifier => "foreach FACTORY_METHOD";
    public List<string> AdditionalLoopIdentifiers => new List<string>();

    public List<TemplateOption> Options { get; } = new List<TemplateOption>();
}
