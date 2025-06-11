using Microsoft.Extensions.DependencyInjection;
using SolidOps.UM.Shared.Domain.Configuration;
using SolidOps.UM.Shared.Domain.CrossCutting;
using SolidOps.UM.Shared.Domain.Entities;
namespace SolidOps.UM.Domain;
public static class AssemblyReference
{
}
public partial class UMDomainServiceRegistrar : IServiceRegistrar
{
    public int Priority => 1;
    public void ConfigureServices(IServiceCollection services, IExtendedConfiguration configuration)
    {
        // Object [EN][AG]
        services.AddTransient<IEntityRules<Guid, Domain.AggregateRoots.LocalUser>, Domain.AggregateRoots.Rules.LocalUserRules>();

        services.AddTransient<IEntityRules<Guid, Domain.AggregateRoots.User>, Domain.AggregateRoots.Rules.UserRules>();

        services.AddTransient<IEntityRules<Guid, Domain.Entities.UserRight>, Domain.Entities.Rules.UserRightRules>();

        services.AddTransient<IEntityRules<Guid, Domain.Entities.Right>, Domain.Entities.Rules.RightRules>();

        services.AddTransient<IEntityRules<Guid, Domain.AggregateRoots.Invite>, Domain.AggregateRoots.Rules.InviteRules>();

        ConfigureAdditionalServices(services, configuration);
    }
    partial void ConfigureAdditionalServices(IServiceCollection services, IExtendedConfiguration configuration);
}