using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using SolidOps.TODO.Shared;
namespace SolidOps.TODO.Application;
public static class AssemblyReference
{
}
public partial class TODOApplicationServiceRegistrar : IServiceRegistrar
{
    public int Priority => 1;
    public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        // UseCase [I]
        services.AddTransient<Contracts.UseCases.IAddItemUseCase, Application.UseCases.AddItemUseCase>();

        services.AddTransient<Contracts.UseCases.IUpdateItemUseCase, Application.UseCases.UpdateItemUseCase>();

        services.AddTransient<Contracts.UseCases.IGetItemsUseCase, Application.UseCases.GetItemsUseCase>();

        ConfigureAdditionalServices(services, configuration);
    }
    partial void ConfigureAdditionalServices(IServiceCollection services, IConfiguration configuration);
}