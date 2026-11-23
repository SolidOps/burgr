using Acme.Library.Api;
using Acme.Library.Infrastructure;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<LibraryDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("Library") ?? "Data Source=library.db"));
builder.Services.AddOpenApi();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    scope.ServiceProvider.GetRequiredService<LibraryDbContext>().Database.EnsureCreated();
}

// The OpenAPI document under /openapi/v1.json, browsed under /swagger. In every
// environment, to try the API; put it under app.Environment.IsDevelopment() before deploying.
app.MapOpenApi();
app.UseSwaggerUI(options => options.SwaggerEndpoint("/openapi/v1.json", "Library"));

app.MapLibraryEndpoints();

app.Run();

public partial class Program;
