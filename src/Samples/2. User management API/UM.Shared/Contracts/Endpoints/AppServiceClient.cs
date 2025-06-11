using SolidOps.SubZero;
using System.Diagnostics;
using System.Text;

namespace SolidOps.UM.Shared.Contracts.Endpoints;

public class AppServiceClient : IDisposable
{
    public Dictionary<string, string> etagsByUri = new Dictionary<string, string>();
    public Dictionary<string, string> cachedContentByUri = new Dictionary<string, string>();
    public AppServiceClient(HttpClient httpClient, bool enableDiagnostic, bool remote, HttpMessageHandler messageHandler)
    {
        HttpClient = httpClient;
        HttpMessageHandler = messageHandler;
        EnableDiagnostic = enableDiagnostic;
        Diagnostics = new List<ResponseDiagnostic>();
        Remote = remote;
    }
    public bool Remote { get; set; }
    public HttpClient HttpClient { get; }
    public HttpMessageHandler HttpMessageHandler { get; }
    public bool EnableDiagnostic { get; }
    public AppServiceClient ParentClient { get; set; }

    public List<ResponseDiagnostic> Diagnostics { get; set; }

    public ResponseDiagnostic LastDiagnostic
    {
        get
        {
            if (Diagnostics != null && Diagnostics.Count > 0)
            {
                return Diagnostics[Diagnostics.Count - 1];
            }
            return null;
        }
    }

    public ResponseDiagnostic OverallDiagnostic
    {
        get
        {
            if (Diagnostics != null && Diagnostics.Count > 0)
            {
                ResponseDiagnostic overallDiagnostic = new ResponseDiagnostic("overall");
                overallDiagnostic.DurationMs = Diagnostics.Sum(d => d.DurationMs);
                overallDiagnostic.NumberOfRequests = Diagnostics.Sum(d => d.NumberOfRequests);
                overallDiagnostic.NetworkShare = Diagnostics.Sum(d => d.NetworkShare) / Diagnostics.Count;
                overallDiagnostic.ApplicationLayerShare = Diagnostics.Sum(d => d.ApplicationLayerShare) / Diagnostics.Count;
                overallDiagnostic.DataAccessLayerShare = Diagnostics.Sum(d => d.DataAccessLayerShare) / Diagnostics.Count;
                return overallDiagnostic;
            }
            return null;

        }
    }

    public async Task<T> Send<T>(RequestParameters<T> parameters)
    {
        return await Send<T, T>(parameters);
    }

