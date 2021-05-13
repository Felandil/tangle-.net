namespace Tangle.Net.Api.Exception
{
  public class MessageNotRetriableException : System.Exception
  {
    public MessageNotRetriableException() : base("Message must not be attached or promoted")
    {
    }
  }
}