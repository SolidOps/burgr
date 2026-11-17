namespace SolidOps.Burgr.Core
{
    // Directory.GetFiles / GetDirectories return entries in the file system's order: sorted
    // on NTFS, but arbitrary on ext4 or APFS. Model and template files are read in that order,
    // which sets the order of the generated code, so burgr sorts them itself. The order is the
    // one NTFS gives (case-insensitive ordinal, files of a directory before those of its
    // sub-directories), so that output generated on Windows does not change.
    public static class FileSystemOrder
    {
        public static string[] Sort(IEnumerable<string> paths)
        {
            return paths.OrderBy(p => p, StringComparer.OrdinalIgnoreCase).ToArray();
        }

        public static string[] SortRecursive(IEnumerable<string> paths, string rootDirectory)
        {
            string root = Path.GetFullPath(rootDirectory);
            return paths
                .Select(p => (Path: p, Directories: GetRelativeDirectories(root, p)))
                .OrderBy(p => p.Directories.Length)
                .ThenBy(p => p.Directories, DirectoriesComparer.Instance)
                .ThenBy(p => Path.GetFileName(p.Path), StringComparer.OrdinalIgnoreCase)
                .Select(p => p.Path)
                .ToArray();
        }

        private static string[] GetRelativeDirectories(string root, string path)
        {
            string relative = Path.GetRelativePath(root, Path.GetDirectoryName(Path.GetFullPath(path)));
            return relative == "."
                ? []
                : relative.Split([Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar], StringSplitOptions.RemoveEmptyEntries);
        }

        private sealed class DirectoriesComparer : IComparer<string[]>
        {
            public static readonly DirectoriesComparer Instance = new();

            public int Compare(string[] x, string[] y)
            {
                for (int i = 0; i < Math.Min(x.Length, y.Length); i++)
                {
                    int result = StringComparer.OrdinalIgnoreCase.Compare(x[i], y[i]);
                    if (result != 0)
                    {
                        return result;
                    }
                }
                return x.Length.CompareTo(y.Length);
            }
        }
    }
}
