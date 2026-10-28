namespace SolidOps.Burgr.Core
{
    public static class GeneratorOptions
    {
        public class Tags
        {
            public const string StartLoopKeyword = "#region ";
            public const string EndLoopKeyword = "#endregion ";
        }
        public static string BuildingDirectory { get; set; }

        public static string NamespaceName { get; set; }

        public static readonly string[] SupportedIdentityKeysTypes = ["Guid", "long"];

        public static string IdentityKeysType { get; set; }

        public static Dictionary<string, string> ExternalModuleKeysTypes { get; set; } = new();

        public static string GetExternalIdentityKeysType(string fullModuleName)
        {
            return ExternalModuleKeysTypes.TryGetValue(fullModuleName, out var type) ? type : "Guid";
        }

        public static Dictionary<string, string[]> ExternalModuleTransients { get; set; } = new();

        public static bool IsExternalTransient(string fullModuleName, string objectName)
        {
            return ExternalModuleTransients.TryGetValue(fullModuleName, out var transients) && transients.Contains(objectName);
        }
        public static string ForcedPrefix { get; set; }
        // true: the default identifier (0) means "no reference" (databases without foreign
        // keys, such as PrestaShop's): it is written as is, and not checked for existence.
        public static bool ZeroIsNoReference { get; set; }
        public static Dictionary<string, string> OverrideDestinations { get; set; }

        public static bool OnlyOneDll { get; set; }
    }
}
