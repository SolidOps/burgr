using SolidOps.Burgr.Core;
using SolidOps.Burgr.Core.Descriptors;
using SolidOps.Burgr.Core.Generators;

namespace SolidOps.Burgr.Essential.Generators.Objects;

public class UniqueQueryablePropertyGenerator : BaseBurgrGenerator, IGenerator
{
    public static string Name = "UniqueQueryableProperty";
    public override string DescriptorType => Name;
    public override string AimedModelDescriptorType => "Property";

    public UniqueQueryablePropertyGenerator()
    {
        TemplateParser = new UniqueQueryablePropertyTemplateParser();
    }

    protected override string CheckIfApply(ModelDescriptor model, TemplateDescriptor template)
    {
        return !model.Is("Unique") ? "property not unique" : null;
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
            type = simpleType.Name;
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

        fieldName = fieldName.Replace("_", string.Empty);

        result = result.Replace("_PROPERTYNAME_", propertyName);
        result = result.Replace("_SIMPLE__FIELDNAME_", fieldName);
        result = result.Replace("_SIMPLE__TYPE_", type);
        result = result.Replace("_CALCULATED_", string.Empty);
        result = result.Replace("_NONPERSISTED_", string.Empty);
        result = result.Replace("_NAVIGATION_", string.Empty);
        if (refOperation != operation)
        {
            result = result.Replace(refOperation, operation);
        }

        result = result.Replace("_SIMPLE_", "");

        return result;
    }
}

public class UniqueQueryablePropertyTemplateParser : ITemplateParser
{
    public string LoopIdentifier => "foreach UNIQUE_QUERYABLE_PROPERTY";
    public List<string> AdditionalLoopIdentifiers => new List<string>();

    public List<TemplateOption> Options { get; } = new List<TemplateOption>();
}