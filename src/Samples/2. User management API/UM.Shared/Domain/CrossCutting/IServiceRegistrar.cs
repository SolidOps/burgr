using Microsoft.Extensions.DependencyInjection;
using SolidOps.UM.Shared.Domain.Configuration;

namespace SolidOps.UM.Shared.Domain.CrossCutting;

public interface IServiceRegistrar
{
    int Priority { get; }
    void ConfigureServices(IServiceCollection services, IExtendedConfiguration configuration);
}
