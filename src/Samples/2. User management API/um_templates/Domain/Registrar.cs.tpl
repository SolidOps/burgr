using Microsoft.Extensions.DependencyInjection;
using SolidOps.UM.Shared.Domain.Configuration;
using SolidOps.UM.Shared.Domain.CrossCutting;
using SolidOps.UM.Shared.Domain.Entities;

namespace MetaCorp.Template.Domain;

public static class AssemblyReference
{
}

public partial class TemplateDomainServiceRegistrar : IServiceRegistrar
{
    public int Priority => 1;
    public void ConfigureServices(IServiceCollection services, IExtendedConfiguration configuration)
    {
        #region foreach MODEL[EN][AG]
        services.AddTransient<IEntityRules<_IDENTITY_KEY_TYPE_, Domain._DOMAINTYPE_.CLASSNAME>, Domain._DOMAINTYPE_.Rules.CLASSNAMERules>();
        #endregion foreach MODEL

        ConfigureAdditionalServices(services, configuration);
    }

    partial void ConfigureAdditionalServices(IServiceCollection services, IExtendedConfiguration configuration);
}
