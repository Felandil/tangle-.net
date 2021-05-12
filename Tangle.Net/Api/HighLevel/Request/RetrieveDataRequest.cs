namespace Tangle.Net.Api.HighLevel.Request
{
  public class RetrieveDataRequest
  {
    public RetrieveDataRequest(string messageId)
    {
      MessageId = messageId;
    }

    public string MessageId { get; }
  }
}