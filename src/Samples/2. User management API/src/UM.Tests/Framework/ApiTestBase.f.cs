using System.Net;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text.Encodings.Web;
using SolidOps.UM.Application;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace SolidOps.UM.Tests;

/// <summary>
/// Runs the API of Program.cs in memory, on a SQLite database of its own per test. Program.cs
/// must read the connection string "UM" and use SQLite for it.
/// The client is authenticated as a test user holding every right; <see cref="AuthenticateAs"/>
/// changes its rights, <see cref="Unauthenticate"/> makes it anonymous. Override
/// <see cref="UseTestAuthentication"/> to test the authentication of Program.cs instead, and
/// <see cref="ConfigureWebHost"/> to replace services.
/// </summary>
public abstract class ApiTestBase
{
    private const string TestScheme = "Test";
    private const string UserHeader = "X-Test-User";
    private const string RightsHeader = "X-Test-Rights";
    private string databasePath = null!;

    /// <summary>An Id no aggregate root or entity has.</summary>
    protected static readonly Guid UnknownId = default;

    protected WebApplicationFactory<Program> Factory { get; private set; } = null!;

    protected HttpClient Client { get; private set; } = null!;

    protected virtual bool UseTestAuthentication => true;

    [TestInitialize]
    public void StartApi()
    {
        databasePath = Path.Combine(Path.GetTempPath(), $"um-{Guid.NewGuid():N}.db");
        Factory = new WebApplicationFactory<Program>().WithWebHostBuilder(builder =>
        {
            builder.UseSetting("ConnectionStrings:UM", $"Data Source={databasePath};Pooling=False");
            if (UseTestAuthentication)
            {
                builder.ConfigureTestServices(services => services
                    .AddAuthentication(TestScheme)
                    .AddScheme<AuthenticationSchemeOptions, TestAuthenticationHandler>(TestScheme, null));
            }
            ConfigureWebHost(builder);
        });
        Client = Factory.CreateClient();
        AuthenticateAs(ICurrentUser.AllRights);
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

    /// <summary>The next requests come from a test user holding <paramref name="rights"/> only.</summary>
    protected void AuthenticateAs(params string[] rights)
    {
        Unauthenticate();
        Client.DefaultRequestHeaders.Add(UserHeader, "test-user");
        if (rights.Length > 0)
        {
            Client.DefaultRequestHeaders.Add(RightsHeader, string.Join(",", rights));
        }
    }

    /// <summary>The next requests are anonymous.</summary>
    protected void Unauthenticate()
    {
        Client.DefaultRequestHeaders.Remove(UserHeader);
        Client.DefaultRequestHeaders.Remove(RightsHeader);
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

    // authenticated as the user header says, with one right claim per right of the rights header
    private sealed class TestAuthenticationHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder)
        : AuthenticationHandler<AuthenticationSchemeOptions>(options, logger, encoder)
    {
        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.TryGetValue(UserHeader, out var user))
            {
                return Task.FromResult(AuthenticateResult.NoResult());
            }

            var claims = Request.Headers[RightsHeader].ToString()
                .Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(right => new Claim(ICurrentUser.RightClaimType, right))
                .Prepend(new Claim(ClaimTypes.Name, user.ToString()));
            var principal = new ClaimsPrincipal(new ClaimsIdentity(claims, TestScheme));
            return Task.FromResult(AuthenticateResult.Success(new AuthenticationTicket(principal, TestScheme)));
        }
    }
}
