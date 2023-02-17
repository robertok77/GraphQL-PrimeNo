namespace Application.GraphQL_PrimeNo.Schema;

public class Event
{
    public EventType Type { get; set; } = EventType.NewMessage;
    public EventArrayData? ArrayData { get; set; }
    public EventValueData? ValueData { get; set; }
}

public record EventData();

public record EventArrayData(int PrimeSieveId, int[]? PrimeNumberList);

public record EventValueData(int PrimeNo);

public enum EventType
{
    NewMessage,
    DeleteMessage,
    ClearMessages,
}
