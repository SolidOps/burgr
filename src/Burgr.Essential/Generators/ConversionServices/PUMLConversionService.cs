using SolidOps.Burgr.Core.Descriptors;

namespace SolidOps.Burgr.Essential.Generators.ConversionServices;

public class PUMLConversionService : BaseConversionService
{
    public PUMLConversionService() : base("PUML")
    {
    }

    public override string ConvertParameterType(string typeName, string fullTypeName, bool isEnum, string namespaceName, string moduleName, string modelPrefix, string modelSuffix, bool convertList = false, bool isInterface = false, bool fullName = false)
    {
        if (fullTypeName.StartsWith("System."))
        {
            Type simpleType = Type.GetType(fullTypeName);
            return ConvertToPUMLPropertyType(simpleType, null);
        }
        if (isEnum)
        {
            return "string";
        }
        if (fullTypeName.Contains(".Common."))
        {
            return "SolidOps.Common." + ConvertParameterType(typeName, namespaceName, moduleName, modelPrefix, modelSuffix, convertList, false, fullName);
        }
        return ConvertParameterType(typeName, namespaceName, moduleName, modelPrefix, modelSuffix, convertList, isInterface, fullName);
    }

    public override string ConvertPropertyName(string propertyName)
    {
        return ConversionHelper.ConvertToCamelCase(propertyName);
    }

    public override string ConvertParameterType(string typeName, string namespaceName, string moduleName, string modelPrefix, string modelSuffix, bool convertList, bool isInterface, bool fullName)
    {
        return ConversionHelper.ConvertToPascalCase(typeName.Replace("[]", ""));
    }

    public override string SimplePropertyType(ModelDescriptor model, bool preventList)
    {
        Type simpleType = GetSimpleType(model.Get("SimpleType"));
        return !model.Is("List") ? ConvertToPUMLPropertyType(simpleType, model) : ConvertToPUMLPropertyType(simpleType, model) + "[]";
    }

    public override string EnumPropertyType(ModelDescriptor model, string suffix, bool preventList)
    {
        string jsClassName = ConversionHelper.ConvertToPascalCase(model.Get("EnumType"));
        if (!model.Is("List") || preventList)
        {
            var res = jsClassName + "Enum" + suffix;
            if (model.Is("Null"))
            {
                return res + "?";
            }
            return res;
        }
        return jsClassName + "Enum" + suffix + "[]";
    }

    public override string ModelPropertyType(ModelDescriptor model, string prefix, string suffix, bool preventList)
    {
        var typeDescriptor = model.GetRelated("Object");

        return ConversionHelper.ConvertToPascalCase(typeDescriptor.Name.Replace("[]", ""));
    }

    public override string ReferencedModelPropertyType(ModelDescriptor model, string prefix, string suffix, bool preventList)
    {
        return ConversionHelper.ConvertToPascalCase(model.Name.Replace("[]", ""));
    }

    public static string ConvertToPUMLPropertyType(string type)
    {
        return type is "Guid" or "string" ? "string" : "number";
    }

    public static string ConvertToPUMLPropertyType(Type type, ModelDescriptor model)
    {
        if (type == null)
        {
            return null;
        }

        if (type.IsArray)
        {
            return ConvertToPUMLPropertyType(type.GetElementType(), model) + "[]";
        }

        Type nullable = Nullable.GetUnderlyingType(type);
        if (nullable != null)
        {
            return ConvertToPUMLPropertyType(nullable, model);
        }

        if (type == typeof(decimal)
            || type == typeof(double)
            || type == typeof(int)
            || type == typeof(float)
            || type == typeof(ulong))
        {
            return "number";
        }

        if (type == typeof(string))
        {
            var fieldSize = model?.Get<int?>("FieldSize");
            if(fieldSize.HasValue)
            {
                return $"string[{fieldSize.Value}]";
            }
            return "string";
        }

        if (type == typeof(bool))
        {
            return "boolean";
        }

        if (type == typeof(DateTime))
        {
            return "string";
        }

        if (type == typeof(byte))
        {
            return "string";
        }

        if (type == typeof(Guid))
        {
            return "string";
        }

        if (type == typeof(object))
        {
            return "any";
        }

        if (type.FullName.StartsWith("SolidOps.Burgr.Core"))
        {
            return " type.FullName";
        }

        throw new NotImplementedException("Could not convert type : " + type.FullName);
    }

    public override string ReplaceIdentityType(string text, string type)
    {
        return text.Replace(Tags.IdentityKeyType, ConvertToPUMLPropertyType(type));
    }

    public override string ConvertDomainType(ModelDescriptor model)
    {
        DomainType domainType;
        if (Enum.TryParse<DomainType>(model.Get("DomainType"), out domainType))
        {
            switch (domainType)
            {
                case DomainType.Aggregate:
                    return "<< (A,#FF473A) Aggregate Root>>";
                case DomainType.ValueObject:
                    return "<< (V,#FFFB28) Value Object >>";
                case DomainType.Transient:
                    return "<< (T,#3D6AFF) Transient >>";
                case DomainType.Entity:
                default:
                    return "<< (E,#90FF28) Entity >>";
            }
        }
        if(model.Is("Enum"))
        {
            return "<< (E,#7FC3FF) >>";
        }
        return "";
    }

    public override string ConvertOption(ModelDescriptor model)
    {
        SpecialType specialType;
        if (Enum.TryParse<SpecialType>(model.Get("SpecialType"), out specialType))
        {
            switch(specialType)
            {
                case SpecialType.Calculated:
                    return "[CA]";
                case SpecialType.NonPersisted:
                    return "[NP]";
                default:
                    return string.Empty;
            }
        }
        return string.Empty;
    }
}

