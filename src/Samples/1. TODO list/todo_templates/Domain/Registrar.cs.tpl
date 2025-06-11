using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SolidOps.TODO.Shared;
using SolidOps.TODO.Shared.Domain;

namespace MetaCorp.Template.Domain;

public static class AssemblyReference
{
}

public partial class TemplateDomainServiceRegistrar : IServiceRegistrar
{
    public int Priority => 1;
    public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        #region foreach MODEL[EN][AG]
        services.AddTransient<IEntityRules<_IDENTITY_KEY_TYPE_, Domain._DOMAINTYPE_.CLASSNAME>, Domain._DOMAINTYPE_.Rules.CLASSNAMERules>();
        #endregion foreach MODEL

        ConfigureAdditionalServices(services, configuration);
    }

    partial void ConfigureAdditionalServices(IServiceCollection services, IConfiguration configuration);
}
