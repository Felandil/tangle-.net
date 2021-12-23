namespace Tangle.Net.Api.HighLevel.Request
{
  public class MessageRequest
  {
    public MessageRequest(string messageId)
    {
      MessageId = messageId;
    }

    public string MessageId { get; }
  }
}