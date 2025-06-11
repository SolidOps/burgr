using SolidOps.UM.Shared.Presentation;

namespace SolidOps.UM.API;

public class Program
{
    public async static Task Main(string[] args)
    {
        var host = CreateHostBuilder(args).Build();
        await host.InitAsync();
        await host.RunAsync();
    }

    public static IHostBuilder CreateHostBuilder(string[] args)
    {
        return new CoreHostBuilder().CreateHostBuilder<UMAPIStartup>("um", args);
    }
}
