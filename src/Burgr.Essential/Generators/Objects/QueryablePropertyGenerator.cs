using SolidOps.Burgr.Core;
using SolidOps.Burgr.Core.Descriptors;
using SolidOps.Burgr.Core.Generators;

namespace SolidOps.Burgr.Essential.Generators.Objects;

public class QueryablePropertyGenerator : BaseBurgrGenerator, IGenerator
{
    public static string Name = "QueryableProperty";
    public override string DescriptorType => Name;
    public override string AimedModelDescriptorType => "Property";

    public QueryablePropertyGenerator()
    {
        TemplateParser = new QueryablePropertyTemplateParser();
    }

    protected override string CheckIfApply(ModelDescriptor model, TemplateDescriptor template)
    {
        return !model.Is("Queryable") ? "property not queryable" : null;
    }

    public override string Generate(string content, ModelDescriptor model, TemplateDescriptor template, string modelPrefix, string modelSuffix)
    {
        var conversionService = ConversionServices[template.DestinationLanguage];
        string result = base.Generate(content, model, template, modelPrefix, modelSuffix);
        if (result == string.Empty)
            return result;
        string type;
        string fieldName = model.Get("FieldName");
        string propertyName = model.Get("PropertyName");
        string refOperation = "CriteriaOperation.Equal";
        string operation = refOperation;
        if (model.Get("PropertyType") == "Simple")
        {
            type = model.Get("SimpleType");
            Type simpleType = conversionService.GetSimpleType(type);
            if (simpleType.IsValueType && model.Is("Null"))
            {
                type = "Nullable" + type;
            }

            if (type == "Byte[]")
            {
                type = "ArrayOfByte";
            }

            if (type == "string" && !model.Is("UniqueCaseSensitive"))
            {
                operation = "CriteriaOperation.Like";
            }
        }
        else
        {
            type = model.Get("IdentityKeysType") ?? GeneratorOptions.IdentityKeysType;
            if (model.Is("Null"))
            {
                type += "?";
            }

            fieldName += "Id";
            propertyName += "Id";
        }

        if (string.IsNullOrEmpty(model.Get("SpecialType")))
        {
            result = result.Replace("_PROPERTYNAME_", propertyName);
            result = result.Replace("_SIMPLE__FIELDNAME_", fieldName);
        }
        else if (model.Get("SpecialType") == SpecialType.Calculated.ToString())
        {
            result = result.Replace("_CALCULATED__PROPERTYNAME_", propertyName);
            result = result.Replace("_CALCULATED__SIMPLE__FIELDNAME_", fieldName);
        }
        else if (model.Get("SpecialType") == SpecialType.NonPersisted.ToString())
        {
            result = result.Replace("_NONPERSISTED__PROPERTYNAME_", propertyName);
            result = result.Replace("_NONPERSISTED__SIMPLE__FIELDNAME_", fieldName);
        }
        else if (model.Get("SpecialType") == SpecialType.Navigation.ToString())
        {
            result = result.Replace("_NAVIGATION__PROPERTYNAME_", propertyName);
            result = result.Replace("_NAVIGATION__SIMPLE__FIELDNAME_", fieldName);
        }
        result = result.Replace("_SIMPLE__TYPE_", type);
        if (refOperation != operation)
        {
            result = result.Replace(refOperation, operation);
        }

        result = result.Replace("_SIMPLE_", "");

        return result;
    }
}

public class QueryablePropertyTemplateParser : ITemplateParser
{
    public string LoopIdentifier => "foreach QUERYABLE_PROPERTY";
    public List<string> AdditionalLoopIdentifiers => new List<string>();

    public List<TemplateOption> Options { get; } = new List<TemplateOption>();
}