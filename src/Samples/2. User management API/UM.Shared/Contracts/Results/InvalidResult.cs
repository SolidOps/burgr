namespace SolidOps.UM.Shared.Contracts.Results;

public class InvalidResult<T> : InvalidResult, IOpsResult<T>
{
    public T Data { get => default; }
    internal InvalidResult(string message) : base(message) { }
    internal InvalidResult(string key, string message) : base(key, message) { }
}

public class InvalidResult : IOpsResult
{
    public OpsError Error { get; }
    internal InvalidResult(string message) : this("General", message) { }
    internal InvalidResult(string key, string message)
    {
        Error = new OpsError(key, message, ErrorType.Invalid);
    }
    public IOpsResult<T> ToResult<T>()
    {
        return new InvalidResult<T>(Error.Key, Error.Message);
    }
}
