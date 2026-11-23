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
        public string Starter { get; set; }
        public string ProjectFile { get; set; }
        public TemplateTokens Tokens { get; set; }
        public TemplateEnumType EnumType { get; set; }

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

        // Copies the set's starter project into an empty specs directory. The starter is laid out
        // as a repository: its specs/ folder (burgr.json and YAML) goes into the specs directory,
        // the rest (hand-written code next to the generated one) into the specs directory's
        // parent, never overwriting a file.
        public List<string> WriteStarter(string templatesDirectory, string specsDirectory)
        {
            if (string.IsNullOrEmpty(Starter))
            {
                throw new InvalidOperationException($"{specsDirectory} is empty and this template set has no starter model (no Starter in {FileName}): add a burgr.json and a YAML model to it");
            }

            const string StarterSpecs = "specs/";
            var source = Path.Combine(templatesDirectory, Starter);
            var root = Path.GetDirectoryName(Path.TrimEndingDirectorySeparator(specsDirectory));
            var written = new List<string>();
            foreach (var file in FileSystemOrder.SortRecursive(Directory.GetFiles(source, "*", SearchOption.AllDirectories), source))
            {
                var relative = Path.GetRelativePath(source, file).Replace('\\', '/');
                var destination = relative.StartsWith(StarterSpecs, StringComparison.Ordinal)
                    ? Path.Combine(specsDirectory, relative.Substring(StarterSpecs.Length))
                    : Path.Combine(root, relative);
                if (File.Exists(destination))
                {
                    Console.WriteLine($"Starter file kept: {relative}");
                    continue;
                }
                Directory.CreateDirectory(Path.GetDirectoryName(destination));
                File.Copy(file, destination);
                Console.WriteLine($"Starter file written: {relative}");
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
