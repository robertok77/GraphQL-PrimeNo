using Microsoft.Extensions.Hosting;

namespace Application.PrimeNumbers.Service;

public class EratosthenesSieveBackgroudService : BackgroundService
{
    private readonly IConsumerProducerPrimesService _service;
    public EratosthenesSieveBackgroudService(IConsumerProducerPrimesService service) => _service = service;
    protected override Task ExecuteAsync(CancellationToken stoppingToken) => _service.ConsumePrimeSieveAsync(stoppingToken);
}
