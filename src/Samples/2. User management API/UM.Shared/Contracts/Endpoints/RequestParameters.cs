using System.Net;

namespace SolidOps.UM.Shared.Contracts.Endpoints;

public class RequestParameters
{
    public RequestParameters(AssertParameters assertParameters)
    {
        if (assertParameters != null)
        {
            AssertParameters = assertParameters;
        }
        else
        {
            AssertParameters = new AssertParameters();
        }
    }

    public RequestParameters(bool ensureSuccess)
    {
        AssertParameters = new AssertParameters() { EnsureSuccess = ensureSuccess, IsForTest = false };
    }

    public string Uri { get; set; }

    public string Method { get; set; }

    public string Id { get; set; }

    public string SubRoute { get; set; }

    public bool DeserializeResponse { get; set; } = true;

    public AssertParameters AssertParameters { get; set; }

    public string ResponseContinuationId { get; set; }
    public string Failure { get; set; }
}

public class RequestParameters<T> : RequestParameters<T, T>
{
    public RequestParameters(AssertParameters assertParameters) : base(assertParameters)
    {
    }

    public RequestParameters(AssertParameters assertParameters, string method, string uri) : base(assertParameters, method, uri)
    {

    }

    public RequestParameters(AssertParameters assertParameters, string method, string uri, T data) : base(assertParameters, method, uri, data)
    {

    }
    public RequestParameters(bool ensureSuccess) : base(ensureSuccess)
    {
    }

    public RequestParameters(bool ensureSuccess, string method, string uri) : base(ensureSuccess, method, uri)
    {

    }

    public RequestParameters(bool ensureSuccess, string method, string uri, T data) : base(ensureSuccess, method, uri, data)
    {

    }
}

public class RequestParameters<TRequest, TResponse> : RequestParameters
{
    public RequestParameters(AssertParameters assertParameters) : base(assertParameters)
    {

    }

    public RequestParameters(AssertParameters assertParameters, string method, string uri) : base(assertParameters)
    {
        Method = method;
        Uri = uri;
    }

    public RequestParameters(AssertParameters assertParameters, string method, string uri, TRequest data) : base(assertParameters)
    {
        Method = method;
        Uri = uri;
        Data = data;
    }
    public RequestParameters(bool ensureSuccess) : base(ensureSuccess)
    {

    }

    public RequestParameters(bool ensureSuccess, string method, string uri) : base(ensureSuccess)
    {
        Method = method;
        Uri = uri;
    }

    public RequestParameters(bool ensureSuccess, string method, string uri, TRequest data) : base(ensureSuccess)
    {
        Method = method;
        Uri = uri;
        Data = data;
    }

    public TRequest Data { get; set; }
}

public class AssertParameters
{
    public bool EnsureSuccess { get; set; } = true;

    private HttpStatusCode? _expectedStatusCode;
    public HttpStatusCode? ExpectedStatusCode
    {
        get
        {
            return _expectedStatusCode;
        }
        set
        {
            _expectedStatusCode = value;
            if (_expectedStatusCode.HasValue && ((int)_expectedStatusCode.Value < 200 || ((int)_expectedStatusCode.Value > 299 && (int)_expectedStatusCode.Value != 304)))
            {
                EnsureSuccess = false;
            }
        }
    }
    public int? MaximumDuration { get; set; }

    public int? MaximumNumberOfRequests { get; set; }

    public string ExpectedError { get; set; }

    public bool IsForTest = true;
}
