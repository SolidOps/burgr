namespace Acme.Library.Domain;

public partial class Book
{
    private partial bool CalculateIsAvailable() => Status == BookStatus.Published && Copies > 0;
}
