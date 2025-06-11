using SolidOps.Burgr.Core.Descriptors;
using System.Globalization;
using System.Reflection;

namespace SolidOps.Burgr.Essential.Generators.ConversionServices;

public abstract class BaseConversionService : IConversionService
{
    public string Language { get; private set; }

    protected BaseConversionService(string language)
    {
        Language = language;
    }

    public static readonly Dictionary<Type, string> Aliases =
        new Dictionary<Type, string>()
    {
        { typeof(byte), "byte" },
        { typeof(sbyte), "sbyte" },
        { typeof(short), "short" },
        { typeof(ushort), "ushort" },
        { typeof(int), "int" },
        { typeof(uint), "uint" },
        { typeof(long), "long" },
        { typeof(ulong), "ulong" },
        { typeof(float), "float" },
        { typeof(double), "double" },
        { typeof(decimal), "decimal" },
        { typeof(object), "object" },
        { typeof(bool), "bool" },
        { typeof(char), "char" },
        { typeof(string), "string" },
        { typeof(void), "void" },
        { typeof(DateTime), "datetime" },
        { typeof(Guid), "guid" }
    };

    public static readonly Dictionary<string, Type> ReverseAliases =
        new Dictionary<string, Type>()
    {
        { "byte", typeof(byte) },
        { "sbyte", typeof(sbyte) },
        { "short", typeof(short) },
        { "ushort" , typeof(ushort) },
        { "int" , typeof(int) },
        { "uint" , typeof(uint) },
        { "long" , typeof(long) },
        { "ulong" , typeof(ulong) },
        { "float" , typeof(float) },
        { "double" , typeof(double) },
        { "decimal" , typeof(decimal) },
        { "object" , typeof(object) },
        { "bool" , typeof(bool) },
        { "char" , typeof(char) },
        { "string" , typeof(string) },
        { "void" , typeof(void) },
        { "datetime" , typeof(DateTime) },
        { "guid" , typeof(Guid) }
    };

    public string ConvertParameterType(ModelDescriptor parameter, string modelPrefix, string modelSuffix, bool convertList = false, bool isInterface = false, bool fullName = false)
    {
        if (parameter.Get("SimpleType") != null)
        {
            Type simpleType = GetSimpleType(parameter.Get("SimpleType"));
            string simpleTypeName = simpleType.Name;
            string simpleFullName = simpleType.FullName;
            return ConvertParameterType(simpleTypeName, simpleFullName, simpleType.IsEnum, parameter.NamespaceName, parameter.ModuleName, modelPrefix, modelSuffix, convertList, isInterface, fullName);
        }

        var related = parameter.GetRelated("Object");
        return ConvertRelatedParameterType(parameter, related, modelPrefix, modelSuffix, convertList, isInterface, fullName);
    }
    
    public string ConvertRelatedParameterType(ModelDescriptor parameter, ModelDescriptor related, string modelPrefix, string modelSuffix, bool convertList = false, bool isInterface = false, bool fullName = false)
    {
        string relatedName = related.Name;
        string relatedFullname = related.FullModuleName + "." + related.Name;
        string relatedDomainType = related.Get("DomainType");

        if (modelPrefix != null)
            modelPrefix = DomainTypeHelper.ReplaceDomainType(modelPrefix, related.Get("DomainType"));

        if (parameter.Is("List"))
        {
            relatedName += "[]";
            relatedFullname += "[]";
        }

        return ConvertParameterType(relatedName, relatedFullname, parameter.Is("Enum"), related.NamespaceName, related.ModuleName, modelPrefix, modelSuffix, convertList, isInterface, fullName);
    }

    public virtual string ConvertParameterType(string typeName, string fullTypeName, bool isEnum, string namespaceName, string moduleName, string modelPrefix, string modelSuffix, bool convertList = false, bool isInterface = false, bool fullName = false)
    {
        if (fullTypeName.StartsWith("System."))
        {
            if (fullTypeName.StartsWith("System.Nullable"))
            {
                return fullTypeName + "?";
            }
            return fullTypeName;
        }
        if (isEnum)
        {
            return ConvertParameterType(typeName, namespaceName, moduleName, ".Contracts.Enums.", "Enum", convertList, false, true);
        }
        return ConvertParameterType(typeName, namespaceName, moduleName, modelPrefix, modelSuffix, convertList, isInterface, fullName);

    }

