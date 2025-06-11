using Extensions.Hosting.AsyncInitialization;
using Microsoft.Extensions.DependencyInjection;

namespace SolidOps.TODO.Shared.Infrastructure;

public class DBInitializer : IAsyncInitializer
{
    private readonly IServiceProvider serviceProvider;

    public DBInitializer(IServiceProvider serviceProvider)
    {
        this.serviceProvider = serviceProvider;
    }

    public virtual async Task InitializeAsync(CancellationToken cancellationToken)
    {
        using (var scope = serviceProvider.CreateScope())
        {
            var dbContextFactories = scope.ServiceProvider.GetRequiredService<IEnumerable<IDBContextFactory>>();
            
            if (dbContextFactories != null)
            {
                foreach (var dbContextFactory in dbContextFactories)
                {
                    await dbContextFactory.EnsureDataAccessAndMigration(scope.ServiceProvider);
                }
            }
        }
    }
}
