using Extensions.Hosting.AsyncInitialization;
using Microsoft.Extensions.DependencyInjection;
using SolidOps.UM.Shared.Domain.Configuration;

namespace SolidOps.UM.Shared.Application;

public class BurgrInitializer : IAsyncInitializer
{
    private readonly IServiceProvider serviceProvider;

    public BurgrInitializer(IServiceProvider serviceProvider)
    {
        this.serviceProvider = serviceProvider;
    }

    public virtual async Task InitializeAsync(CancellationToken cancellationToken)
    {
        await this.serviceProvider.GetRequiredService<IExtendedConfiguration>().Reload(serviceProvider);
        await this.serviceProvider.GetRequiredService<IBackgroundServicesRunner>().RunBackgroundServices();
    }
}
