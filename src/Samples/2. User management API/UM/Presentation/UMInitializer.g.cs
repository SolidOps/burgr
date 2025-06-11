using Extensions.Hosting.AsyncInitialization;
using Microsoft.Extensions.DependencyInjection;
using SolidOps.Burgr.Shared.Presentation.ETag;
namespace SolidOps.UM.Presentation;
public partial class UMPresentationInitializer : BaseUMPresentationInitializer, IAsyncInitializer
{
    public UMPresentationInitializer(IServiceProvider serviceProvider) : base(serviceProvider)
    {
    }
    public async Task InitializeAsync(CancellationToken cancellationToken)
    {
        var etagConfiguration = serviceProvider.GetRequiredService<ETagConfiguration>();

        // Object [AG][CACHE]
        etagConfiguration.ManageEtagsFor(typeof(Domain.AggregateRoots.Organization));

        await AdditionalInitialization(this.serviceProvider, cancellationToken);
    }
}
public abstract class BaseUMPresentationInitializer
{
    protected readonly IServiceProvider serviceProvider;
    public BaseUMPresentationInitializer(IServiceProvider serviceProvider)
    {
        this.serviceProvider = serviceProvider;
    }
    protected virtual async Task AdditionalInitialization(IServiceProvider serviceProvider, CancellationToken cancellationToken)
    {
        await Task.CompletedTask;
    }
}