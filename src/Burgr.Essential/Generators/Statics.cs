namespace SolidOps.Burgr.Essential.Generators;

public static class Tags
{
    public const string Namespace = "MetaCorp.Template";
    public const string IdentityKeyType = "_IDENTITY_KEY_TYPE_";
    public const string PrimaryIdentityKeyType = "_PRIMARY_IDENTITY_KEY_TYPE_";
    public const string DependencyEventDataType = "DEPENDENCYEVENTDATATYPE";
}

public static class DescriptorTypes
{
    public const string PRODUCED_EVENT_DESCRIPTOR = "ProducedEvent";
    public const string RESOURCE_DEFINITION_DESCRIPTOR = "ResourceDefinition";
    public const string USECASE_STEP_PARAMETER_DESCRIPTOR = "UseCaseStepParameter";
    public const string COMPONENT_DEFINITION_DESCRIPTOR = "ComponentDefinition";
    public const string VIEW_DEFINITION_DESCRIPTOR = "ViewDefinition";
}

public enum NavigationType
{
    None = 0,
    Single = 1,
    List = 2,
    Both = 3
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
