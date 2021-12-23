namespace Tangle.Net.Api.Exception
{
  public class MessageNotFoundException : System.Exception
  {
    public MessageNotFoundException(string messageId) : base($"Message with id {messageId} not found")
    {
    }
  }
}