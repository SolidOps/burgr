using SolidOps.UM.Shared.Contracts.Endpoints;
using SolidOps.UM.Shared.Domain.UnitOfWork;
using SolidOps.UM.Shared.Tests;
using SolidOps.UM.Contracts;
using SolidOps.UM.Contracts.DTO;
using SolidOps.UM.Tests.Endpoints;

namespace SolidOps.UM.Tests;

public class BaseUMTestSetup : TestSetup
{
    public const string AUTHORIZATION_HEADER = "Authorization";
    public const string UM = "UM";

    public Dictionary<string, string> PasswordPerUser { get; private set; }

    protected string AuthenticationHostName { get; set; }
    public bool DisablePasswordChange { get; set; }

    public UserTestInstance RootTestInstance;
    public AppServiceClient RootClient;

    public override void BeforeStart()
    {
        base.BeforeStart();

        PasswordPerUser = new Dictionary<string, string>();
        PasswordPerUser.Add(IExecutionContext.ROOTUSER, Guid.NewGuid().ToString());
    }

    public override async Task Start()
    {
        await base.Start();

        if (!DisablePasswordChange)
        {
            await UpdateRootPassword();
            RootTestInstance = await NewUserTestInstance(IExecutionContext.ROOTUSER, AuthenticationHostName);
            RootClient = RootTestInstance.PickClient(AuthenticationHostName);
        }
    }

    public async Task UpdateRootPassword()
    {
        bool needRootPasswordUpdate = false;

        if (DBState == (int)DBStateEnum.Empty)
        {
            needRootPasswordUpdate = true;
        }
        else
        {
            using (var userTestInstance = await NewUserTestInstance(string.Empty, AuthenticationHostName))
            {
                using (var client = userTestInstance.PickClient(AuthenticationHostName))
                {
                    needRootPasswordUpdate = await client.UMFacade_ServerStatus_NeedTechUserPasswordUpdate(IExecutionContext.ROOTUSER);
                }
            }
        }

        if (needRootPasswordUpdate)
        {
            using (var userTestInstance = await NewUserTestInstance(string.Empty, AuthenticationHostName))
            {
                using (var client = userTestInstance.PickClient(AuthenticationHostName))
                {
                    await client.UMFacade_Authentication_SetInitialPassword(IExecutionContext.ROOTUSER, PasswordPerUser[IExecutionContext.ROOTUSER]);
                }
            }
        }
    }

    public override sealed void RegisterAppService(IAppServiceInfo startServiceInfo)
    {
        base.RegisterAppService(startServiceInfo);
        if (startServiceInfo.IsUsedForAuthentication)
        {
            AuthenticationHostName = startServiceInfo.ServiceName;
        }
    }

    public override async Task<UserTestInstance> NewUserTestInstance(string userName, params string[] knownHosts)
    {
        var client = await base.NewUserTestInstance(userName, knownHosts);

        if (!string.IsNullOrEmpty(userName))
        {
            string host = AuthenticationHostName;
            //if (knownHosts.Length > 0)
            //    host = knownHosts[0];
            await client.PickClient(host).Login(userName, PasswordPerUser[userName]);
            SyncAuthorizations(client);
        }

        return client;
    }

    public void SyncAuthorizations(UserTestInstance userTestInstance)
    {
        IEnumerable<string> values;
        if (userTestInstance.PickClient(AuthenticationHostName).HttpClient.DefaultRequestHeaders.TryGetValues(AUTHORIZATION_HEADER, out values))
        {
            foreach (var kvp in userTestInstance.Clients)
            {
                if (kvp.Key != AuthenticationHostName)
                {
                    foreach (var client in kvp.Value)
                    {
                        client.HttpClient.DefaultRequestHeaders.Remove(AUTHORIZATION_HEADER);
                        client.HttpClient.DefaultRequestHeaders.Add(AUTHORIZATION_HEADER, values.First());
                    }
                }
            }
        }
    }

    public void SyncAuthorizationsWithToken(UserTestInstance client, string token)
    {
        client.PickClient(AuthenticationHostName).HttpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
        SyncAuthorizations(client);
    }

    public void SyncAuthorizations(UserTestInstance client, string authorization)
    {
        client.PickClient(AuthenticationHostName).HttpClient.DefaultRequestHeaders.Add("Authorization", authorization);
        SyncAuthorizations(client);
    }

    //public async Task<string> CreateOrganizationWithRoot(string name)
    //{
    //    using (var userTestInstance = await NewUserTestInstance(IExecutionScope.ROOTUSER, AuthenticationHostName))
    //    {
    //        var client = userTestInstance.PickClient(AuthenticationHostName);

    //        return await client.UMAPI_AddOrganization(new OrganizationWriteDTO() { Name = name });
    //    }
    //}

    public async Task<string> CreateUserWithRoot(string name, string roleName = null, List<UserRightDTO> rights = null)
    {
        using (var userTestInstance = await NewUserTestInstance(IExecutionContext.ROOTUSER, AuthenticationHostName))
        {
            var client = userTestInstance.PickClient(AuthenticationHostName);

            var password = Guid.NewGuid().ToString();
            var userId = await client.UMFacade_UserCreation_CreateUser(new UserCreationInfoDTO()
            {
                UserEmail = name,
                Password = password,
                Rights = rights
            });

            PasswordPerUser.Add(name, password);

            return userId;
        }
    }

    public override async Task Cleanup()
    {
        if (RootClient != null)
            RootClient.Dispose();
        if (RootTestInstance != null)
            RootTestInstance.Dispose();

        await base.Cleanup();

        await Task.Delay(1000);
    }
}
