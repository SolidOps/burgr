using SolidOps.UM.Shared.Presentation;

namespace SolidOps.UM.API;

public class UMAPIStartup : CoreJwtStartup
{
    public override string APIName => "UM";

    public UMAPIStartup(IConfiguration configuration) : base(configuration, "UM")
    {
        Dependencies.Add(typeof(SolidOps.UM.Contracts.AssemblyReference).Assembly);
        Dependencies.Add(typeof(SolidOps.UM.AssemblyReference).Assembly);
    }
}
