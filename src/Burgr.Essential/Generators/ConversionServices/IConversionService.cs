using SolidOps.Burgr.Core.Descriptors;
using System.Reflection;

namespace SolidOps.Burgr.Essential.Generators.ConversionServices;

public interface IConversionService
{
    string Language { get; }
    string ConvertToLabel(string name);
    string ConvertParameterType(ModelDescriptor parameter, string modelPrefix, string modelSuffix, bool convertList = false, bool isInterface = false, bool fullName = false);
    string ConvertRelatedParameterType(ModelDescriptor parameter, ModelDescriptor related, string modelPrefix, string modelSuffix, bool convertList = false, bool isInterface = false, bool fullName = false);
    string ConvertParameterType(string typeName, string fullTypeName, bool isEnum, string namespaceName, string moduleName, string modelPrefix, string modelSuffix, bool convertList = false, bool isInterface = false, bool fullName = false);
    string ConvertParameterType(string typeName, string namespaceName, string moduleName, string modelPrefix, string modelSuffix, bool convertList, bool isInterface, bool fullName);
    string ConvertPropertyName(string propertyName);

    string ConvertToEnumValue(string text);

    string ConvertToField(string text);
    string ConvertToPropertyType(string text);

    string ConvertToShortPropertyType(string text);

    string ConvertToListPropertyType(string text);

    string ConvertToGetMethod(string text);

    string ConvertToTableName(string tablePrefix, string typeName, string tableName);

    string ConvertToFullTableName(string moduleName, string typeName, string forcedPrefix, string tableName);
    string ConvertToFullMySQLTableName(string moduleName, string typeName, string forcedPrefix, string tableName);

    ReturnType GetReturnType(MethodInfo methodInfo);

    Type GetSimpleType(string typeName);

    string SimplePropertyType(ModelDescriptor model, bool preventList);

    string EnumPropertyType(ModelDescriptor model, string suffix, bool preventList);
    string ModelPropertyType(ModelDescriptor model,string prefix, string suffix, bool preventList);

    string ReferencedModelPropertyType(ModelDescriptor model, string prefix, string suffix, bool preventList);
    string ReplaceIdentityType(string text, string type);

    string ConvertModuleName(string moduleName);
    string ConvertDomainType(ModelDescriptor model);
    string ConvertOption(ModelDescriptor model);
}

