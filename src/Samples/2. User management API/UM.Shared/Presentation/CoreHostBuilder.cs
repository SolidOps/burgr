using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using System.Reflection;

namespace SolidOps.UM.Shared.Presentation;

public class CoreHostBuilder
{
    public string AppName { get; set; }
    public IConfiguration Configuration
    {
        get
        {
            var directory = Directory.GetCurrentDirectory();
            var conf = new ConfigurationBuilder().SetBasePath(directory);
            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            List<FileInfo> files = new List<FileInfo>();
            files.AddRange(new DirectoryInfo(directory).GetFiles("*.appsettings.json"));
            if(!string.IsNullOrEmpty(env))
                files.AddRange(new DirectoryInfo(directory).GetFiles("*.appsettings." + env + ".json"));
            foreach (var file in files)
            {
                if (file.Name.StartsWith(AppName + "."))
                {
                    conf = conf.AddJsonFile(file.Name, optional: false, reloadOnChange: true);
                }
            }

            return conf.Build();
        }
    }

    public IHostBuilder CreateHostBuilder<TStartup>(string appName, string[] args)
        where TStartup : class
    {
        AppName = appName;
        Serilog.Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(Configuration)
            .Enrich.With(new ThreadIdEnricher())
            .CreateLogger();

        var host = Host.CreateDefaultBuilder()
            .ConfigureAppConfiguration(options =>
            {
                var settingsFiles = GetSettingsFiles();
                settingsFiles.ForEach(f => options.AddJsonFile(f, optional: false, reloadOnChange: true));
            })
            .UseSerilog()
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<TStartup>();
            });

        return host;
    }

    private List<string> GetSettingsFiles()
    {
        List<FileInfo> files = new List<FileInfo>();
        var currentDirectory = new DirectoryInfo(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));
        files.AddRange(currentDirectory.GetFiles(AppName + ".appsettings.json"));
        var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        if (!string.IsNullOrEmpty(env))
            files.AddRange(currentDirectory.GetFiles(AppName + ".appsettings." + env + ".json"));
            
        return files.Select(f => f.FullName).ToList();
    }
}

class ThreadIdEnricher : ILogEventEnricher
{
    public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
    {
        logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty(
                "ThreadId", Thread.CurrentThread.ManagedThreadId));
    }
}
