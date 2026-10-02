using System.Text.RegularExpressions;

namespace SolidOps.Burgr.Core
{
    public static partial class DuplicateLineCleaner
    {
        [GeneratedRegex(@"^using [\w.]+;$")]
        private static partial Regex CsUsing();

        [GeneratedRegex(@"^import .*;$")]
        private static partial Regex JsImport();

        public static string Clean(string content, string destinationLanguage)
        {
            if (destinationLanguage != "CS" && destinationLanguage != "JS")
            {
                return content;
            }

            Regex headerLine = destinationLanguage == "CS" ? CsUsing() : JsImport();
            HashSet<string> seen = new();
            List<string> lines = new();
            string previous = null;

            foreach (string line in content.Split('\n'))
            {
                string key = line.TrimEnd('\r');
                bool duplicate =
                    (headerLine.IsMatch(key) && !seen.Add(key)) ||
                    (key.StartsWith("// ") && key == previous);
                previous = key;
                if (!duplicate)
                {
                    lines.Add(line);
                }
            }

            return string.Join("\n", lines);
        }
    }
}
