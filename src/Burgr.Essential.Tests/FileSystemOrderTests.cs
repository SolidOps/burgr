using SolidOps.Burgr.Core;

namespace SolidOps.Burgr.Essential.Tests;

[TestClass]
public class FileSystemOrderTests
{
    [TestMethod]
    public void SortIsCaseInsensitiveOrdinalAsOnNtfs()
    {
        var sorted = FileSystemOrder.Sort(["b", "aggregate_roots.yaml", "A", "aggregates.yaml"]);

        CollectionAssert.AreEqual(new[] { "A", "aggregates.yaml", "aggregate_roots.yaml", "b" }, sorted);
    }

    [TestMethod]
    public void SortRecursiveListsFilesOfADirectoryBeforeItsSubDirectories()
    {
        var root = Path.Combine(Path.GetTempPath(), "specs");
        string[] files =
        [
            Path.Combine(root, "Zeta", "services.yaml"),
            Path.Combine(root, "z.yaml"),
            Path.Combine(root, "alpha", "inner", "a.yaml"),
            Path.Combine(root, "Zeta", "aggregates.yaml"),
            Path.Combine(root, "alpha", "b.yaml"),
            Path.Combine(root, "A.yaml"),
        ];

        var sorted = FileSystemOrder.SortRecursive(files, root).Select(f => Path.GetRelativePath(root, f).Replace('\\', '/'));

        CollectionAssert.AreEqual(new[]
        {
            "A.yaml",
            "z.yaml",
            "alpha/b.yaml",
            "Zeta/aggregates.yaml",
            "Zeta/services.yaml",
            "alpha/inner/a.yaml",
        }, sorted.ToArray());
    }
}
