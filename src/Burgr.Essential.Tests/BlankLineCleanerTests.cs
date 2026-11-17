using SolidOps.Burgr.Core;

namespace SolidOps.Burgr.Essential.Tests;

[TestClass]
public class BlankLineCleanerTests
{
    [TestMethod]
    public void EmptyLinesAreRemovedAndWhitespaceRunsBecomeOneEmptyLine()
    {
        var content = "    \r\na\r\n\r\nb\r\n    \r\n  \r\n\r\nc\nd\n\n  \ne";

        Assert.AreEqual("a\r\nb\r\n\r\nc\r\nd\r\n\r\ne", BlankLineCleaner.Clean(content));
    }

    [TestMethod]
    public void ResultDoesNotDependOnThePositionInTheFile()
    {
        const string snippet = "class A\r\n{\r\n    \r\n    int b;\r\n}\r\n";
        var expected = BlankLineCleaner.Clean(snippet);

        // shifts the snippet across the 10000-character blocks the former implementation used
        for (int length = 9960; length < 10040; length++)
        {
            var padding = new string('x', length) + "\r\n";
            var cleaned = BlankLineCleaner.Clean(padding + snippet);

            Assert.AreEqual(padding + expected, cleaned, $"padding of {length} characters");
        }
    }
}
