using Microsoft.VisualStudio.TestTools.UnitTesting;
using SolidOps.Burgr.Shared.Tests;
using SolidOps.UM.Contracts.DTO;
using SolidOps.UM.Tests.Endpoints;

namespace SolidOps.UM.Tests;

[TestClass]
public class ApplicationTests : BaseAppServiceTest<UMTestSetup>
{
    [TestMethod]
    public async Task TestThatOnlyRootCanCreateApplicationAndEnvironment()
    {
        var testEnv = await TestSetup.RootClient.UMAPI_AddEnvironmentAndGet(new EnvironmentWriteDTO()
        {
            Name = UMTestSetup.Environment.TEST,
            ConfigurationContent = "{ \"common\": \"env-value\", \"env-key\": true, \"sub_env\": { \"akey\": \"a_value\" } }"
        });
        Assert.IsNotNull(testEnv);

        var application = await TestSetup.RootClient.UMAPI_AddApplicationAndGet(new ApplicationWriteDTO()
        {
            Name = UMTestSetup.Application.INFINITY,
            ConfigurationContent = "{ \"common\": \"app-value\", \"app-key\": false, \"sub_env\": { \"anotherkey\": \"anothervalue\" }, \"sub_app\": { \"appkey\": \"appvalue\" } }",
            ApplicationEnvironments = new List<ApplicationEnvironmentWriteDTO>()
            {
                new ApplicationEnvironmentWriteDTO()
                {
                    EnvironmentId = testEnv.Id,
                    ConfigurationContent = "{ \"common\": \"app-env-value\", \"app-env-key\": 5, \"sub_env\": { \"anotherkey\": \"yetanothervalue\" }, \"sub_app\": { \"appkey\": \"appvalue\" }, \"sub_appenv\": { \"appenvkey\": \"appenvvalue\" } }",
                }
            }
        });
        Assert.IsNotNull(application);

        var conf = await TestSetup.RootClient.UMFacade_Configurations_GetConfiguration(UMTestSetup.Application.INFINITY, UMTestSetup.Environment.TEST);
        Assert.AreEqual("{\"common\":\"app-env-value\",\"env-key\":\"True\",\"sub_env:akey\":\"a_value\",\"app-key\":\"False\",\"sub_env:anotherkey\":\"yetanothervalue\",\"sub_app:appkey\":\"appvalue\",\"app-env-key\":\"5\",\"sub_appenv:appenvkey\":\"appenvvalue\"}", conf);
    }
}
