using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Text;

namespace SolidOps.UM.Shared.Presentation;

public abstract class CoreJwtStartup : CoreStartup
{
    public CoreJwtStartup(IConfiguration configuration, string appName) : base(configuration, appName)
    {

    }

    protected override void AddAuthorization(IApplicationBuilder app)
    {
        base.AddAuthorization(app);
        app.UseAuthorization();
    }

    protected override void AddAuthorizationOnControllers(MvcOptions options)
    {
        base.AddAuthorizationOnControllers(options);
        options.Filters.Add(new AuthorizeFilter(GetAuthorizeDefaultPolicy()));
    }

    private static AuthorizationPolicy GetAuthorizeDefaultPolicy()
    {
        var defaultPolicyBuilder = new AuthorizationPolicyBuilder();
        defaultPolicyBuilder.AuthenticationSchemes.Add(JwtBearerDefaults.AuthenticationScheme);
        defaultPolicyBuilder.RequireAuthenticatedUser();
        return defaultPolicyBuilder.Build();
    }

    protected override void ConfigureAuthentication(IServiceCollection services)
    {
        if (this._configuration["Jwt:Key"] == "remote")
        {
            services.AddAuthentication("Bearer")
                .AddScheme<RemoteAuthenticationOptions, RemoteAuthenticationHandler>("Bearer", null);
        }
        else
        {
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(x =>
                {
                    x.RequireHttpsMetadata = false;
                    x.SaveToken = true;
                    x.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"])),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                });
        }
    }

    protected override void ConfigureSwagger(SwaggerGenOptions options)
    {
        base.ConfigureSwagger(options);

        var _bearerSchemeId = "Bearer";
        options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Description = @"JWT Authorization header using the Bearer scheme. Enter 'Bearer' [space] and then your token in the text input below. Example: 'Bearer 12345abcdef'",
            Name = "Authorization",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.ApiKey,
            Scheme = "Bearer"
        });
        options.OperationFilter<SecurityRequirementsOperationFilter>(_bearerSchemeId);
    }
}
