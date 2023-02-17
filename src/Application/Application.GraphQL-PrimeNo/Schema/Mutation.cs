using Application.GraphQL_PrimeNo.Service;
using GraphQL;

namespace Application.GraphQL_PrimeNo.Schema;

public class Mutation
{
    public static PrimeSieveMessage PostPrimeSieve([FromServices] IPrimeNoService primeNoService, InputPrimeSieveMessage primeSieve)
        => primeNoService.PostPrimeSieveMessage(primeSieve);
}
