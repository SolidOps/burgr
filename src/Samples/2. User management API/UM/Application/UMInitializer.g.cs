using Extensions.Hosting.AsyncInitialization;
using Microsoft.Extensions.DependencyInjection;
namespace SolidOps.UM.Application;
public partial class UMApplicationInitializer : BaseUMApplicationInitializer, IAsyncInitializer
{
    public UMApplicationInitializer(IServiceProvider serviceProvider) : base(serviceProvider)
    {
        // Object 
        //

        //

        //

        //

        //

        //

        //

        //

        //

    }
    public async Task InitializeAsync(CancellationToken cancellationToken)
    {
        await AdditionalInitialization(this.serviceProvider, cancellationToken);
    }
}
public abstract class BaseUMApplicationInitializer
{
    protected readonly IServiceProvider serviceProvider;
    public BaseUMApplicationInitializer(IServiceProvider serviceProvider)
    {
        this.serviceProvider = serviceProvider;
    }
    protected virtual async Task AdditionalInitialization(IServiceProvider serviceProvider, CancellationToken cancellationToken)
    {
        await Task.CompletedTask;
    }
}