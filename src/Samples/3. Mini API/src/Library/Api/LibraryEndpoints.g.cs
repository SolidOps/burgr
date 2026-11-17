#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Acme.Library.Contracts;
using Acme.Library.Infrastructure;
namespace Acme.Library.Api;
public static partial class LibraryEndpoints
{
    public static IEndpointRouteBuilder MapLibraryEndpoints(this IEndpointRouteBuilder app)
    {
        // Object [AG][R]
        MapAuthorEndpoints(app.MapGroup("/authors").WithTags("Authors"));

        MapBookEndpoints(app.MapGroup("/books").WithTags("Books"));

        return app;
    }
    // a unique index or a foreign key violated by a concurrent change
    private static async Task<bool> TrySaveAsync(LibraryDbContext db, CancellationToken cancellationToken)
    {
        try
        {
            await db.SaveChangesAsync(cancellationToken);
            return true;
        }
        catch (DbUpdateException)
        {
            return false;
        }
    }
    // Object [AG][R]
    private static void MapAuthorEndpoints(RouteGroupBuilder group)
    {

        group.MapGet("/", async (LibraryDbContext db, CancellationToken cancellationToken) =>
        {
            var entities = await db.Authors.AsNoTracking().ToListAsync(cancellationToken);
            return TypedResults.Ok(entities.Select(AuthorDto.From).ToList());
        });

        group.MapGet("/{id}", async Task<Results<Ok<AuthorDto>, NotFound>> (Guid id, LibraryDbContext db, CancellationToken cancellationToken) =>
        {
            var entity = await db.Authors.AsNoTracking()
                // Property [M][LNA][AG]
                .Include(x => x.Books)

                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
            return entity is null ? TypedResults.NotFound() : TypedResults.Ok(AuthorDto.From(entity));
        });

        group.MapPost("/", async Task<Results<Created<AuthorDto>, ValidationProblem, Conflict>> (AuthorInput input, LibraryDbContext db, CancellationToken cancellationToken) =>
        {
            var errors = await ValidateAuthorAsync(input, db, cancellationToken);
            if (errors.Count > 0)
            {
                return TypedResults.ValidationProblem(errors);
            }
            var entity = new Domain.Author();
            input.ApplyTo(entity);
            db.Authors.Add(entity);
            if (!await TrySaveAsync(db, cancellationToken))
            {
                return TypedResults.Conflict();
            }
            return TypedResults.Created($"/authors/{entity.Id}", AuthorDto.From(entity));
        });

        group.MapPut("/{id}", async Task<Results<Ok<AuthorDto>, NotFound, ValidationProblem, Conflict>> (Guid id, AuthorInput input, LibraryDbContext db, CancellationToken cancellationToken) =>
        {
            var entity = await db.Authors.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
            if (entity is null)
            {
                return TypedResults.NotFound();
            }
            var errors = await ValidateAuthorAsync(input, db, cancellationToken);
            if (errors.Count > 0)
            {
                return TypedResults.ValidationProblem(errors);
            }
            input.ApplyTo(entity);
            if (!await TrySaveAsync(db, cancellationToken))
            {
                return TypedResults.Conflict();
            }
            return TypedResults.Ok(AuthorDto.From(entity));
        });

        group.MapDelete("/{id}", async Task<Results<NoContent, NotFound, Conflict>> (Guid id, LibraryDbContext db, CancellationToken cancellationToken) =>
        {
            var entity = await db.Authors.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
            if (entity is null)
            {
                return TypedResults.NotFound();
            }
            db.Authors.Remove(entity);
            if (!await TrySaveAsync(db, cancellationToken))
            {
                return TypedResults.Conflict();
            }
            return TypedResults.NoContent();
        });

    }
    private static async Task<Dictionary<string, string[]>> ValidateAuthorAsync(AuthorInput input, LibraryDbContext db, CancellationToken cancellationToken)
    {
        var errors = input.Validate();

        return errors;
    }

    private static void MapBookEndpoints(RouteGroupBuilder group)
    {

        group.MapGet("/", async (LibraryDbContext db, CancellationToken cancellationToken) =>
        {
            var entities = await db.Books.AsNoTracking().ToListAsync(cancellationToken);
            return TypedResults.Ok(entities.Select(BookDto.From).ToList());
        });

        group.MapGet("/{id}", async Task<Results<Ok<BookDto>, NotFound>> (Guid id, LibraryDbContext db, CancellationToken cancellationToken) =>
        {
            var entity = await db.Books.AsNoTracking()

                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
            return entity is null ? TypedResults.NotFound() : TypedResults.Ok(BookDto.From(entity));
        });

        group.MapPost("/", async Task<Results<Created<BookDto>, ValidationProblem, Conflict>> (BookInput input, LibraryDbContext db, CancellationToken cancellationToken) =>
        {
            var errors = await ValidateBookAsync(input, db, cancellationToken);
            if (errors.Count > 0)
            {
                return TypedResults.ValidationProblem(errors);
            }
            var entity = new Domain.Book();
            input.ApplyTo(entity);
            db.Books.Add(entity);
            if (!await TrySaveAsync(db, cancellationToken))
            {
                return TypedResults.Conflict();
            }
            return TypedResults.Created($"/books/{entity.Id}", BookDto.From(entity));
        });

        group.MapPut("/{id}", async Task<Results<Ok<BookDto>, NotFound, ValidationProblem, Conflict>> (Guid id, BookInput input, LibraryDbContext db, CancellationToken cancellationToken) =>
        {
            var entity = await db.Books.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
            if (entity is null)
            {
                return TypedResults.NotFound();
            }
            var errors = await ValidateBookAsync(input, db, cancellationToken);
            if (errors.Count > 0)
            {
                return TypedResults.ValidationProblem(errors);
            }
            input.ApplyTo(entity);
            if (!await TrySaveAsync(db, cancellationToken))
            {
                return TypedResults.Conflict();
            }
            return TypedResults.Ok(BookDto.From(entity));
        });

        group.MapDelete("/{id}", async Task<Results<NoContent, NotFound, Conflict>> (Guid id, LibraryDbContext db, CancellationToken cancellationToken) =>
        {
            var entity = await db.Books.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
            if (entity is null)
            {
                return TypedResults.NotFound();
            }
            db.Books.Remove(entity);
            if (!await TrySaveAsync(db, cancellationToken))
            {
                return TypedResults.Conflict();
            }
            return TypedResults.NoContent();
        });

    }
    private static async Task<Dictionary<string, string[]>> ValidateBookAsync(BookInput input, LibraryDbContext db, CancellationToken cancellationToken)
    {
        var errors = input.Validate();
        // Property [M][NO][NAR][AG][PUO][NRO][NN]
        if (!await db.Authors.AnyAsync(x => x.Id == input.AuthorId, cancellationToken))
        {
            errors["AuthorId"] = ["Author not found"];
        }

        return errors;
    }

}