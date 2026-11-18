using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;

namespace SolidOps.UM.Tests;

/// <summary>
/// Runs the API of Program.cs in memory, on a SQLite database of its own per test. Program.cs
/// must read the connection string "UM" and use SQLite for it. Override
/// <see cref="ConfigureWebHost"/> to replace services (authentication, clock...).
/// </summary>
public abstract class ApiTestBase
{
    private string databasePath = null!;

    /// <summary>An Id no aggregate root or entity has.</summary>
    protected static readonly Guid UnknownId = default;

    protected WebApplicationFactory<Program> Factory { get; private set; } = null!;

    protected HttpClient Client { get; private set; } = null!;

    [TestInitialize]
    public void StartApi()
    {
        databasePath = Path.Combine(Path.GetTempPath(), $"um-{Guid.NewGuid():N}.db");
        Factory = new WebApplicationFactory<Program>().WithWebHostBuilder(builder =>
        {
            builder.UseSetting("ConnectionStrings:UM", $"Data Source={databasePath};Pooling=False");
            ConfigureWebHost(builder);
        });
        Client = Factory.CreateClient();
    }

    [TestCleanup]
    public void StopApi()
    {
        Client.Dispose();
        Factory.Dispose();
        File.Delete(databasePath);
    }

    protected virtual void ConfigureWebHost(Microsoft.AspNetCore.Hosting.IWebHostBuilder builder)
    {
    }

    /// <summary>POSTs <paramref name="body"/>, asserts 201 and returns the created value.</summary>
    protected async Task<T> PostAsync<T>(string uri, object body)
    {
        var response = await Client.PostAsJsonAsync(uri, body);
        Assert.AreEqual(HttpStatusCode.Created, response.StatusCode, await response.Content.ReadAsStringAsync());
        return (await response.Content.ReadFromJsonAsync<T>())!;
    }

    /// <summary>Asserts a 400 whose validation errors name <paramref name="property"/>.</summary>
    protected static async Task AssertInvalidAsync(HttpResponseMessage response, string property)
    {
        Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        var problem = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>();
        Assert.IsTrue(problem!.Errors.ContainsKey(property), $"no error for {property}: {string.Join(", ", problem.Errors.Keys)}");
    }
}
