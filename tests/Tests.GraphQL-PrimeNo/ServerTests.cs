using System.Net;
using Microsoft.AspNetCore.Mvc.Testing;
using Tests.GraphQL_PrimeNo.TestServerExtensions;

namespace Tests.GraphQL_PrimeNo;

public class ServerTests<TProgram> : IDisposable where TProgram : class
{
    private readonly WebApplicationFactory<TProgram> _webApp = new();

    public Task VerifyGraphQLPostAsync(string url = "/graphql", string query = "{count}", string expected = @"{""data"":{""count"":0}}", HttpStatusCode statusCode = HttpStatusCode.OK, string? jwtToken = null)
        => _webApp.Server.VerifyGraphQLPostAsync(url, query, expected, statusCode, jwtToken);

    public void Dispose() => _webApp.Dispose();
}
