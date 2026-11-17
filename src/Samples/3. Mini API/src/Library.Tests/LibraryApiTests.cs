using System.Net;
using System.Net.Http.Json;
using Acme.Library.Contracts;
using Acme.Library.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Acme.Library.Tests;

[TestClass]
public sealed class LibraryApiTests
{
    private string databasePath = null!;
    private WebApplicationFactory<Program> factory = null!;
    private HttpClient client = null!;

    [TestInitialize]
    public void Initialize()
    {
        databasePath = Path.Combine(Path.GetTempPath(), $"library-{Guid.NewGuid():N}.db");
        factory = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder => builder.UseSetting("ConnectionStrings:Library", $"Data Source={databasePath};Pooling=False"));
        client = factory.CreateClient();
    }

    [TestCleanup]
    public void Cleanup()
    {
        client.Dispose();
        factory.Dispose();
        File.Delete(databasePath);
    }

    [TestMethod]
    public async Task AuthorGoesThroughTheWholeCrud()
    {
        var created = await PostAsync<AuthorDto>("/authors", new AuthorInput { Name = "Ursula K. Le Guin" });

        var updated = await client.PutAsJsonAsync($"/authors/{created.Id}", new AuthorInput { Name = "Ursula Le Guin", Biography = "Earthsea" });
        Assert.AreEqual(HttpStatusCode.OK, updated.StatusCode);

        var read = await client.GetFromJsonAsync<AuthorDto>($"/authors/{created.Id}");
        Assert.AreEqual("Ursula Le Guin", read!.Name);
        Assert.AreEqual("Earthsea", read.Biography);

        var list = await client.GetFromJsonAsync<List<AuthorDto>>("/authors");
        Assert.HasCount(1, list!);

        Assert.AreEqual(HttpStatusCode.NoContent, (await client.DeleteAsync($"/authors/{created.Id}")).StatusCode);
        Assert.AreEqual(HttpStatusCode.NotFound, (await client.GetAsync($"/authors/{created.Id}")).StatusCode);
    }

    [TestMethod]
    public async Task AuthorIsReadWithItsBooksAndTheirCalculatedProperty()
    {
        var author = await PostAsync<AuthorDto>("/authors", new AuthorInput { Name = "Frank Herbert" });
        await PostAsync<BookDto>("/books", NewBook(author.Id, "978-0441013593", BookStatus.Published, copies: 2));
        await PostAsync<BookDto>("/books", NewBook(author.Id, "978-0441172719", BookStatus.OutOfPrint, copies: 2));

        var read = await client.GetFromJsonAsync<AuthorDto>($"/authors/{author.Id}");

        Assert.HasCount(2, read!.Books);
        Assert.AreEqual(1, read.Books.Count(b => b.IsAvailable));
    }

    [TestMethod]
    public async Task BookOfAnUnknownAuthorIsInvalid()
    {
        var response = await client.PostAsJsonAsync("/books", NewBook(Guid.NewGuid(), "978-0000000001", BookStatus.Draft, copies: 0));

        await AssertInvalidAsync(response, nameof(BookInput.AuthorId));
    }

    [TestMethod]
    public async Task HandWrittenValidationRuns()
    {
        var author = await PostAsync<AuthorDto>("/authors", new AuthorInput { Name = "Iain Banks" });

        var response = await client.PostAsJsonAsync("/books", NewBook(author.Id, "978-0000000002", BookStatus.Draft, copies: -1));

        await AssertInvalidAsync(response, nameof(BookInput.Copies));
    }

    [TestMethod]
    public async Task GeneratedValidationRuns()
    {
        var response = await client.PostAsJsonAsync("/authors", new AuthorInput { Name = new string('a', 101) });

        await AssertInvalidAsync(response, nameof(AuthorInput.Name));
    }

    [TestMethod]
    public async Task DuplicateUniquePropertyIsAConflict()
    {
        var author = await PostAsync<AuthorDto>("/authors", new AuthorInput { Name = "Octavia E. Butler" });
        await PostAsync<BookDto>("/books", NewBook(author.Id, "978-0446675505", BookStatus.Published, copies: 1));

        var response = await client.PostAsJsonAsync("/books", NewBook(author.Id, "978-0446675505", BookStatus.Published, copies: 1));

        Assert.AreEqual(HttpStatusCode.Conflict, response.StatusCode);
    }

    private static BookInput NewBook(Guid authorId, string isbn, BookStatus status, int copies) => new()
    {
        Title = "A title",
        Isbn = isbn,
        Status = status,
        Copies = copies,
        AuthorId = authorId,
    };

    private async Task<T> PostAsync<T>(string url, object input)
    {
        var response = await client.PostAsJsonAsync(url, input);
        Assert.AreEqual(HttpStatusCode.Created, response.StatusCode, await response.Content.ReadAsStringAsync());
        return (await response.Content.ReadFromJsonAsync<T>())!;
    }

    private static async Task AssertInvalidAsync(HttpResponseMessage response, string property)
    {
        Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        var problem = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>();
        Assert.IsTrue(problem!.Errors.ContainsKey(property), string.Join(", ", problem.Errors.Keys));
    }
}
