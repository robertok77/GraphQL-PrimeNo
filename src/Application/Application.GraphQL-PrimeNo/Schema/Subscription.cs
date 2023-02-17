using Application.GraphQL_PrimeNo.Service;
using GraphQL;

namespace Application.GraphQL_PrimeNo.Schema;

public class Subscription
{
    public static IObservable<EventArrayData> LatestPrimeSieveListAdded([FromServices] IPrimeNoService primeNoService, int? id = null)
        => id == null ? primeNoService.SubscribePrimeSieveDataEvent() : primeNoService.SubscribePrimeSieveDataEvent(id.Value);

    public static IObservable<EventValueData> PrimeNumberAdding([FromServices] IPrimeNoService primeNoService)
        => primeNoService.SubscribeCurrentPrimeNo();


    public static IObservable<Event> Events([FromServices] IPrimeNoService primeNoService)
        => primeNoService.SubscribeEvents();
}
