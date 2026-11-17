#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
namespace Acme.Library.Contracts;
// Object [AG]
public sealed partial class AuthorDto
{
    public Guid Id { get; set; }
    // Property [S][NO][NAR][PUO]
    public System.String Name { get; set; } = string.Empty;

    public String? Biography { get; set; } = null;

    // Property [M][LNA][AG][PUO]
    public List<BookDto> Books { get; set; } = [];

    public static AuthorDto From(Domain.Author entity) => new()
    {
        Id = entity.Id,
        // Property [S][NO][CA][NAR][PUO]
        Name = entity.Name,

        Biography = entity.Biography,

        // Property [M][LNA][AG][PUO]
        Books = entity.Books.Select(BookDto.From).ToList(),

    };
}
public sealed partial class AuthorInput
{
    // Property [S][NO][NAR][PUO][NRO]
    public System.String Name { get; set; } = string.Empty;

    public String? Biography { get; set; } = null;

    public Dictionary<string, string[]> Validate()
    {
        var errors = new Dictionary<string, string[]>();
        // Property [S][NO][NAR][PUO][NRO]
        // VALIDATION RULE - NULLABLE_PROPERTY
        if (string.IsNullOrWhiteSpace(Name))
        {
            errors["Name"] = ["Name is required"];
        }

        // Property [S][NO][NAR][PUO][NRO]
        // VALIDATION RULE - PROPERTY_HAS_MAX_SIZE
        if (Name?.Length > 100)
        {
            errors["Name"] = ["Name is longer than 100 characters"];
        }

        // VALIDATION RULE - PROPERTY_HAS_MAX_SIZE
        if (Biography?.Length > 255)
        {
            errors["Biography"] = ["Biography is longer than 255 characters"];
        }

        OnValidating(errors);
        return errors;
    }
    partial void OnValidating(Dictionary<string, string[]> errors);
    public void ApplyTo(Domain.Author entity)
    {
        // Property [S][NO][NAR][PUO][NRO]
        entity.Name = Name;

        entity.Biography = Biography;

    }
}
public sealed partial class BookDto
{
    public Guid Id { get; set; }
    // Property [S][NO][NAR][PUO]
    public System.String Title { get; set; } = string.Empty;

    public System.String Isbn { get; set; } = string.Empty;

    public System.Int32 Copies { get; set; } = default(System.Int32);

    // Property [S][CA][NAR][PUO]
    public System.Boolean IsAvailable { get; set; } = default(System.Boolean);

    // Property [E][NO][CA][NAR][PUO]
    public Domain.BookStatus Status { get; set; }

    // Property [M][NO][NAR][AG][PUO]
    public Guid AuthorId { get; set; }

    public static BookDto From(Domain.Book entity) => new()
    {
        Id = entity.Id,
        // Property [S][NO][CA][NAR][PUO]
        Title = entity.Title,

        Isbn = entity.Isbn,

        Copies = entity.Copies,

        IsAvailable = entity.IsAvailable,

        // Property [E][NO][CA][NAR][PUO]
        Status = entity.Status,

        // Property [M][NO][NAR][AG][PUO]
        AuthorId = entity.AuthorId,

    };
}
public sealed partial class BookInput
{
    // Property [S][NO][NAR][PUO][NRO]
    public System.String Title { get; set; } = string.Empty;

    public System.String Isbn { get; set; } = string.Empty;

    public System.Int32 Copies { get; set; } = default(System.Int32);

    // Property [E][NO][NAR][PUO][NRO]
    public Domain.BookStatus Status { get; set; }

    // Property [M][NO][NAR][AG][PUO][NRO]
    public Guid AuthorId { get; set; }

    public Dictionary<string, string[]> Validate()
    {
        var errors = new Dictionary<string, string[]>();
        // Property [S][NO][NAR][PUO][NRO]
        // VALIDATION RULE - NULLABLE_PROPERTY
        if (string.IsNullOrWhiteSpace(Title))
        {
            errors["Title"] = ["Title is required"];
        }

        // VALIDATION RULE - NULLABLE_PROPERTY
        if (string.IsNullOrWhiteSpace(Isbn))
        {
            errors["Isbn"] = ["Isbn is required"];
        }

        // Property [S][NO][NAR][PUO][NRO]
        // VALIDATION RULE - PROPERTY_HAS_MAX_SIZE
        if (Title?.Length > 200)
        {
            errors["Title"] = ["Title is longer than 200 characters"];
        }

        // VALIDATION RULE - PROPERTY_HAS_MAX_SIZE
        if (Isbn?.Length > 20)
        {
            errors["Isbn"] = ["Isbn is longer than 20 characters"];
        }

        // Property [E][NO][NAR][PUO][NRO][NN]
        if (!Enum.IsDefined(Status))
        {
            errors["Status"] = ["Status is not a valid value"];
        }

        OnValidating(errors);
        return errors;
    }
    partial void OnValidating(Dictionary<string, string[]> errors);
    public void ApplyTo(Domain.Book entity)
    {
        // Property [S][NO][NAR][PUO][NRO]
        entity.Title = Title;

        entity.Isbn = Isbn;

        entity.Copies = Copies;

        // Property [E][NO][NAR][PUO][NRO]
        entity.Status = Status;

        // Property [M][NO][NAR][AG][PUO][NRO]
        entity.AuthorId = AuthorId;

    }
}