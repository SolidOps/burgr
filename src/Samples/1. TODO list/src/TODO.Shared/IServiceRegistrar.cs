using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace SolidOps.TODO.Shared;

public interface IServiceRegistrar
{
    void ConfigureServices(IServiceCollection services, IConfiguration configuration);
}
