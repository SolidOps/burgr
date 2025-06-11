namespace SolidOps.UM.Shared.Contracts.Results;

public interface IOpsResult
{
    public OpsError Error { get; }

    public bool HasError { get { return Error != null; } }

    public IOpsResult<T> ToResult<T>();

    #region Ok
    public static IOpsResult Ok()
    {
        return new OkResult();
    }
    
    public static IOpsResult<T> Ok<T>(T data)
    {
        return new OkResult<T>(data);
    }
    #endregion

    #region Invalid
    public static IOpsResult Invalid(string message)
    {
        return new InvalidResult(message);
    }

    public static IOpsResult Invalid(string key, string message)
    {
        return new InvalidResult(key, message);
    }
    #endregion

    #region Forbidden
    public static IOpsResult Forbidden(string message)
    {
        return new ForbiddenResult(message);
    }

    public static IOpsResult Forbidden(string key, string message)
    {
        return new ForbiddenResult(key, message);
    }
    #endregion
}

public interface IOpsResult<T> : IOpsResult
{
    public T Data { get; }
}
