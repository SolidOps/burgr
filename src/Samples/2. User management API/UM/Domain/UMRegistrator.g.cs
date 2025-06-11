using Microsoft.Extensions.DependencyInjection;
using SolidOps.Burgr.Shared.Domain.Configuration;
using SolidOps.Burgr.Shared.Domain.CrossCutting;
using SolidOps.Burgr.Shared.Domain.Entities;
namespace SolidOps.UM.Domain;
public static class AssemblyReference
{
}
public partial class UMDomainServiceRegistrator : IServiceRegistrator
{
    public int Priority => 1;
    public void ConfigureServices(IServiceCollection services, IExtendedConfiguration configuration)
    {
        // Object [EN][AG]
        services.AddTransient<IEntityRules<Guid, Domain.AggregateRoots.Application>, Domain.AggregateRoots.Rules.ApplicationRules>();

        services.AddTransient<IEntityRules<Guid, Domain.Entities.ApplicationEnvironment>, Domain.Entities.Rules.ApplicationEnvironmentRules>();

        services.AddTransient<IEntityRules<Guid, Domain.AggregateRoots.Environment>, Domain.AggregateRoots.Rules.EnvironmentRules>();

        services.AddTransient<IEntityRules<Guid, Domain.AggregateRoots.LocalUser>, Domain.AggregateRoots.Rules.LocalUserRules>();

        services.AddTransient<IEntityRules<Guid, Domain.AggregateRoots.User>, Domain.AggregateRoots.Rules.UserRules>();

        services.AddTransient<IEntityRules<Guid, Domain.Entities.UserRole>, Domain.Entities.Rules.UserRoleRules>();

        services.AddTransient<IEntityRules<Guid, Domain.Entities.Role>, Domain.Entities.Rules.RoleRules>();

        services.AddTransient<IEntityRules<Guid, Domain.AggregateRoots.Organization>, Domain.AggregateRoots.Rules.OrganizationRules>();

        services.AddTransient<IEntityRules<Guid, Domain.Entities.UserOrganization>, Domain.Entities.Rules.UserOrganizationRules>();

        services.AddTransient<IEntityRules<Guid, Domain.AggregateRoots.Invite>, Domain.AggregateRoots.Rules.InviteRules>();

        ConfigureAdditionalServices(services, configuration);
    }
    partial void ConfigureAdditionalServices(IServiceCollection services, IExtendedConfiguration configuration);
}