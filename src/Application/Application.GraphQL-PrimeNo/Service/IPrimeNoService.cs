using Application.GraphQL_PrimeNo.Schema;

namespace Application.GraphQL_PrimeNo.Service;

public interface IPrimeNoService
{
    int[] GetPrimeNoDataMessage(int? low = 2, int? high = int.MaxValue);
    PrimeSieveMessage PostPrimeSieveMessage(InputPrimeSieveMessage inputPrimeSieveMessage);
    IObservable<Event> SubscribeEvents();
    IObservable<EventValueData> SubscribeCurrentPrimeNo();
    IObservable<EventArrayData> SubscribePrimeSieveDataEvent(int sieveId);

    IObservable<EventArrayData> SubscribePrimeSieveDataEvent();
}