    public virtual string ConvertParameterType(string typeName, string namespaceName, string moduleName, string modelPrefix, string modelSuffix, bool convertList, bool isInterface, bool fullName)
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
            return "IEnumerable<" + prefix + ConversionHelper.ConvertToPascalCase(typeName.Replace("[]", "")) + suffix + ">";
        }
        if (typeName.EndsWith("DTO"))
        {
            return prefix + ConversionHelper.ConvertToPascalCase(typeName.Replace("DTO", "")) + suffix;
        }
        return prefix + ConversionHelper.ConvertToPascalCase(typeName) + suffix;
    }

    public virtual string ConvertPropertyName(string propertyName)
    {
        return propertyName;
    }

    public string ConvertToLabel(string text)
    {
        return string.Join(" ", text.Split('_').Select(t => t.Substring(0, 1).ToUpper() + t.ToLower().Substring(1)));
    }

    public string ConvertToEnumValue(string text)
    {
        return text == null ? null : text.Length > 0 ? text.ToUpper(CultureInfo.InvariantCulture) : string.Empty;
    }

    public string ConvertToField(string text)
    {
        if (text == null)
        {
            return null;
        }

        if (text.Length > 0)
        {
            string[] arr = text.Split(new char[] { '_' });

            string returnedString = string.Empty;
            for (int i = 0; i < arr.Length; i++)
            {
                returnedString += arr[i][..1].ToUpper(CultureInfo.InvariantCulture) + arr[i][1..];
            }
            returnedString = "_" + returnedString[..1].ToLower(CultureInfo.InvariantCulture) + returnedString[1..];
            return returnedString;
        }
        else
        {
            return string.Empty;
        }
    }

    public string ConvertToPropertyType(string text)
    {
        if (text == null)
        {
            return null;
        }

        if (text.Length > 0)
        {
            string[] arr = text.Split(new char[] { '_' });

            string returnedString = string.Empty;
            for (int i = 0; i < arr.Length; i++)
            {
                returnedString += arr[i][..1].ToUpper(CultureInfo.InvariantCulture) + arr[i][1..];
            }

            if (returnedString.EndsWith("[]", StringComparison.Ordinal))
            {
                returnedString = !returnedString.StartsWith("System.Byte", StringComparison.Ordinal) && !returnedString.StartsWith("Byte", StringComparison.Ordinal)
                    ? string.Format(CultureInfo.InvariantCulture, "IEnumerable<{0}>", returnedString.Replace("[]", string.Empty))
                    : "IEnumerable<System.Byte>";
            }
            else if (returnedString == "Stream")
            {
                returnedString = "System.IO.Stream";
            }
            else if (returnedString.EndsWith("DTO"))
            {
                //returnedString = returnedString;
                //returnedString = returnedString.Replace("DTO", string.Empty);
            }

            return returnedString;
        }
        else
        {
            return string.Empty;
        }
    }

    public string ConvertToShortPropertyType(string text)
    {
        if (text == null)
        {
            return null;
        }

        string[] arr = text.Split('.');
        return arr == null || arr.Length == 0 ? ConvertToPropertyType(text) : ConvertToPropertyType(arr[^1]);
    }

    public string ConvertToListPropertyType(string text)
    {
        if (text == null)
        {
            return null;
        }

        if (text.Length > 0)
        {
            string[] arr = text.Split(new char[] { '_' });

            string returnedString = string.Empty;
            for (int i = 0; i < arr.Length; i++)
            {
                returnedString += arr[i][..1].ToUpper(CultureInfo.InvariantCulture) + arr[i][1..];
            }

            if (returnedString.EndsWith("[]", StringComparison.Ordinal))
            {
                returnedString = returnedString.Replace("[]", string.Empty);
            }

            return returnedString;
        }
        else
        {
            return string.Empty;
        }
    }
    public string ConvertToGetMethod(string text)
    {
        if (text == null)
        {
            return null;
        }

        if (text.Length > 0)
        {
            string[] arr = text.Split(new char[] { '_' });

            string returnedString = "To";
            for (int i = 0; i < arr.Length; i++)
            {
                returnedString += arr[i][..1].ToUpper(CultureInfo.InvariantCulture) + arr[i][1..];
            }

            if (returnedString.EndsWith("[]", StringComparison.Ordinal))
            {
                returnedString = string.Format(CultureInfo.InvariantCulture, "{0}List", returnedString.Replace("[]", string.Empty));
            }

            return returnedString;
        }
        else
        {
            return string.Empty;
        }
    }

    public string ConvertToTableName(string tablePrefix, string typeName, string tableName)
    {
        tableName ??= typeName + "s";
        return tableName;
    }

    public string ConvertToFullTableName(string moduleName, string typeName, string forcedPrefix, string tableName)
    {
        tableName ??= typeName + "s";

        string shortenModuleName;

        if (forcedPrefix != null)
        {
            shortenModuleName = forcedPrefix;
        }
        else
        {
            if (moduleName == null)
            {
                throw new ArgumentNullException("moduleName");
            }

            shortenModuleName = moduleName.Length >= 3
                ? moduleName.ToLower(CultureInfo.InvariantCulture)[..3]
                : moduleName.ToLower(CultureInfo.InvariantCulture);
        }

        string res = string.Format(CultureInfo.InvariantCulture, "[{0}].", shortenModuleName);
        res += "[" + tableName + "]";

        return res;
    }

    public string ConvertToFullMySQLTableName(string moduleName, string typeName, string forcedPrefix, string tableName)
    {
        tableName ??= typeName + "s";

        string shortenModuleName;

        if (forcedPrefix != null)
        {
            shortenModuleName = forcedPrefix;
        }
        else
        {
            if (moduleName == null)
            {
                throw new ArgumentNullException("moduleName");
            }

            string lastModuleName = moduleName.Split('.').Last();

            shortenModuleName = lastModuleName.Length >= 3
                ? lastModuleName.ToLower(CultureInfo.InvariantCulture)[..3]
                : lastModuleName.ToLower(CultureInfo.InvariantCulture);
        }

        string res = string.Format(CultureInfo.InvariantCulture, "{0}_", shortenModuleName);
        res += tableName;

        return res;
    }

    public ReturnType GetReturnType(MethodInfo methodInfo)
    {
        return methodInfo.ReturnType.FullName == "System.Void"
            ? ReturnType.Void
            : methodInfo.ReturnType.FullName.StartsWith("System")
                ? ReturnType.Simple
                : methodInfo.ReturnType.FullName.EndsWith("[]") ? ReturnType.ModelList : ReturnType.Model;
    }

    public Type GetSimpleType(string typeName)
    {
        if (!ReverseAliases.ContainsKey(typeName))
        {
            throw new Exception($"{typeName} is not a known simple type. Maybe it is an domain object and misses object identifier");
        }
        return ReverseAliases[typeName];
    }

    public virtual string SimplePropertyType(ModelDescriptor model, bool preventList)
    {
        Type simpleType = GetSimpleType(model.Get("SimpleType"));

        if (!model.Is("List"))
        {
            string typeName = simpleType.FullName;
            Type nullable = Nullable.GetUnderlyingType(simpleType);
            if (nullable != null)
            {
                return ConvertToPropertyType(nullable.Name) + "?";
            }
            else
            {
                var tmp = ConvertToPropertyType(typeName);
                if (simpleType.IsValueType)
                {
                    if (model.Is("Null"))
                    {
                        return tmp.Replace("System.", "") + "?";
                    }
                }
                return tmp;
            }
        }
        else
        {
            string typeName = simpleType.FullName.Replace("[]", "");
            return "List<" + ConvertToPropertyType(typeName) + ">";
        }
    }

    public virtual string EnumPropertyType(ModelDescriptor model, string suffix, bool preventList)
    {
        string csClassName = ConversionHelper.ConvertToPascalCase(model.Get("EnumType"));
        if (!model.Is("List") || preventList)
        {
            var res = csClassName + "Enum" + suffix;
            if (model.Is("Null"))
            {
                return res + "?";
            }
            return res;
        }
        return "List<" + csClassName + "Enum" + suffix + ">";
    }

    public virtual string ModelPropertyType(ModelDescriptor model, string prefix, string suffix, bool preventList)
    {
        var typeDescriptor = model.GetRelated("Object");

        return !model.Is("List") || preventList
                    ? prefix + ConversionHelper.ConvertToPascalCase(typeDescriptor.Name) + suffix
                    : "List<" + prefix + ConversionHelper.ConvertToPascalCase(typeDescriptor.Name) + suffix + ">";
    }

    public virtual string ReferencedModelPropertyType(ModelDescriptor model, string prefix, string suffix, bool preventList)
    {
        return !model.Is("List") || preventList
                                    ? prefix + ConversionHelper.ConvertToPascalCase(model.Name.Replace("[]", "")) + suffix
                                    : "List<" + prefix + ConversionHelper.ConvertToPascalCase(model.Name.Replace("[]", "")) + suffix + ">";
    }

    public virtual string ReplaceIdentityType(string text, string type)
    {
        return text.Replace(Tags.IdentityKeyType, type);
    }

    public virtual string ConvertModuleName(string moduleName)
    {
        return moduleName;
    }

    public virtual string ConvertDomainType(ModelDescriptor model)
    {
        return DomainTypeHelper.GetDomainType(model.Get("DomainType"));
    }

    public virtual string ConvertOption(ModelDescriptor model)
    {
        return string.Empty;
    }
}

