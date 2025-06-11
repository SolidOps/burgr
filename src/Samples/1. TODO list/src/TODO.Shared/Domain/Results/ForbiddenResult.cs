namespace SolidOps.TODO.Shared.Domain.Results;

public class ForbiddenResult<T> : ForbiddenResult, IOpsResult<T>
{
    public T Data { get => default; }
    internal ForbiddenResult(string message) : base(message) { }
    internal ForbiddenResult(string key, string message) : base(key, message) { }
}

public class ForbiddenResult : IOpsResult
{
    public OpsError Error { get; }
    internal ForbiddenResult(string message) : this("General", message) { }
    internal ForbiddenResult(string key, string message)
    {
        Error = new OpsError(key, message, ErrorType.Forbidden);
    }
    public IOpsResult<T> ToResult<T>()
    {
        return new ForbiddenResult<T>(Error.Key, Error.Message);
    }
}
