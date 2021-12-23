namespace Tangle.Net.Api.HighLevel.Response
{
  public class GetBalanceResponse
  {
    public GetBalanceResponse(long balance)
    {
      Balance = balance;
    }

    public long Balance { get; }
  }
}