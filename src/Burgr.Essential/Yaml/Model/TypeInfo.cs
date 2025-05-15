using SolidOps.Burgr.Essential.Generators.ConversionServices;
using SolidOps.Burgr.Essential.Generators.Objects;

namespace SolidOps.Burgr.Essential.Yaml.Model;

public class TypeInfo
{
    private static string EnumIdentifier = "$";
    private static string ArrayIdentifier = "[]";
    private static string NullIdentifier = "?";
    private static string RefSeparator = ".";
    private static string SplitSeparator = "|";
    private static string Identity = "identity";
    private static string Void = "void";
    private static string Text = "text";
    private static string NavigationIdentifier = "->";
    private static string CalculatedIdentifier = "=";
    private static string NonPersistedIdentifier = "-";

    public TypeType TypeType { get; private set; }
    public string TypeName { get; private set; }
    public string Name { get; private set; }
    public string ModuleName { get; private set; }
    public string FullName { get; private set; }
    public bool IsEnum { get; private set; }
    public bool IsArray { get; private set; }
    public bool IsNull { get; private set; }
    public bool IsNavigation { get; private set; }
    public bool IsCalculated { get; private set; }
    public bool IsNonPersisted { get; private set; }

    public TypeInfo(string typeName, string moduleName)
    {
        TypeName = typeName;
        Name = typeName;

        if (Name.EndsWith(ArrayIdentifier))
        {
            if (Name.IndexOf(TypeInfo.ArrayIdentifier, StringComparison.Ordinal) != Name.LastIndexOf(TypeInfo.ArrayIdentifier, StringComparison.Ordinal))
            {
                throw new Exception(string.Format("Could not treat multiple array type : {0}", Name));
            }
            IsArray = true;
            Name = Name.Replace(ArrayIdentifier, string.Empty);
        }

        if (Name.EndsWith(NullIdentifier))
        {
            IsNull = true;
            Name = Name.Replace(NullIdentifier, string.Empty);
        }

        if (Name.StartsWith(NavigationIdentifier))
        {
            IsNavigation = true;
            Name = Name.Replace(NavigationIdentifier, string.Empty);
        }
        else if (Name.StartsWith(CalculatedIdentifier))
        {
            IsCalculated = true;
            Name = Name.Replace(CalculatedIdentifier, string.Empty);
        }
        else if (Name.StartsWith(NonPersistedIdentifier))
        {
            IsNonPersisted = true;
            Name = Name.Replace(NonPersistedIdentifier, string.Empty);
        }

        if (Name.StartsWith(EnumIdentifier))
        {
            TypeType = TypeType.Enum;
            Name = Name.Substring(1);
            IsEnum = true;
        }
        else
        {
            if (IsSimpleType(Name)
                || Name == "text"
                || Name == Identity
                || Name == Void)
            {
                TypeType = TypeType.Simple;

                if (IsNavigation)
                    throw new Exception("Only models can be navigated to");
            }
            else
            {
                if (Name.Contains(RefSeparator))
                {
                    TypeType = TypeType.ReferencedModel;
                    var parts = Name.Split(RefSeparator);
                    ModuleName = string.Join(".", parts.SkipLast(1));
                    Name = parts.Last();
                }
                else
                {
                    TypeType = TypeType.Model;
                    ModuleName = moduleName;
                }
            }
        }

        FullName = ModuleName + "." + Name;
    }

    public static bool IsSimpleType(string typeName)
    {
        if (BaseConversionService.ReverseAliases.Keys.Contains(typeName))
            return true;
        return false;
    }
}

public enum TypeType
{
    Simple,
    Enum,
    Model,
    ReferencedModel
}
