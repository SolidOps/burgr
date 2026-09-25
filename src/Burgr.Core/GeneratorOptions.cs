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
        public static string ForcedPrefix { get; set; }
        public static Dictionary<string, string> OverrideDestinations { get; set; }

        public static bool OnlyOneDll { get; set; }
    }
}
