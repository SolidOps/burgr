using Extensions.Hosting.AsyncInitialization;
using Microsoft.Extensions.DependencyInjection;

namespace MetaCorp.Template.Application;

public partial class TemplateApplicationInitializer : BaseTemplateApplicationInitializer, IAsyncInitializer
{
    public TemplateApplicationInitializer(IServiceProvider serviceProvider) : base(serviceProvider)
    {
        #region foreach MODEL
        //
        #endregion foreach MODEL
    }

    public async Task InitializeAsync(CancellationToken cancellationToken)
    {
        await AdditionalInitialization(this.serviceProvider, cancellationToken);
    }

}

public abstract class BaseTemplateApplicationInitializer
{
    protected readonly IServiceProvider serviceProvider;

    public BaseTemplateApplicationInitializer(IServiceProvider serviceProvider)
    {
        this.serviceProvider = serviceProvider;
    }

    protected virtual async Task AdditionalInitialization(IServiceProvider serviceProvider, CancellationToken cancellationToken)
    {
        await Task.CompletedTask;
    }
}