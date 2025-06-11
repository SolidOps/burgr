using Microsoft.VisualStudio.TestTools.UnitTesting;
using SolidOps.UM.Shared.Contracts.Endpoints;
using SolidOps.UM.Shared.Contracts.Results;

namespace SolidOps.UM.Shared.Tests;

public class TestServiceClient : AppServiceClient
{
    public TestServiceClient(HttpClient httpClient, bool enableDiagnostic, bool remote, HttpMessageHandler messageHandler)
        : base(httpClient, enableDiagnostic, remote, messageHandler)
    {
        
    }

    public override async Task AssertResult<TRequest, TResponse>(RequestParameters<TRequest, TResponse> parameters, HttpResponseMessage response, string responseContent, string uri)
    {
        if (parameters.AssertParameters.ExpectedStatusCode.HasValue)
        {
            Assert.AreEqual(parameters.AssertParameters.ExpectedStatusCode.Value, response.StatusCode);
            if (!string.IsNullOrEmpty(parameters.AssertParameters.ExpectedError))
            {
                Assert.AreEqual(parameters.AssertParameters.ExpectedError, responseContent.Substring(1, responseContent.Length - 2));
            }
        }
        else if (parameters.AssertParameters.EnsureSuccess)
        {
            var ensureResult = await EnsureSuccessStatusCode($"{parameters.Method} {uri}", response, parameters.AssertParameters.IsForTest);
            if (ensureResult.HasError)
                parameters.Failure = ensureResult.Error.Message;
        }
    }

    public override async Task AssertDiagnostic<TRequest, TResponse>(RequestParameters<TRequest, TResponse> parameters, HttpResponseMessage response, ResponseDiagnostic<TResponse> responseDiagnostic, string uri)
    {
        if (parameters.AssertParameters.MaximumNumberOfRequests.HasValue)
        {
            Assert.IsTrue(parameters.AssertParameters.MaximumNumberOfRequests.Value >= responseDiagnostic.NumberOfRequests, $"too many requests, expected max {parameters.AssertParameters.MaximumNumberOfRequests.Value}, current {responseDiagnostic.NumberOfRequests}");
        }
        if (parameters.AssertParameters.ExpectedStatusCode.HasValue)
        {
            Assert.AreEqual(parameters.AssertParameters.ExpectedStatusCode.Value, response.StatusCode);
            if (!string.IsNullOrEmpty(parameters.AssertParameters.ExpectedError))
            {
                var error = await response.Content.ReadAsStringAsync();
                Assert.AreEqual(parameters.AssertParameters.ExpectedError, error.Substring(1, error.Length - 2));
            }
        }
        else if (parameters.AssertParameters.EnsureSuccess)
        {
            var ensureResult = await EnsureSuccessStatusCode($"{parameters.Method} {uri}", response, parameters.AssertParameters.IsForTest);
        }
    }

    private static async Task<IOpsResult> EnsureSuccessStatusCode(string request, HttpResponseMessage response, bool isTest)
    {
        if (!response.IsSuccessStatusCode && response.StatusCode != System.Net.HttpStatusCode.NotModified)
        {
            var content = await response.Content.ReadAsStringAsync();
            if (isTest)
            {
                Assert.Fail($"Response status code does not indicate success:\n{request}\n{response.StatusCode}\n{Clean(content)}");
            }
            return IOpsResult.Invalid($"Response status code does not indicate success:\n{request}\n{response.StatusCode}\n{Clean(content)}");
        }
        return IOpsResult.Ok();
    }
}
