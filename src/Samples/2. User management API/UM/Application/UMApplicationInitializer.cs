using Microsoft.Extensions.DependencyInjection;
using SolidOps.UM.Shared.Domain.Configuration;
using SolidOps.UM.Shared.Domain.UnitOfWork;
using SolidOps.UM.Domain.AggregateRoots;
using SolidOps.UM.Domain.Entities;
using SolidOps.UM.Domain.Repositories;
using SolidOps.UM.Domain.Services;
using SolidOps.UM.Infrastructure.Repositories;

namespace SolidOps.UM.Application;

public partial class UMApplicationInitializer
{
    private IUserRepository userRepository;
    private ILocalUserRepository localUserRepository;
    private IExecutionContext contextService;
    private IExtendedConfiguration configuration;
    private IRightRepository rightRepository;
    private IUserRightRepository userRightRepository;

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
        contextService = serviceProvider.GetRequiredService<IExecutionContext>();
        configuration = serviceProvider.GetRequiredService<IExtendedConfiguration>();
        rightRepository = serviceProvider.GetRequiredService<IRightRepository>();
        userRightRepository = serviceProvider.GetRequiredService<IUserRightRepository>();

        this.contextService.SetUserId(() =>
        {
            return ("", new List<string>() { IExecutionContext.WILDCARD }, null);
        });

        using (var unitOfWork = contextService.StartUnitOfWork("UM", "Init", UnitOfWorkType.Write))
        {
            await CheckAndCreate(IExecutionContext.ROOTUSER, new List<string>() { IExecutionContext.ALLRIGHT });

            var technicalUsers = configuration.TechnicalUsers;

            if (technicalUsers != null && technicalUsers.Count > 0)
            {
                foreach (var techUser in technicalUsers)
                {
                    await CheckAndCreate(techUser.Key, new List<string>() { IExecutionContext.ALLRIGHT });
                }
            }

            unitOfWork.Complete();
        }
    }

    private async Task CheckAndCreate(string userName, List<string> rights)
    {
        if (await Verify(userName))
        {
            User user = User.Create(userName, typeof(LocalIdentityProviderService).Name, true);
            var addUserRes = await userRepository.Add(user);
            if (addUserRes.HasError)
                throw new Exception("an error occured");

            foreach (var rightName in rights)
            {
                var right = await rightRepository.GetSingleByName(rightName);
                if (right == null)
                {
                    right = Right.Create(rightName);
                    var addRightRes = await rightRepository.Add(right);
                    if (addRightRes.HasError)
                        throw new Exception("an error occured");
                }
                var addUrRes = await userRightRepository.Add(UserRight.Create(user.Id, right.Id));
                if (addUrRes.HasError)
                    throw new Exception("an error occured");
            }

            LocalUser localUser = LocalUser.Create(userName, string.Empty);
            await localUserRepository.Add(localUser);
        }
    }
}
