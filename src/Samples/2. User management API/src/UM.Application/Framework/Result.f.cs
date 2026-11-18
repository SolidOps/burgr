namespace SolidOps.UM.Application;

public enum ErrorKind
{
    NotFound,
    Invalid,
    Conflict,
    Forbidden,
}

/// <summary>
/// Why a use case failed. The API turns it into a status code: 404, 400 with the
/// validation errors by property path, 409, 403.
/// </summary>
public sealed record Error(ErrorKind Kind, IReadOnlyDictionary<string, string[]>? Errors = null, string? Detail = null)
{
    public static Error NotFound(string? detail = null) => new(ErrorKind.NotFound, Detail: detail);

    public static Error Invalid(IReadOnlyDictionary<string, string[]> errors) => new(ErrorKind.Invalid, errors);

    public static Error Conflict(string? detail = null) => new(ErrorKind.Conflict, Detail: detail);

    public static Error Forbidden(string? detail = null) => new(ErrorKind.Forbidden, Detail: detail);
}

/// <summary>The outcome of a use case without a value: success, or an <see cref="Application.Error"/>.</summary>
public class Result
{
    protected Result(Error? error)
    {
        Error = error;
    }

    public Error? Error { get; }

    public bool IsSuccess => Error is null;

    public static Result Success() => new(null);

    public static implicit operator Result(Error error) => new(error);
}

/// <summary>The outcome of a use case: a value, or an <see cref="Application.Error"/>. Both convert implicitly.</summary>
public sealed class Result<T> : Result
{
    private readonly T? value;

    private Result(T value) : base(null)
    {
        this.value = value;
    }

    private Result(Error error) : base(error)
    {
    }

    /// <summary>The value; throws when the use case failed.</summary>
    public T Value => IsSuccess ? value! : throw new InvalidOperationException($"No value: the use case failed ({Error!.Kind})");

    public static implicit operator Result<T>(T value) => new(value);

    public static implicit operator Result<T>(Error error) => new(error);
}
