namespace SolidOps.Burgr.Essential.Generators;

public static class Tags
{
    public const string Namespace = "MetaCorp.Template";
    public const string ShortModule = "Template";
    public const string GenericProjectRoot = "MetaCorp";
    public const string ClassName = "CLASSNAME";
    public const string Property = "PROPERTY";
    public const string PropertyType = "PROPERTYTYPE";
    public const string PropertyName = "_PROPERTYNAME_";
    public const string FieldName = "FIELDNAME";
    public const string EnumPropertyName = "_ENUM__PROPERTYNAME_";
    public const string EnumProperty = "ENUMPROPERTY";
    public const string GenericProperty = "GENERICPROPERTY";
    public const string TableName = "TABLENAME";
    public const string ConvertMethod = "CONVERTMETHOD";
    public const string GetMethodName = "GETMETHODNAME";
    public const string SimplePropertyName = "_SIMPLE__PROPERTYNAME_";
    public const string SimpleFieldName = "SIMPLEFIELDNAME";
    public const string ListPropertyName = "LISTPROPERTYNAME";
    public const string ListFieldName = "LISTFIELDNAME";
    public const string IdColumnName = "IDCOLUMNNAME";
    public const string ControlName = "CONTROLNAME";
    public const string PositionFlag = " 99);//FLAG";

    public const string Slug = "Slug";
    public const string IdentityKeyType = "_IDENTITY_KEY_TYPE_";
    public const string PrimaryIdentityKeyType = "_PRIMARY_IDENTITY_KEY_TYPE_";
    public const string DependencyEventDataType = "DEPENDENCYEVENTDATATYPE";
    public const string ExtensionType = "EXTENSIONTYPE";
    public const string ExtensionNamespace = "EXTENSIONNAMESPACE";
}

public static class DescriptorTypes
{
    public const string PRODUCED_EVENT_DESCRIPTOR = "ProducedEvent";
    public const string RESOURCE_DEFINITION_DESCRIPTOR = "ResourceDefinition";
    public const string SERVICE_METHOD_PARAMETER_DESCRIPTOR = "ServiceMethodParameter";
    public const string COMPONENT_DEFINITION_DESCRIPTOR = "ComponentDefinition";
    public const string VIEW_DEFINITION_DESCRIPTOR = "ViewDefinition";
    public const string RULE_DEFINITION_DESCRIPTOR = "RuleDefinition";
}

public enum NavigationType
{
    None = 0,
    Single = 1,
    List = 2,
    Both = 3
}

public enum ArrayOption
{
    Both = 0,
    OnlyArray = 1,
    OnlyNonArray = 2
}

public static class Constants
{
    public const int StandardMaxSize = 255;
}

public enum SpecialType
{
    Calculated,
    NonPersisted,
    Navigation,
    DBAlias
}
