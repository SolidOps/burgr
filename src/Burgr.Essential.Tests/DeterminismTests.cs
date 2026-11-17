using System.Diagnostics;
using System.Reflection;
using System.Text;

namespace SolidOps.Burgr.Essential.Tests;

// The samples' committed generated files are the golden files: burgr run from scratch on a
// sample must give them back byte for byte, on any machine, and a second run must change
// nothing. burgr runs as the real tool, one process per run, as a user would run it.
[TestClass]
public class DeterminismTests
{
    private const string GeneratedFilePattern = "*.g.*";
    private static readonly string[] BuildDirectories = ["bin", "obj"];
    private static readonly string[] GenerationMessages = ["Create generated file", "Update generated file", "Delete generated file"];

    [TestMethod]
    [DataRow("1. TODO list", "todo_specs")]
    [DataRow("2. User management API", "um_specs")]
    public void GenerationGivesBackTheCommittedFilesAndIsStable(string sample, string specsDirectory)
    {
        var workDirectory = Path.Combine(Path.GetTempPath(), "burgr-tests", Guid.NewGuid().ToString("N"));
        try
        {
            CopyDirectory(Path.Combine(GetMetadata("SamplesDirectory"), sample), workDirectory);
            var expected = ReadGeneratedFiles(workDirectory);
            Assert.AreNotEqual(0, expected.Count, $"no generated file committed in sample '{sample}'");
            foreach (var file in expected.Keys)
            {
                File.Delete(Path.Combine(workDirectory, file));
            }

            RunBurgr(Path.Combine(workDirectory, specsDirectory));
            var firstRun = ReadGeneratedFiles(workDirectory);
            AssertSameFiles(expected, firstRun, "committed sample", "generation from scratch");

            var output = RunBurgr(Path.Combine(workDirectory, specsDirectory));
            var secondRun = ReadGeneratedFiles(workDirectory);
            AssertSameFiles(firstRun, secondRun, "first generation", "second generation");
            var changes = output.Split('\n').Where(l => GenerationMessages.Any(l.Contains)).ToList();
            Assert.AreEqual(0, changes.Count, "second generation reported changes:\n" + string.Join("\n", changes));
        }
        finally
        {
            if (Directory.Exists(workDirectory))
            {
                Directory.Delete(workDirectory, true);
            }
        }
    }

    private static string RunBurgr(string specsDirectory)
    {
        var startInfo = new ProcessStartInfo(Environment.GetEnvironmentVariable("DOTNET_HOST_PATH") ?? "dotnet")
        {
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
        };
        startInfo.ArgumentList.Add(GetMetadata("BurgrToolPath"));
        startInfo.ArgumentList.Add(specsDirectory);
        // A culture where "I".ToLower() is a dotless "ı": the output must not change. Taken
        // into account outside Windows only (the CI), where .NET reads the culture from them.
        startInfo.Environment["LC_ALL"] = "tr_TR.UTF-8";
        startInfo.Environment["LANG"] = "tr_TR.UTF-8";

        using var process = Process.Start(startInfo)!;
        var error = process.StandardError.ReadToEndAsync();
        var output = process.StandardOutput.ReadToEnd();
        process.WaitForExit();
        output += error.Result;

        // burgr reports a generation error on the console but exits with -1.
        Assert.AreEqual(0, process.ExitCode, "burgr failed:\n" + output);
        Assert.IsFalse(output.Contains("Error occurred in Generation"), "burgr failed:\n" + output);
        return output;
    }

    private static Dictionary<string, byte[]> ReadGeneratedFiles(string directory)
    {
        return Directory.GetFiles(directory, GeneratedFilePattern, SearchOption.AllDirectories)
            .Select(f => Path.GetRelativePath(directory, f))
            .Where(f => !f.Split(Path.DirectorySeparatorChar).Any(BuildDirectories.Contains))
            .ToDictionary(f => f.Replace('\\', '/'), f => File.ReadAllBytes(Path.Combine(directory, f)));
    }

    private static void AssertSameFiles(Dictionary<string, byte[]> expected, Dictionary<string, byte[]> actual, string expectedName, string actualName)
    {
        var report = new StringBuilder();
        foreach (var file in expected.Keys.Except(actual.Keys).Order())
        {
            report.AppendLine($"only in {expectedName}: {file}");
        }
        foreach (var file in actual.Keys.Except(expected.Keys).Order())
        {
            report.AppendLine($"only in {actualName}: {file}");
        }
        foreach (var file in expected.Keys.Intersect(actual.Keys).Order())
        {
            if (!expected[file].AsSpan().SequenceEqual(actual[file]))
            {
                report.AppendLine($"different: {file} ({DescribeDifference(expected[file], actual[file])})");
            }
        }
        Assert.AreEqual(0, report.Length, $"{actualName} differs from {expectedName}:\n{report}");
    }

    private static string DescribeDifference(byte[] expected, byte[] actual)
    {
        var expectedLines = Encoding.UTF8.GetString(expected).Split('\n');
        var actualLines = Encoding.UTF8.GetString(actual).Split('\n');
        for (int i = 0; i < Math.Max(expectedLines.Length, actualLines.Length); i++)
        {
            var expectedLine = i < expectedLines.Length ? expectedLines[i] : "<end of file>";
            var actualLine = i < actualLines.Length ? actualLines[i] : "<end of file>";
            if (expectedLine != actualLine)
            {
                return $"line {i + 1}: expected {Escape(expectedLine)}, got {Escape(actualLine)}";
            }
        }
        return "same lines, different bytes";
    }

    private static string Escape(string line) => "\"" + line.Replace("\r", "\\r") + "\"";

    private static void CopyDirectory(string source, string destination)
    {
        Directory.CreateDirectory(destination);
        foreach (var file in Directory.GetFiles(source))
        {
            File.Copy(file, Path.Combine(destination, Path.GetFileName(file)));
        }
        foreach (var directory in Directory.GetDirectories(source))
        {
            var name = Path.GetFileName(directory);
            if (!BuildDirectories.Contains(name))
            {
                CopyDirectory(directory, Path.Combine(destination, name));
            }
        }
    }

    private static string GetMetadata(string key)
    {
        return typeof(DeterminismTests).Assembly.GetCustomAttributes<AssemblyMetadataAttribute>().Single(a => a.Key == key).Value!;
    }
}
