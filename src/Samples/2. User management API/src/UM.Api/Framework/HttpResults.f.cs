using SolidOps.UM.Application;
using SolidOps.UM.Domain;

namespace SolidOps.UM.Api;

/// <summary>Turns the outcome of a use case into an HTTP response.</summary>
public static class HttpResults
{
    /// <summary>200 with the value, or the response of the error.</summary>
    public static IResult ToHttpResult<T>(this Result<T> result)
    {
        return result.IsSuccess ? TypedResults.Ok(result.Value) : result.Error!.ToHttpResult();
    }

    /// <summary>201 with the value at <paramref name="location"/>, or the response of the error.</summary>
    public static IResult ToCreatedHttpResult<T>(this Result<T> result, Func<T, string> location)
    {
        return result.IsSuccess ? TypedResults.Created(location(result.Value), result.Value) : result.Error!.ToHttpResult();
    }

    /// <summary>204, or the response of the error.</summary>
    public static IResult ToHttpResult(this Result result)
    {
        return result.IsSuccess ? TypedResults.NoContent() : result.Error!.ToHttpResult();
    }

    /// <summary>404, 400 with the validation errors, 409 or 403, as a problem details body.</summary>
    public static IResult ToHttpResult(this Error error)
    {
        return error.Kind switch
        {
            ErrorKind.Invalid => TypedResults.ValidationProblem(error.Errors ?? new Dictionary<string, string[]>()),
            ErrorKind.NotFound => TypedResults.Problem(error.Detail, statusCode: StatusCodes.Status404NotFound),
            ErrorKind.Conflict => TypedResults.Problem(error.Detail, statusCode: StatusCodes.Status409Conflict),
            ErrorKind.Forbidden => TypedResults.Problem(error.Detail, statusCode: StatusCodes.Status403Forbidden),
            _ => throw new ArgumentOutOfRangeException(nameof(error), error.Kind, null),
        };
    }

    /// <summary>The page request of the query string: page (from 1), pageSize, orderBy, descending.</summary>
    public static PageRequest ToPageRequest(int? page, int? pageSize, string? orderBy, bool? descending)
    {
        return new PageRequest(page ?? 1, pageSize ?? PageRequest.DefaultPageSize, orderBy, descending ?? false);
    }
}
