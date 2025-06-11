using Microsoft.Extensions.DependencyInjection;
using SolidOps.Burgr.Shared.Domain.Configuration;
using SolidOps.Burgr.Shared.Domain.UnitOfWork;
using SolidOps.UM.Domain.AggregateRoots;
using SolidOps.UM.Domain.Repositories;
using SolidOps.UM.Domain.Services;
using SolidOps.UM.Domain.ValueObjects;

namespace SolidOps.UM.Application;

public partial class UMApplicationInitializer
{
    private IUserRepository userRepository;
    private ILocalUserRepository localUserRepository;
    private IExecutionScope contextService;
    private IExtendedConfiguration configuration;

    private async Task<bool> Verify(string email)
    {
        // search for Admin
        var user = await userRepository.GetSingleByEmail(email);
        return user == null;
    }

    protected override async Task AdditionalInitialization(IServiceProvider serviceProvider, CancellationToken cancellationToken)
    {
        userRepository = serviceProvider.GetRequiredService<IUserRepository>();
        localUserRepository = serviceProvider.GetRequiredService<ILocalUserRepository>();
        contextService = serviceProvider.GetRequiredService<IExecutionScope>();
        configuration = serviceProvider.GetRequiredService<IExtendedConfiguration>();

        this.contextService.SetUserId(() =>
        {
            return ("", new List<string>() { IExecutionScope.WILDCARD }, new List<string>(), null);
        });

        using (var unitOfWork = contextService.StartUnitOfWork("UM", "Init", UnitOfWorkType.Write))
        {
            await CheckAndCreate(IExecutionScope.ROOTUSER, new List<(string app, string right)>() { (IExecutionScope.ALLAPP, IExecutionScope.ALLRIGHT) });

            var technicalUsers = configuration.TechnicalUsers;

            if (technicalUsers != null && technicalUsers.Count > 0)
            {
                foreach (var techUser in technicalUsers)
                {
                    await CheckAndCreate(techUser.Key, new List<(string app, string right)>() { (IExecutionScope.ALLAPP, IExecutionScope.ALLRIGHT) });
                }
            }

            unitOfWork.Complete();
        }
    }

    private async Task CheckAndCreate(string userName, List<(string app, string right)> applicationRights)
    {
        if (await Verify(userName))
        {
            User user = User.Create(userName, typeof(LocalIdentityProviderService).Name, true);

            var appRights = new List<ApplicationRight>();
            foreach (var right in applicationRights)
                appRights.Add(ApplicationRight.Create(right.app, right.right));
            user.ApplicationRights = appRights;
            await userRepository.Add(user);

            LocalUser localUser = LocalUser.Create(userName, string.Empty);
            await localUserRepository.Add(localUser);
        }
    }
}
