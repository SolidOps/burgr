using SolidOps.Burgr.Core.Descriptors;

namespace SolidOps.Burgr.Essential.Generators.ConversionServices;

public class JSConversionService : BaseConversionService
{
    public JSConversionService() : base("JS")
    {

    }
    public override string ConvertParameterType(string typeName, string fullTypeName, bool isEnum, string namespaceName, string moduleName, string modelPrefix, string modelSuffix, bool convertList = false, bool isInterface = false, bool fullName = false)
    {
        if (fullTypeName.StartsWith("System."))
        {
            Type simpleType = Type.GetType(fullTypeName);
            return ConvertToJSPropertyType(simpleType);
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
        string suffix = modelSuffix ?? string.Empty;
        string prefix = modelPrefix ?? string.Empty;

        if (fullName)
        {
            string root = namespaceName + "." + moduleName;
            prefix = root + prefix;
        }

        if (isInterface)
        {
            prefix += "I";
        }

        if (typeName.EndsWith("[]"))
        {
            if (!convertList)
            {
                return prefix + ConversionHelper.ConvertToPascalCase(typeName.Replace("[]", "")) + suffix;
            }
            return prefix + ConversionHelper.ConvertToPascalCase(typeName.Replace("[]", "")) + suffix + "[]";
        }
        if (typeName.EndsWith("DTO"))
        {
            return prefix + ConversionHelper.ConvertToPascalCase(typeName.Replace("DTO", "")) + suffix;
        }
        return prefix + ConversionHelper.ConvertToPascalCase(typeName) + suffix;
    }

    public override string SimplePropertyType(ModelDescriptor model, bool preventList)
    {
        Type simpleType = GetSimpleType(model.Get("SimpleType"));
        return !model.Is("List") ? ConvertToJSPropertyType(simpleType) : ConvertToJSPropertyType(simpleType) + "[]";
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

        return !model.Is("List") || preventList
                   ? prefix + ConversionHelper.ConvertToPascalCase(typeDescriptor.Name) + suffix
                   : prefix + ConversionHelper.ConvertToPascalCase(typeDescriptor.Name) + suffix + "[]";
    }

    public override string ReferencedModelPropertyType(ModelDescriptor model, string prefix, string suffix, bool preventList)
    {
        return !model.Is("List") || preventList
                                    ? prefix + ConversionHelper.ConvertToPascalCase(model.Name.Replace("[]", "")) + suffix
                                    : prefix + ConversionHelper.ConvertToPascalCase(model.Name.Replace("[]", "")) + suffix + "[]";
    }

    public static string ConvertToJSPropertyType(string type)
    {
        return type is "Guid" or "string" ? "string" : "number";
    }

    public static string ConvertToJSPropertyType(Type type)
    {
        if (type == null)
        {
            return null;
        }

        if (type.IsArray)
        {
            return ConvertToJSPropertyType(type.GetElementType()) + "[]";
        }

        Type nullable = Nullable.GetUnderlyingType(type);
        return nullable != null
            ? ConvertToJSPropertyType(nullable)
            : type == typeof(decimal)
            || type == typeof(double)
            || type == typeof(int)
            || type == typeof(float)
            || type == typeof(ulong)
            ? "number"
            : type == typeof(string)
                ? "string"
                : type == typeof(bool)
                                ? "boolean"
                                : type == typeof(DateTime)
                                                ? "string"
                                                : type == typeof(byte)
                                                                ? "string"
                                                                : type == typeof(Guid)
                                                                                ? "string"
                                                                                : type == typeof(object)
                                                                                                ? "any"
                                                                                                : type.FullName.StartsWith("SolidOps.Burgr.Core")
                                                                                                                ? type.FullName
                                                                                                                : throw new NotImplementedException("Could not convert type : " + type.FullName);
    }
    public static string ConvertToJSLibrary(string moduleName)
    {
        return moduleName.ToLower().Replace(".", "-");
    }

    public override string ReplaceIdentityType(string text, string type)
    {
        return text.Replace(Tags.IdentityKeyType, ConvertToJSPropertyType(type));
    }

    public override string ConvertModuleName(string moduleName)
    {
        return ConvertToJSLibrary(moduleName);
    }
}

