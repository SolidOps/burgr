#nullable enable
using System;
using System.Collections.Generic;
namespace Acme.Library.Domain;
// Enum 
public enum BookStatus
{
    Draft = 0,
Published = 1,
OutOfPrint = 2
}
// Object [AG]
public partial class Author
{
    public Guid Id { get; set; }
    // Property [S][NO][NAR]
    public System.String Name { get; set; } = string.Empty;

    public String? Biography { get; set; } = null;

    // Property [M][LNA][AG]
    public List<Book> Books { get; set; } = [];

}
public partial class Book
{
    public Guid Id { get; set; }
    // Property [S][NO][NAR]
    public System.String Title { get; set; } = string.Empty;

    public System.String Isbn { get; set; } = string.Empty;

    public System.Int32 Copies { get; set; } = default(System.Int32);

    // Property [E][NO][NAR]
    public BookStatus Status { get; set; }

    // Property [M][NO][NAR][AG]
    public Guid AuthorId { get; set; }
    public Author? Author { get; set; }

    // Property [S][CA][NAR]
    public System.Boolean IsAvailable => CalculateIsAvailable();
    private partial System.Boolean CalculateIsAvailable();

}