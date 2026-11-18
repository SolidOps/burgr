using System.Diagnostics;
using System.Reflection;
using System.Text;

namespace SolidOps.Burgr.Essential.Tests;

// Golden files: burgr run from scratch must give back the expected files byte for byte, on
// any machine, and a second run must change nothing. burgr runs as the real tool, one
// process per run, from a directory other than the specs one, as a user would run it.
[TestClass]
public class DeterminismTests
{
    private const string GeneratedFilePattern = "*.g.*";
    private const string AllFilesPattern = "*";
    private const string UpdateReferenceVariable = "BURGR_UPDATE_REFERENCE";
    private static readonly string[] BuildDirectories = ["bin", "obj"];
    private static readonly string[] GenerationMessages = ["Create generated file", "Update generated file", "Delete generated file"];

    // The samples' committed generated files are their golden files. A sample without a
    // template source in its burgr.json uses one of the template sets of src/Templates.
    [TestMethod]
    [DataRow("1. TODO list", "todo_specs", null)]
    [DataRow("2. User management API", "um_specs", null)]
    [DataRow("3. Mini API", "specs", "Mini")]
    public void SampleGenerationGivesBackTheCommittedFilesAndIsStable(string sample, string specsDirectory, string? templateSet)
    {
        var options = templateSet == null
            ? []
            : new[] { "/templateSpecDirectory:" + Path.Combine(GetMetadata("TemplatesDirectory"), templateSet, "templates") };
        InWorkDirectory(workDirectory =>
        {
            CopyDirectory(Path.Combine(GetMetadata("SamplesDirectory"), sample), workDirectory);
            var expected = ReadFiles(workDirectory, GeneratedFilePattern);
            Assert.AreNotEqual(0, expected.Count, $"no generated file committed in sample '{sample}'");
            foreach (var file in expected.Keys)
            {
                File.Delete(Path.Combine(workDirectory, file));
            }

            var specs = Path.Combine(workDirectory, specsDirectory);
            RunBurgr(specs, options);
            var firstRun = ReadFiles(workDirectory, GeneratedFilePattern);
            AssertSameFiles(expected, firstRun, "committed sample", "generation from scratch");
            AssertSecondRunChangesNothing(specs, workDirectory, GeneratedFilePattern, firstRun, options);
        });
    }

    // Reference/ covers what the samples do not: two modules and model files whose names
    // differ in case, per-model files written to folders that do not exist yet (with
    // children_level), path_style Lower, TypeScript, HTML, SQL and PlantUML outputs, rights,
    // a composite key, a framework file, templates with LF and CRLF line endings, and a burgr.json without
    // ModelSpecDirectory. Run with BURGR_UPDATE_REFERENCE=1 to replace Reference/expected
    // with the current output, after an intended change of the generated code.
    [TestMethod]
    public void ReferenceGenerationGivesTheExpectedFilesAndIsStable()
    {
        InWorkDirectory(workDirectory =>
        {
            var reference = GetMetadata("ReferenceDirectory");
            CopyDirectory(Path.Combine(reference, "specs"), Path.Combine(workDirectory, "specs"));
            CopyDirectory(Path.Combine(reference, "templates"), Path.Combine(workDirectory, "templates"));
            var specs = Path.Combine(workDirectory, "specs");
            var output = Path.Combine(workDirectory, "output");

            RunBurgr(specs);
            var firstRun = ReadFiles(output, AllFilesPattern);

            var expectedDirectory = Path.Combine(reference, "expected");
            if (Environment.GetEnvironmentVariable(UpdateReferenceVariable) == "1")
            {
                if (Directory.Exists(expectedDirectory))
                {
                    Directory.Delete(expectedDirectory, true);
                }
                CopyDirectory(output, expectedDirectory);
                Assert.Inconclusive($"{expectedDirectory} replaced by the current output: review its diff");
            }

            AssertSameFiles(ReadFiles(expectedDirectory, AllFilesPattern), firstRun, "Reference/expected", "generation from scratch");
            AssertSecondRunChangesNothing(specs, output, AllFilesPattern, firstRun);
        });
    }

    internal static void InWorkDirectory(Action<string> test)
    {
        var workDirectory = Path.Combine(Path.GetTempPath(), "burgr-tests", Guid.NewGuid().ToString("N"));
        try
        {
            test(workDirectory);
        }
        finally
        {
            if (Directory.Exists(workDirectory))
            {
                Directory.Delete(workDirectory, true);
            }
        }
    }

    private static void AssertSecondRunChangesNothing(string specs, string outputDirectory, string pattern, Dictionary<string, byte[]> firstRun, params string[] options)
    {
        var output = RunBurgr(specs, options);
        AssertSameFiles(firstRun, ReadFiles(outputDirectory, pattern), "first generation", "second generation");
        var changes = output.Split('\n').Where(l => GenerationMessages.Any(l.Contains)).ToList();
        Assert.AreEqual(0, changes.Count, "second generation reported changes:\n" + string.Join("\n", changes));
    }

    internal static string RunBurgr(string specsDirectory, params string[] options)
    {
        var (exitCode, output) = StartBurgr(specsDirectory, options);

        // burgr reports a generation error on the console but exits with -1.
        Assert.AreEqual(0, exitCode, "burgr failed:\n" + output);
        Assert.IsFalse(output.Contains("Error occurred in Generation"), "burgr failed:\n" + output);
        return output;
    }

    internal static (int ExitCode, string Output) StartBurgr(string specsDirectory, params string[] options)
    {
        var startInfo = new ProcessStartInfo(Environment.GetEnvironmentVariable("DOTNET_HOST_PATH") ?? "dotnet")
        {
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
        };
        startInfo.ArgumentList.Add(GetMetadata("BurgrToolPath"));
        startInfo.ArgumentList.Add(specsDirectory);
        foreach (var option in options)
        {
            startInfo.ArgumentList.Add(option);
        }
        // A culture where "I".ToLower() is a dotless "ı": the output must not change. Taken
        // into account outside Windows only (the CI), where .NET reads the culture from them.
        startInfo.Environment["LC_ALL"] = "tr_TR.UTF-8";
        startInfo.Environment["LANG"] = "tr_TR.UTF-8";

        using var process = Process.Start(startInfo)!;
        var error = process.StandardError.ReadToEndAsync();
        var output = process.StandardOutput.ReadToEnd();
        process.WaitForExit();
        output += error.Result;
        return (process.ExitCode, output);
    }

    private static Dictionary<string, byte[]> ReadFiles(string directory, string pattern)
    {
        return Directory.GetFiles(directory, pattern, SearchOption.AllDirectories)
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

    internal static void CopyDirectory(string source, string destination)
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

    internal static string GetMetadata(string key)
    {
        return typeof(DeterminismTests).Assembly.GetCustomAttributes<AssemblyMetadataAttribute>().Single(a => a.Key == key).Value!;
    }
}
