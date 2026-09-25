using System.IO.Compression;

namespace SolidOps.Burgr.Core
{
    public static class TemplatePackageResolver
    {
        public const string DefaultSource = "https://api.nuget.org/v3-flatcontainer/";
        private const string TemplatesFolder = "templates";

        public static string Resolve(TemplatePackageConfig package)
        {
            if (string.IsNullOrEmpty(package.Id) || string.IsNullOrEmpty(package.Version))
            {
                throw new ArgumentException("TemplatePackage requires both Id and Version");
            }

            var id = package.Id.ToLowerInvariant();
            var version = package.Version.ToLowerInvariant();

            var nugetPackages = Environment.GetEnvironmentVariable("NUGET_PACKAGES")
                ?? Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".nuget", "packages");
            var restored = Path.Combine(nugetPackages, id, version, TemplatesFolder);
            if (Directory.Exists(restored))
            {
                return restored;
            }

            var cached = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "burgr", "packages", id, version);
            var cachedTemplates = Path.Combine(cached, TemplatesFolder);
            if (!Directory.Exists(cachedTemplates))
            {
                Download(package.Source ?? DefaultSource, id, version, cached);
            }
            return cachedTemplates;
        }

        private static void Download(string source, string id, string version, string destination)
        {
            var url = $"{source.TrimEnd('/')}/{id}/{version}/{id}.{version}.nupkg";
            Console.WriteLine($"Downloading template package {url}");

            using var httpClient = new HttpClient();
            using var response = httpClient.GetAsync(url).GetAwaiter().GetResult();
            if (!response.IsSuccessStatusCode)
            {
                throw new InvalidOperationException($"Template package {id} {version} could not be downloaded from {url} ({(int)response.StatusCode})");
            }

            var temporary = destination + ".tmp";
            if (Directory.Exists(temporary))
            {
                Directory.Delete(temporary, true);
            }
            var temporaryRoot = Path.GetFullPath(temporary) + Path.DirectorySeparatorChar;

            using (var archive = new ZipArchive(response.Content.ReadAsStream()))
            {
                foreach (var entry in archive.Entries.Where(e => e.FullName.StartsWith(TemplatesFolder + "/", StringComparison.Ordinal) && !string.IsNullOrEmpty(e.Name)))
                {
                    var path = Path.GetFullPath(Path.Combine(temporary, Uri.UnescapeDataString(entry.FullName)));
                    if (!path.StartsWith(temporaryRoot, StringComparison.OrdinalIgnoreCase))
                    {
                        throw new InvalidOperationException($"Template package {id} {version} contains an invalid entry: {entry.FullName}");
                    }
                    Directory.CreateDirectory(Path.GetDirectoryName(path));
                    entry.ExtractToFile(path);
                }
            }

            Directory.CreateDirectory(Path.GetDirectoryName(destination));
            if (Directory.Exists(destination))
            {
                Directory.Delete(destination, true);
            }
            Directory.Move(temporary, destination);
        }
    }
}
