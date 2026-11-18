namespace SolidOps.Burgr.Core
{
    // EnumType of burgr-templates.json: how burgr names an enum type it writes itself, in a
    // method parameter or result for instance. NORAD's convention by default.
    public class TemplateEnumType
    {
        public const string DefaultPrefix = ".Contracts.Enums.";
        public const string DefaultSuffix = "Enum";

        // after the module namespace, dots included
        public string Prefix { get; set; } = DefaultPrefix;
        public string Suffix { get; set; } = DefaultSuffix;
    }
}
