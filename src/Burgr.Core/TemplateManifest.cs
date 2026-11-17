using System.Reflection;
using System.Text.Json;

namespace SolidOps.Burgr.Core
{
    // burgr-templates.json, at the root of a template set: what the set needs and declares.
    public class TemplateManifest
    {
        public const string FileName = "burgr-templates.json";

        public string MinimumBurgrVersion { get; set; }
        public string Description { get; set; }
        public string[] Templates { get; set; }
        public string[] Generators { get; set; }
        public string[] SupportedFeatures { get; set; }
        public TemplateDependency[] Dependencies { get; set; }
        public string Harness { get; set; }
        public string ProjectFile { get; set; }

        public static TemplateManifest Read(string templatesDirectory)
        {
            var path = Path.Combine(templatesDirectory, FileName);
            if (!File.Exists(path))
            {
                return null;
            }
            return JsonSerializer.Deserialize<TemplateManifest>(File.ReadAllText(path));
        }

        public void CheckBurgrVersion()
        {
            if (string.IsNullOrEmpty(MinimumBurgrVersion))
            {
                return;
            }

            var current = CurrentBurgrVersion();
            if (current == null)
            {
                Console.WriteLine($"Development build of burgr: templates require burgr {MinimumBurgrVersion} or later, not checked");
                return;
            }

            if (current < Version.Parse(MinimumBurgrVersion))
            {
                throw new InvalidOperationException($"These templates require burgr {MinimumBurgrVersion} or later, current version is {current}. Update the burgr tool.");
            }
        }

        public void CheckFeatures(IEnumerable<string> usedFeatures)
        {
            if (SupportedFeatures == null)
            {
                return;
            }

            var unsupported = usedFeatures.Except(SupportedFeatures, StringComparer.Ordinal).Order(StringComparer.Ordinal).ToList();
            if (unsupported.Count > 0)
            {
                throw new InvalidOperationException($"The model uses features this template set does not support: {string.Join(", ", unsupported)}. Supported: {string.Join(", ", SupportedFeatures)}.");
            }
        }

        // Copies the set's harness into target, never overwriting a file already there.
        public List<string> InstallHarness(string templatesDirectory, string target)
        {
            if (string.IsNullOrEmpty(Harness))
            {
                throw new InvalidOperationException("This template set has no harness (no Harness in " + FileName + ")");
            }

            var source = Path.Combine(templatesDirectory, Harness);
            var written = new List<string>();
            foreach (var file in FileSystemOrder.SortRecursive(Directory.GetFiles(source, "*", SearchOption.AllDirectories), source))
            {
                var relative = Path.GetRelativePath(source, file).Replace('\\', '/');
                var destination = Path.Combine(target, relative);
                if (File.Exists(destination))
                {
                    Console.WriteLine($"Harness file kept: {relative}");
                    continue;
                }
                Directory.CreateDirectory(Path.GetDirectoryName(destination));
                File.Copy(file, destination);
                Console.WriteLine($"Harness file written: {relative}");
                written.Add(relative);
            }
            return written;
        }

        // prerelease versions (0.0.0-dev for local builds) are not compared
        public static Version CurrentBurgrVersion()
        {
            var informational = typeof(TemplateManifest).Assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion;
            var version = informational?.Split('+')[0];
            if (string.IsNullOrEmpty(version) || version.Contains('-'))
            {
                return null;
            }
            return Version.Parse(version);
        }
    }

    public class TemplateDependency
    {
        public string Id { get; set; }
        public string Version { get; set; }
    }
}
