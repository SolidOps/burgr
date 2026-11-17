using System.Text;

namespace SolidOps.Burgr.Core
{
    // Cleans the blank lines left by generation (removed regions, markers, empty loops), for
    // the templates that ask for it (RemoveConsecutiveLineBreaks):
    // - an empty line is removed;
    // - a line of whitespace only becomes an empty line, a run of them a single one;
    // - those at the start of the file are removed.
    // The file is processed as a whole: the result of a line never depends on its position.
    public static class BlankLineCleaner
    {
        public static string Clean(string content)
        {
            var result = new StringBuilder(content.Length);
            bool previousBlank = true;
            foreach (string line in content.Split(Utilities.SingleNewLine, StringSplitOptions.RemoveEmptyEntries))
            {
                foreach (string subline in line.Split(Utilities.AltNewLine, StringSplitOptions.RemoveEmptyEntries))
                {
                    bool blank = subline.Trim().Length == 0;
                    if (blank && previousBlank)
                    {
                        continue;
                    }
                    previousBlank = blank;

                    if (result.Length > 0)
                    {
                        result.Append(Utilities.SingleNewLine);
                    }
                    if (!blank)
                    {
                        result.Append(subline);
                    }
                }
            }
            return result.ToString();
        }
    }
}
