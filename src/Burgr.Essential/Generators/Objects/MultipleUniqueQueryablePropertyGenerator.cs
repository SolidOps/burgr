using SolidOps.Burgr.Core;
using SolidOps.Burgr.Core.Descriptors;
using SolidOps.Burgr.Core.Generators;
using SolidOps.Burgr.Essential.Generators.ConversionServices;

namespace SolidOps.Burgr.Essential.Generators.Objects;

public class MultipleUniqueQueryablePropertyGenerator : BaseBurgrGenerator, IGenerator
{
    public static string Name = "MultipleUniqueQueryableProperty";
    public override string DescriptorType => Name;
    public override string AimedModelDescriptorType => "Property";

    public MultipleUniqueQueryablePropertyGenerator()
    {
        TemplateParser = new MultipleUniqueQueryablePropertyTemplateParser();
    }

    protected override string CheckIfApply(ModelDescriptor model, TemplateDescriptor template)
    {
        return !model.Is("MultipleUniqueConstraint") ? "property not multiple unique constraint" : null;
    }

    public override string Generate(string content, ModelDescriptor model, TemplateDescriptor template, string modelPrefix, string modelSuffix)
    {
        var conversionService = ConversionServices[template.DestinationLanguage];
        string result = base.Generate(content, model, template, modelPrefix, modelSuffix);
        if (result == string.Empty)
            return result;
        string constraintName = model.Get("PropertyName");
        string operation = "CriteriaOperation.Equal";
        string expressionOperator = "==";
        if (model.Get("SimpleType") == "string")
        {
            operation = "CriteriaOperation.Like";
        }
        string paramType = GetParamType(model, conversionService);

        string constraintTuples = "(\"" + model.Get("PropertyName") + "\", " + operation + ", " + model.Get("FieldName") + ")";
        string constraintValues = "entity." + model.Get("QueryPropertyName");
        string constraintParams = "object " + model.Get("FieldName");
        string constraintParamsTyped = $"{paramType} {model.Get("FieldName")}";
        string constraintExpression = $"x => x.{model.Get("QueryPropertyName")} {expressionOperator} {model.Get("FieldName")}";
        foreach (string additionalPropertyName in model.GetList("UniqueConstraintAdditionalMembers"))
        {
            ModelDescriptor additionalPropertyDescriptor = model.Parent.GetChildren(PropertyGenerator.Name).Where(x => x.Name == additionalPropertyName).SingleOrDefault();
            if (additionalPropertyDescriptor != null)
            {
                operation = "CriteriaOperation.Equal";
                expressionOperator = "==";
                constraintName += "And" + additionalPropertyDescriptor.Get("PropertyName");
                if (additionalPropertyDescriptor.Get("SimpleType") == "string")
                {
                    operation = "CriteriaOperation.Like";
                }
                paramType = GetParamType(additionalPropertyDescriptor, conversionService);

                constraintTuples += ", (\"" + additionalPropertyDescriptor.Get("PropertyName") + "\", " + operation + ", " + additionalPropertyDescriptor.Get("FieldName") + ")";
                constraintValues += ", entity." + additionalPropertyDescriptor.Get("QueryPropertyName");
                constraintParams += ", object " + additionalPropertyDescriptor.Get("FieldName");
                constraintParamsTyped += $", {paramType} {additionalPropertyDescriptor.Get("FieldName")}";
                constraintExpression += $" && x.{additionalPropertyDescriptor.Get("QueryPropertyName")} {expressionOperator} {additionalPropertyDescriptor.Get("FieldName")}";
            }
        }

        result = result.Replace("_MULTIPLE_CONSTRAINT_", constraintName);
        result = result.Replace("_CONSTRAINT_TUPLES_", constraintTuples);
        result = result.Replace("_CONSTRAINT_VALUES_", constraintValues);
        result = result.Replace("_CONSTRAINT_PARAMS_TYPED_", constraintParamsTyped);
        result = result.Replace("_CONSTRAINT_PARAMS_", constraintParams);
        result = result.Replace("_CONSTRAINT_EXPRESSION_", constraintExpression);
        return result;
    }

    private string GetParamType(ModelDescriptor model, IConversionService conversionService)
    {
        string paramType;
        if (model.Get("PropertyType") == "Simple")
        {
            paramType = model.Get("SimpleType");
            Type simpleType = conversionService.GetSimpleType(paramType);
            if (simpleType.IsValueType && model.Is("Null"))
            {
                paramType += "?";
            }
        }
        else if (model.Get("PropertyType") == "Enum")
        {
            paramType = "Contracts.Enums." + model.GetPropertyType(conversionService, null, string.Empty, true);
        }
        else
        {
            paramType = model.Get("IdentityKeysType") ?? GeneratorOptions.IdentityKeysType;
            if (model.Is("Null"))
            {
                paramType += "?";
            }
        }
        return paramType;
    }
}

public class MultipleUniqueQueryablePropertyTemplateParser : ITemplateParser
{
    public string LoopIdentifier => "foreach MULTIPLE_UNIQUE_QUERYABLE_PROPERTY";
    public List<string> AdditionalLoopIdentifiers => new List<string>();

    public List<TemplateOption> Options { get; } = new List<TemplateOption>();
}