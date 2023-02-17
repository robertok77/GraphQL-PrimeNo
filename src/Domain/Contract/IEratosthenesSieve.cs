namespace Domain.PrimeNumbers.Contract;

public interface IEratosthenesSieveAlgorithm
{
    IEnumerable<ISieveResponse> Search(int start, int range, CancellationToken token);
    IAsyncEnumerable<ISieveResponse> SearchAsync(int start, int range, IAsyncEnumerable<int> primesAsyncEnumerable, CancellationToken token);
}
