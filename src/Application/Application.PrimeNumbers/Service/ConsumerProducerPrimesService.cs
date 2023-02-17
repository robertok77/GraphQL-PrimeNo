using System.Collections.Concurrent;
using System.Threading.Channels;
using Application.PrimeNumbers.Extensions;
using Domain.PrimeNumbers.Contract;
using Microsoft.Extensions.Logging;

namespace Application.PrimeNumbers.Service;

public class ConsumerProducerPrimesService : IConsumerProducerPrimesService
{
    private readonly IEratosthenesSieveAlgorithm _algorithm;
    private readonly ILogger<ConsumerProducerPrimesService> _logger;
    private readonly Channel<PrimeSieve> _channel = Channel.CreateUnbounded<PrimeSieve>();
    private ChannelWriter<PrimeSieve> ChannelWriter => _channel.Writer;
    private ChannelReader<PrimeSieve> ChannelReader => _channel.Reader;
    private readonly ConcurrentQueue<int> _primeNoData = new();
    private PrimeSieve _primeSieve = new(0, 0, 0);
    private readonly object _object = new();
    public event EventHandler<PrimeSieveEventArgs>? PrimeSieveDoneEvent;
    public event EventHandler<PrimeNoEventArgs>? PrimeNoEvent;
    public ConsumerProducerPrimesService(IEratosthenesSieveAlgorithm algorithm, ILogger<ConsumerProducerPrimesService> logger)
    {
        _algorithm = algorithm;
        _logger = logger;
    }
    public IEnumerable<int> GetPrimeNoEnumerable() => _primeNoData.ToArray();
    public PrimeSieve ProducePrimeSieve(int amount)
    {
        lock (_object)
        {
            var result = _primeSieve.Id == 0
                ? new PrimeSieve(Id: 1, Start: 2, Range: amount)
                : new PrimeSieve(Id: _primeSieve.Id + 1, Start: _primeSieve.End + 1, Range: amount);

            if (!ChannelWriter.TryWrite(result))
                throw new ApplicationException($"Unable to append prime sieve {amount}");

            _primeSieve = result;
            return result;
        }
    }
    public async Task ConsumePrimeSieveAsync(CancellationToken stoppingToken)
    {
        await foreach (var primeSieve in ChannelReader.ReadAllAsync(CancellationToken.None))
        {
            await foreach (var primeNo in _algorithm.SearchAsync(primeSieve.Start, primeSieve.Range, _primeNoData.AsAsyncEnumerable(), stoppingToken))
            {
                await Task.Delay(2000);
                _primeNoData.Enqueue(primeNo.Value);
                OnPrimeNoEvent(new PrimeNoEventArgs() { PrimeNo = primeNo.Value });
            }
            OnPrimeSieveDoneEvent(new PrimeSieveEventArgs()
            {
                SieveId = primeSieve.Id,
                PrimeNoData = _primeNoData.Where(x => x >= primeSieve.Start && x <= primeSieve.End).ToArray()
            });
        }
    }
    protected virtual void OnPrimeSieveDoneEvent(PrimeSieveEventArgs e) => PrimeSieveDoneEvent?.Invoke(this, e);
    protected virtual void OnPrimeNoEvent(PrimeNoEventArgs e) => PrimeNoEvent?.Invoke(this, e);
}
