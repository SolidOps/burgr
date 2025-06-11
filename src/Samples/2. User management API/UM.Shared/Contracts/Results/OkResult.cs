namespace SolidOps.UM.Shared.Contracts.Results;

public class OkResult<T> : OkResult, IOpsResult<T>
{
    public T Data { get; }

    internal OkResult(T data) : base()
    {
        Data = data;
    }
}

public class OkResult : IOpsResult
{
    public OkResult() { }

    public OpsError Error { get => null; }

    public IOpsResult<T> ToResult<T>()
    {
        throw new NotImplementedException();
    }
}
