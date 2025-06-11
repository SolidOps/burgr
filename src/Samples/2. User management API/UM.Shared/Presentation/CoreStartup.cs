using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Serilog;
using SolidOps.UM.Shared.Application;
using SolidOps.UM.Shared.Domain.Configuration;
using SolidOps.UM.Shared.Domain.CrossCutting;
using SolidOps.UM.Shared.Domain.UnitOfWork;
using SolidOps.UM.Shared.Infrastructure;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SolidOps.UM.Shared.Presentation;

public abstract class CoreStartup
{
    public readonly IConfiguration _configuration;

    public abstract string APIName
    {
        get;
    }

    protected List<Assembly> Dependencies { get; set; }

    public List<string> Subscriptions { get; set; }

    private string _appName { get; set; }

    private ExtendedConfiguration _burgrConfig { get; set; }

    public CoreStartup(IConfiguration configuration, string appName)
    {
        _configuration = configuration;
        Dependencies = [typeof(SolidOps.UM.Shared.Presentation.AssemblyReference).Assembly];
        Subscriptions = new List<string>();
        _appName = appName;
    }

    // This method gets called by the runtime. Use this method to add services to the container.
    public virtual void ConfigureServices(IServiceCollection services)
    {
        _burgrConfig = new ExtendedConfiguration(_configuration);
        services.AddSingleton<IExtendedConfiguration>(_burgrConfig);

        services
            .AddControllers(options =>
            {
                options.Conventions.Add(new ApplicationDescription(Dependencies));
                options.Conventions.Add(new RouteTokenTransformerConvention(new SlugifyParameterTransformer()));
                AddAuthorizationOnControllers(options);
            })
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
            });

        services.AddOptions();

        ConfigureAuthentication(services);

        services.AddSwaggerGen(ConfigureSwagger);

        services.AddScoped<IExecutionContext, BurgrExecutionContext>();

        services.AddAsyncInitializer<DBInitializer>();

        RegisterModules(services, _burgrConfig);

        services.AddAsyncInitializer<BurgrInitializer>();

        //enable CORS
        if (!string.IsNullOrEmpty(_burgrConfig.BurgrConfiguration.Origins))
        {
            services.AddCors(options =>
            {
                options.AddPolicy(name: APIName,
                    builder =>
                    {
                        builder.WithOrigins(_burgrConfig.BurgrConfiguration.Origins.Split("|"))
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials()
                        .WithExposedHeaders("Authorization", "Location");
                    });
            });
        }
    }
    private void RegisterModules(IServiceCollection services, IExtendedConfiguration configuration)
    {
        var registratorType = typeof(IServiceRegistrar);
        var registrationTypes = new List<Type>();

        foreach (var assembly in Dependencies)
        {
            registrationTypes.AddRange(assembly.GetTypes());
        }

        var serviceRegistratorTypes = registrationTypes
                    .Where(t => registratorType.IsAssignableFrom(t) && !t.IsInterface)
                    .ToList();

        List<IServiceRegistrar> registrators = new List<IServiceRegistrar>();
        serviceRegistratorTypes.ForEach(p =>
        {
            registrators.Add((IServiceRegistrar)Activator.CreateInstance(p));
        });

        foreach (var registrator in registrators.OrderBy(r => r.Priority))
        {
            registrator.ConfigureServices(services, configuration);
        }
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public virtual void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (!string.IsNullOrEmpty(_burgrConfig.BurgrConfiguration.Origins))
        {
            // enable CORS
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });
            app.UseCors(APIName);
        }

        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        var configuration = app.ApplicationServices.GetService<IExtendedConfiguration>();
        if (configuration != null && configuration.BurgrConfiguration != null && configuration.BurgrConfiguration.EnableSwagger)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint($"/swagger/{APIName}/swagger.json", APIName);
                c.RoutePrefix = string.Empty;
            });
        }

        app.UseMiddleware<GlobalExceptionMiddleware>();
        app.UseMiddleware<DiagnosticMiddleware>();

        app.UseSerilogRequestLogging();

        app.UseRouting();

        AddAuthorization(app);

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });

        var serverAddressesFeature = app.ServerFeatures.Get<IServerAddressesFeature>();
        var logger = app.ApplicationServices.GetRequiredService<ILogger<CoreStartup>>();
        logger.LogInformation("Listening on the following addresses: " + string.Join(", ", serverAddressesFeature.Addresses));
    }

    protected virtual void AddAuthorization(IApplicationBuilder app)
    {

    }

    protected virtual void ConfigureAuthentication(IServiceCollection services)
    {

    }

    protected virtual void ConfigureSwagger(SwaggerGenOptions options)
    {
        options.SwaggerDoc(APIName, new OpenApiInfo { Title = APIName });
    }

    protected virtual void AddAuthorizationOnControllers(MvcOptions options)
    {

    }
}
