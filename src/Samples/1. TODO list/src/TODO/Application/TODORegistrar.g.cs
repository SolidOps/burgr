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
        // Service 
        services.AddTransient<Contracts.Services.IAddItemService, Application.Services.AddItemService>();

        services.AddTransient<Contracts.Services.IUpdateItemService, Application.Services.UpdateItemService>();

        services.AddTransient<Contracts.Services.IGetItemsService, Application.Services.GetItemsService>();

        ConfigureAdditionalServices(services, configuration);
    }
    partial void ConfigureAdditionalServices(IServiceCollection services, IConfiguration configuration);
}