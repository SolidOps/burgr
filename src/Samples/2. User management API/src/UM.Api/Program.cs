using Microsoft.EntityFrameworkCore;
using SolidOps.UM.Api;
using SolidOps.UM.Application;
using SolidOps.UM.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

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
