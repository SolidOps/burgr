using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SolidOps.UM.Shared.Tests;

[TestClass]
public abstract class BaseAppServiceTest<TTestSetup>
    where TTestSetup : TestSetup, new()
{
    protected int DBState = (int)DBStateEnum.Empty;

    protected Dictionary<int, string> Suffixes;
    protected readonly List<string> AlternateConnectionStringFiles = new List<string>();

    protected TTestSetup TestSetup { get; set; }

    public BaseAppServiceTest()
    {
        Suffixes = new Dictionary<int, string>();
        Suffixes.Add((int)DBStateEnum.Minimal, "_minimal");
        Suffixes.Add((int)DBStateEnum.Empty, "_empty");

        TestSetup = new TTestSetup();
        TestSetup.Suffixes = Suffixes;
    }

    [TestInitialize]
    public async Task Initialize()
    {
        await BeforeInitialize();

        await TestSetup.Start();

        await AfterInitialize();
    }

    public virtual async Task BeforeInitialize()
    {
        Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "tests");
        TestSetup.BeforeStart();
        await Task.CompletedTask;
    }

    public virtual async Task AfterInitialize()
    {
        await Task.CompletedTask;
    }

    [TestCleanup]
    public async Task Cleanup()
    {
        await BeforeCleanup();
        await TestSetup.Cleanup();
        await AfterCleanup();
    }

    public virtual async Task BeforeCleanup()
    {
        await Task.CompletedTask;
    }

    public virtual async Task AfterCleanup()
    {
        await Task.CompletedTask;
    }
}
