namespace SolidOps.UM.Shared.Contracts.Results;

public class OpsError
{
    public string Key { get; set; }
    public string Message { get; set; }
    public ErrorType ErrorType { get; set; }
    internal OpsError(string message) : this("General", message, ErrorType.Invalid) { }
    internal OpsError(string key, string message) : this(key, message, ErrorType.Invalid) { }
    internal OpsError(string message, ErrorType errorType) : this("General", message, ErrorType.Invalid) { }
    internal OpsError(string key, string message, ErrorType errorType)
    {
        Key = key;
        Message = message;
        ErrorType = errorType;
    }
}

public enum ErrorType
{
    Invalid,
    Forbidden
}
