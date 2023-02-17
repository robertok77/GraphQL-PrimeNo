
namespace Application.GraphQL_PrimeNo.Schema;

public class InputPrimeSieveMessage
{
    public int Amount { get; set; }
}
public class PrimeSieveMessage
{
    public int PrimeSieveId { get; set; }
    public int Start { get; set; }
    public int End { get; set; }
}
