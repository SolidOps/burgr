namespace Acme.Library.Contracts;

public sealed partial class BookInput
{
    partial void OnValidating(Dictionary<string, string[]> errors)
    {
        if (Copies < 0)
        {
            errors[nameof(Copies)] = ["Copies cannot be negative"];
        }
    }
}
