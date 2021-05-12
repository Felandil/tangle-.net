namespace Tangle.Net.Api.HighLevel.Response
{
  public class RetrieveDataResponse
  {
    public RetrieveDataResponse(string indexationKey, string data)
    {
      IndexationKey = indexationKey;
      Data = data;
    }

    public string IndexationKey { get; }
    public string Data { get; }
  }
}