    public async Task<TResponse> Send<TRequest, TResponse>(RequestParameters<TRequest, TResponse> parameters)
    {
        Stopwatch watch = null;
        if (EnableDiagnostic)
        {
            watch = Stopwatch.StartNew();
        }
        string uri = parameters.Uri;
        if (uri == null)
        {
            uri = UriHelper.Instance.GetUri<TRequest>(parameters.Id, parameters.SubRoute);
        }

        if (etagsByUri.ContainsKey(uri))
        {
            HttpClient.DefaultRequestHeaders.Add("If-None-Match", etagsByUri[uri]);
        }

        if (EnableDiagnostic)
        {
            HttpClient.DefaultRequestHeaders.Add("X-Diag", "1");
            HttpClient.DefaultRequestHeaders.Add("X-Requests", "1");
        }

        StringContent body = null;
        if (parameters.Data != null)
        {
            body = new StringContent(Serializer.Serialize(parameters.Data), Encoding.UTF8, "application/json");
        }

        HttpResponseMessage response;
        switch (parameters.Method.ToUpper())
        {
            case "GET":
                response = await HttpClient.GetAsync(uri);
                break;
            case "POST":
                response = await HttpClient.PostAsync(uri, body);
                break;
            case "PUT":
                response = await HttpClient.PutAsync(uri, body);
                break;
            case "DELETE":
                response = await HttpClient.DeleteAsync(uri);
                break;
            case "PATCH":
                response = await HttpClient.PatchAsync(uri, body);
                break;
            default:
                throw new Exception("Unsupported");
        }
        TResponse result = default;
        string responseContent = await response.Content.ReadAsStringAsync();
        if (response.StatusCode == System.Net.HttpStatusCode.NotModified)
        {
            if (cachedContentByUri.ContainsKey(uri))
            {
                responseContent = cachedContentByUri[uri];
            }
        }

        await AssertResult<TRequest, TResponse>(parameters, response, responseContent, uri);

        if (response.Headers.Contains("ETag"))
        {
            var etag = response.Headers.GetValues("ETag").FirstOrDefault();
            if (etag != null)
            {
                if (etagsByUri.ContainsKey(uri))
                    etagsByUri.Remove(uri);
                etagsByUri.Add(uri, etag);

                if (cachedContentByUri.ContainsKey(uri))
                    cachedContentByUri.Remove(uri);
                cachedContentByUri.Add(uri, responseContent);
            }
        }

        if (response.Headers.Contains("ContinuationId"))
        {
            parameters.ResponseContinuationId = response.Headers.GetValues("ContinuationId").FirstOrDefault();
        }

        if (etagsByUri.ContainsKey(uri))
        {
            HttpClient.DefaultRequestHeaders.Remove("If-None-Match");
        }

        if (parameters.AssertParameters.EnsureSuccess)
        {
            if (parameters.DeserializeResponse)
            {
                if (typeof(TResponse) == typeof(string))
                {
                    result = (TResponse)(object)responseContent;
                }
                else
                {
                    result = Serializer.Deserialize<TResponse>(responseContent);
                }
            }
            else
            {
                IEnumerable<string> locations;
                if (typeof(TResponse) == typeof(string) && response.Headers.TryGetValues("Location", out locations) && locations.Any())
                {
                    result = (TResponse)(object)locations.First();
                }
            }
        }

        if (EnableDiagnostic)
        {
            ResponseDiagnostic<TResponse> responseDiagnostic = new ResponseDiagnostic<TResponse>(parameters.Method + " " + uri);
            responseDiagnostic.ResponseContent = responseContent;
            responseDiagnostic.Result = default;
            if (response.IsSuccessStatusCode)
            {
                responseDiagnostic.Result = result;
            }
            responseDiagnostic.HttpStatusCode = response.StatusCode;
            HttpClient.DefaultRequestHeaders.Remove("X-Diag");
            HttpClient.DefaultRequestHeaders.Remove("X-Requests");
            responseDiagnostic.DurationMs = Convert.ToInt32(watch.ElapsedMilliseconds);
            IEnumerable<string> diagValues;
            if (response.Headers.TryGetValues("X-Diag", out diagValues))
            {
                string diagValue = diagValues.Single();
                var arr = diagValue.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                var internalDuration = int.Parse(arr[0]);
                if (arr.Length > 2)
                {
                    var dbRequestsDuration = int.Parse(arr[1]);
                    var requestNumber = int.Parse(arr[2]);
                    responseDiagnostic.NumberOfRequests = requestNumber;
                    responseDiagnostic.DataAccessLayerShare = Convert.ToInt32(dbRequestsDuration * 100 / responseDiagnostic.DurationMs);
                    responseDiagnostic.ApplicationLayerShare = Convert.ToInt32((internalDuration - dbRequestsDuration) * 100 / responseDiagnostic.DurationMs);
                    responseDiagnostic.NetworkShare = Convert.ToInt32((responseDiagnostic.DurationMs - internalDuration) * 100 / responseDiagnostic.DurationMs);
                }
            }

            await AssertDiagnostic(parameters, response, responseDiagnostic, uri);

            IEnumerable<string> jsonRequests;
            if (response.Headers.TryGetValues("X-Requests", out jsonRequests))
            {
                List<RequestStat> requests = Serializer.Deserialize<List<RequestStat>>(jsonRequests.Single());
                responseDiagnostic.Requests = requests;
            }

            this.Diagnostics.Add(responseDiagnostic);
        }

        return result;
    }

    public virtual Task AssertResult<TRequest, TResponse>(RequestParameters<TRequest, TResponse> parameters, HttpResponseMessage response, string responseContent, string uri)
    {
        return Task.CompletedTask;
    }

    public virtual Task AssertDiagnostic<TRequest, TResponse>(RequestParameters<TRequest, TResponse> parameters, HttpResponseMessage response, ResponseDiagnostic<TResponse> responseDiagnostic, string uri)
    {
        return Task.CompletedTask;
    }

    protected static string Clean(string input)
    {
        return input.Replace("\\r\\n", "\n");
    }

    public void Dispose()
    {
        this.HttpClient.Dispose();
    }
}
