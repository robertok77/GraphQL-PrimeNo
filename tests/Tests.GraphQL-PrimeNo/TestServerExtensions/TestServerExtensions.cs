using System.Net;
using Microsoft.AspNetCore.TestHost;

namespace Tests.GraphQL_PrimeNo.TestServerExtensions;

public static class TestServerExtensions
{
    public static async Task VerifyGraphQLPostAsync(
        this TestServer server,
        string url = "/graphql",
        string query = "{count}",
        string expected = @"{""data"":{""count"":0}}",
        HttpStatusCode statusCode = HttpStatusCode.OK,
        string? jwtToken = null)
    {
        using var client = server.CreateClient();
        var body = System.Text.Json.JsonSerializer.Serialize(new { query });
        var content = new StringContent(body, System.Text.Encoding.UTF8, "application/json");
        using var request = new HttpRequestMessage(HttpMethod.Post, url);
        request.Content = content;
        if (jwtToken != null)
            request.Headers.Authorization = new("Bearer", jwtToken);
        using var response = await client.SendAsync(request);
        response.StatusCode.ShouldBe(statusCode);
        var ret = await response.Content.ReadAsStringAsync();
        ret.ShouldBe(expected);
    }

}
