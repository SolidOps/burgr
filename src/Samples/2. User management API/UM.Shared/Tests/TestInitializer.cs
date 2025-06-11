using Extensions.Hosting.AsyncInitialization;
using Microsoft.Extensions.DependencyInjection;
using SolidOps.UM.Shared.Domain.Configuration;
using SolidOps.UM.Shared.Domain.UnitOfWork;
using SolidOps.UM.Shared.Infrastructure.EF;

namespace SolidOps.UM.Shared.Tests;

public class TestInitializer : IAsyncInitializer
{
    private readonly IServiceProvider serviceProvider;

    public TestInitializer(IServiceProvider serviceProvider)
    {
        this.serviceProvider = serviceProvider;
    }

    public virtual async Task InitializeAsync(CancellationToken cancellationToken)
    {
        await Task.CompletedTask;

        var config = serviceProvider.GetRequiredService<IExtendedConfiguration>();

        var suffix = config["Suffix"];
        var eventOption = Enum.Parse<EventOption>(config["EventOption"]);
        var DBState = Enum.Parse<DBStateEnum>(config["DBState"]);

        foreach (var kvp in config.BurgrConfiguration.Databases)
        {
            var parts = kvp.Value.ConnectionString.Split(new char[] { ';' });
            for (var i = 0; i < parts.Length; i++)
            {
                // sql
                if (parts[i].StartsWith("database="))
                {
                    if (!string.IsNullOrEmpty(suffix) && !parts[i].EndsWith(suffix))
                    {
                        parts[i] += suffix;
                    }
                }
                // file
                if (parts[i].StartsWith("DataSource="))
                {
                    if (!string.IsNullOrEmpty(suffix) && !parts[i].EndsWith(suffix))
                    {
                        parts[i] += suffix + ".db";
                    }
                }
            }
            kvp.Value.ConnectionString = string.Join(";", parts);
        }
        config["EventOption"] = eventOption.ToString();

        using (var scope = serviceProvider.CreateScope())
        {
            var dbContextFactories = scope.ServiceProvider.GetRequiredService<IEnumerable<IBurgrDBContextFactory>>();
            if (dbContextFactories != null)
            {
                if (DBState == (int)DBStateEnum.Empty)
                {
                    foreach (var dbContextFactory in dbContextFactories)
                    {
                        dbContextFactory.DeleteAllModuleData(scope.ServiceProvider).Wait();
                    }
                }
            }
        }
    }
}
