using Application.GraphQL_PrimeNo.Service;
using GraphQL;

namespace Application.GraphQL_PrimeNo.Schema;

public class Query
{
    public static int[] GetPrimeNumberList([FromServices] IPrimeNoService primeNoService, int? from = 2, int? to = int.MaxValue) 
        => primeNoService.GetPrimeNoDataMessage(from, to);
}
