using System.Reactive.Linq;
using System.Reactive.Subjects;
using Application.GraphQL_PrimeNo.Schema;
using Application.PrimeNumbers.Service;
using Microsoft.Extensions.Logging;

namespace Application.GraphQL_PrimeNo.Service;
public class PrimeNoService : IDisposable, IPrimeNoService
{
    private readonly IConsumerProducerPrimesService _consumerProducerPrimesService;
    private readonly ILogger<PrimeNoService> _logger;
    private readonly Subject<Event> _broadcaster = new();
    public PrimeNoService(IConsumerProducerPrimesService consumerProducerPrimesService, ILogger<PrimeNoService> logger)
    {
        _consumerProducerPrimesService = consumerProducerPrimesService;
        _consumerProducerPrimesService.PrimeNoEvent += _consumerProducerPrimesService_PrimeNoEvent;
        _consumerProducerPrimesService.PrimeSieveDoneEvent += _consumerProducerPrimesService_PrimeSieveDoneEvent;
        _logger = logger;
    }
    private void _consumerProducerPrimesService_PrimeSieveDoneEvent(object? sender, PrimeSieveEventArgs e) =>
        _broadcaster.OnNext(new Event() { ArrayData = new EventArrayData(e.SieveId, e.PrimeNoData) });

    private void _consumerProducerPrimesService_PrimeNoEvent(object? sender, PrimeNoEventArgs e) =>
        _broadcaster.OnNext(new Event
        {
            ValueData = new EventValueData(e.PrimeNo)
        });

    public int[] GetPrimeNoDataMessage(int? low = 2, int? high = int.MaxValue) =>
        _consumerProducerPrimesService.GetPrimeNoEnumerable().Where(x => x >= (low ?? 2) && x <= (high ?? int.MaxValue))
            .Select(x => x).ToArray();

    public PrimeSieveMessage PostPrimeSieveMessage(InputPrimeSieveMessage inputPrimeSieveMessage)
    {
        var primeSieve = _consumerProducerPrimesService.ProducePrimeSieve(inputPrimeSieveMessage.Amount);
        var primeSieveMessage = new PrimeSieveMessage()
        {
            PrimeSieveId = primeSieve.Id,
            Start = primeSieve.Start,
            End = primeSieve.End
        };
        return primeSieveMessage;
    }
    public IObservable<Event> SubscribeEvents() => _broadcaster;

    public IObservable<EventValueData> SubscribeCurrentPrimeNo() => _broadcaster
        .Where(x => x is { Type: EventType.NewMessage, ValueData: { } }).Select(x => x.ValueData!);

    public IObservable<EventArrayData> SubscribePrimeSieveDataEvent(int sieveId) => _broadcaster
        .Where(x => x is { Type: EventType.NewMessage, ArrayData: { } arrayData } && arrayData.PrimeSieveId == sieveId)
        .Select(x => x.ArrayData!);

    public IObservable<EventArrayData> SubscribePrimeSieveDataEvent() => _broadcaster
        .Where(x => x is { Type: EventType.NewMessage, ArrayData: { } })
        .Select(x => x.ArrayData!);

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            _broadcaster.Dispose();
            _consumerProducerPrimesService.PrimeNoEvent -= _consumerProducerPrimesService_PrimeNoEvent;
            _consumerProducerPrimesService.PrimeSieveDoneEvent -= _consumerProducerPrimesService_PrimeSieveDoneEvent;
        }
    }
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}
