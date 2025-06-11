using SolidOps.UM.Shared.Tests;
using SolidOps.UM.API;

namespace SolidOps.UM.Tests;

public class UMTestSetup : BaseUMTestSetup
{
    public class Organization
    {
        public const string DUNDER_MIFFLIN = "Dunder Mifflin";
        public const string DUNDER_MIFFLIN_NAMESPACE = "@dunder-mifflin.com";
    }

    public class User
    {
        // users
        public const string PAM = "pam" + Organization.DUNDER_MIFFLIN_NAMESPACE;
        public const string JIM = "jim" + Organization.DUNDER_MIFFLIN_NAMESPACE;
        public const string DWIGHT = "dwight" + Organization.DUNDER_MIFFLIN_NAMESPACE;
        public const string MICHAEL = "michael" + Organization.DUNDER_MIFFLIN_NAMESPACE;
        public const string ANGELA = "angela" + Organization.DUNDER_MIFFLIN_NAMESPACE;
        public const string ANDREW = "andrew" + Organization.DUNDER_MIFFLIN_NAMESPACE;
        public const string KEVIN = "kevin" + Organization.DUNDER_MIFFLIN_NAMESPACE;
    }

    public class Application
    {
        // application
        public const string INFINITY = "Dunder Mifflin Infinity";
        public const string WUPHF = "WUPHF.com";
    }

    public class Environment
    {
        // environment
        public const string DEV = "Dev";
        public const string TEST = "Test";
        public const string PROD = "Prod";
    }

    public override void BeforeStart()
    {
        base.BeforeStart();

        RegisterAppService(new AppServiceInfo<UMAPIStartup>()
        {
            ServiceName = UM,
            IsUsedForAuthentication = true
        });
    }
}
