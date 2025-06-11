using System.Net;

namespace SolidOps.UM.Shared.Contracts.Endpoints;

public class ResponseDiagnostic
{
    public ResponseDiagnostic(string uri)
    {
        Uri = uri;
    }
    public string Uri { get; set; }
    public HttpStatusCode HttpStatusCode { get; set; }

    public string ResponseContent { get; set; }

    public int DurationMs { get; set; }

    public int NumberOfRequests { get; set; }

    public int ApplicationLayerShare { get; set; }

    public int DataAccessLayerShare { get; set; }

    public int NetworkShare { get; set; }

    public List<RequestStat> Requests { get; set; }

    public override string ToString()
    {
        return Uri;
    }
}

public class ResponseDiagnostic<T> : ResponseDiagnostic
{
    public ResponseDiagnostic(string uri) : base(uri)
    {

    }
    public T Result { get; set; }
}
