using SolidOps.Burgr.Core;

namespace SolidOps.Burgr.Essential.Tests;

[TestClass]
public class DuplicateLineCleanerTests
{
    [TestMethod]
    public void RepeatedUsingKeepsFirstOccurrence()
    {
        var input = "using A.B;\nusing A.B;\nusing C;\nusing A.B;\nclass X {}";

        Assert.AreEqual("using A.B;\nusing C;\nclass X {}", DuplicateLineCleaner.Clean(input, "CS"));
    }

    [TestMethod]
    public void UsingStatementsAndAliasesAreUntouched()
    {
        var input = "using X = Y;\nusing X = Y;\n    using (var a = b)\n    using var c = d;\n    using var c = d;\nusing static A.B;\nusing static A.B;";

        Assert.AreEqual(input, DuplicateLineCleaner.Clean(input, "CS"));
    }

    [TestMethod]
    public void RepeatedImportIsDeduplicatedButDistinctNamesFromSameModuleStay()
    {
        var input = "import X from 'a';\nimport X from 'a';\nimport { a } from 'x';\nimport { b } from 'x';\nimport { a } from 'x';";

        Assert.AreEqual("import X from 'a';\nimport { a } from 'x';\nimport { b } from 'x';", DuplicateLineCleaner.Clean(input, "JS"));
    }

    [TestMethod]
    public void ConsecutiveMarkersCollapseButSeparatedOnesStay()
    {
        var input = "// Object [EN]\n// Object [EN]\n// Object [EN]\ncode;\n// Object [EN]\nmore;";

        Assert.AreEqual("// Object [EN]\ncode;\n// Object [EN]\nmore;", DuplicateLineCleaner.Clean(input, "CS"));
    }

    [TestMethod]
    public void CrLfLinesAreComparedWithoutCarriageReturn()
    {
        var input = "using A;\r\nusing A;\r\nclass X {}";

        Assert.AreEqual("using A;\r\nclass X {}", DuplicateLineCleaner.Clean(input, "CS"));
    }

    [TestMethod]
    public void OtherLanguagesAreUntouched()
    {
        var input = "using A;\nusing A;";

        Assert.AreEqual(input, DuplicateLineCleaner.Clean(input, "MySQL"));
    }
}
