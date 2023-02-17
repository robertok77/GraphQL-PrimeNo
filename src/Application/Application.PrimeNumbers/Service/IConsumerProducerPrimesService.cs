namespace Application.PrimeNumbers.Service;

public interface IConsumerProducerPrimesService
{
    event EventHandler<PrimeNoEventArgs>? PrimeNoEvent;
    event EventHandler<PrimeSieveEventArgs>? PrimeSieveDoneEvent;
    PrimeSieve ProducePrimeSieve(int amount);
    Task ConsumePrimeSieveAsync(CancellationToken stoppingToken);
    IEnumerable<int> GetPrimeNoEnumerable();
}

public record PrimeSieve(int Id, int Start, int Range)
{
    public int End => Start + Range;
}

public class PrimeSieveEventArgs : EventArgs
{
    public int SieveId { get; set; }
    public int[]? PrimeNoData { get; set; }
}

public class PrimeNoEventArgs : EventArgs
{
    public int PrimeNo { get; set; }
}
