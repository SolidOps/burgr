using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using SolidOps.UM.Api;
using SolidOps.UM.Application;
using SolidOps.UM.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

var key = JwtTokenIssuer.KeyFrom(builder.Configuration);
builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options => options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidIssuer = JwtTokenIssuer.Issuer,
        ValidAudience = JwtTokenIssuer.Issuer,
        IssuerSigningKey = key,
        NameClaimType = JwtRegisteredClaimNames.Name,
    });
builder.Services.AddSingleton<ITokenIssuer>(new JwtTokenIssuer(key));

builder.Services.AddUMApi();
builder.Services.AddUMApplication();
builder.Services.AddUMInfrastructure(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("UM") ?? "Data Source=um.db"));

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    scope.ServiceProvider.GetRequiredService<UMDbContext>().Database.EnsureCreated();
}

app.MapUMEndpoints();

app.Run();
