using System.Reflection;
using System.Text.Json;

namespace SolidOps.Burgr.Core
{
    public static class TemplateCompatibility
    {
        public const string ManifestFileName = "burgr-templates.json";

        public static void Check(string templatesDirectory)
        {
            var manifest = Path.Combine(templatesDirectory, ManifestFileName);
            if (!File.Exists(manifest))
            {
                return;
            }

            using var json = JsonDocument.Parse(File.ReadAllText(manifest));
            if (!json.RootElement.TryGetProperty("MinimumBurgrVersion", out var minimumElement) || minimumElement.GetString() is not string minimum)
            {
                return;
            }

            var current = CurrentVersion();
            if (current == null)
            {
                Console.WriteLine($"Development build of burgr: templates require burgr {minimum} or later, not checked");
                return;
            }

            if (current < Version.Parse(minimum))
            {
                throw new InvalidOperationException($"These templates require burgr {minimum} or later, current version is {current}. Update the burgr tool.");
            }
        }

        // prerelease versions (0.0.0-dev for local builds) are not compared
        private static Version CurrentVersion()
        {
            var informational = typeof(TemplateCompatibility).Assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion;
            var version = informational?.Split('+')[0];
            if (string.IsNullOrEmpty(version) || version.Contains('-'))
            {
                return null;
            }
            return Version.Parse(version);
        }
    }
}
