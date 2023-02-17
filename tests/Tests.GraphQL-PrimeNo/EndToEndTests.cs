using ServerAPI.GraphQL_PrimeNo;

namespace Tests.GraphQL_PrimeNo;

public class EndToEndTests
{
    private const string _url = @"http://localhost:5068/graphql";


    [Theory]
    [InlineData("{getPrimeNumberList}", "{\"data\":{\"getPrimeNumberList\":[]}}")]
    [InlineData("mutation{\r\npostPrimeSieve(\r\nprimeSieve:{amount:10}){\r\nprimeSieveId\r\nstart\r\nend\r\n}\r\n}"
        , "{\"data\":{\"postPrimeSieve\":{\"primeSieveId\":1,\"start\":2,\"end\":12}}}")]
    public Task GraphQLPostPayLoadExpectedResponse(string query, string expected)
        => new ServerTests<Program>().VerifyGraphQLPostAsync(url: _url, query: query, expected: expected);

    [Fact]
    public async Task GraphQLPostSieveExpectedPrimeNumbers()
    {
        using var serverTests = new ServerTests<Program>();
        var postMutation = "mutation{\r\npostPrimeSieve(\r\nprimeSieve:{amount:10}){\r\nprimeSieveId\r\nstart\r\nend\r\n}\r\n}";
        var expectedMutationResponse = "{\"data\":{\"postPrimeSieve\":{\"primeSieveId\":1,\"start\":2,\"end\":12}}}";

        await serverTests.VerifyGraphQLPostAsync(url: _url, query: postMutation, expected: expectedMutationResponse);
        await Task.Delay(11_000);
        await serverTests.VerifyGraphQLPostAsync(url: _url, query: "{getPrimeNumberList}", expected: "{\"data\":{\"getPrimeNumberList\":[2,3,5,7,11]}}");
        
    }
}
