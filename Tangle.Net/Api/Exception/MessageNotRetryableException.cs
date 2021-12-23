namespace Tangle.Net.Api.Exception
{
  public class MessageNotRetryableException : System.Exception
  {
    public MessageNotRetryableException() : base("Message must not be attached or promoted")
    {
    }
  }
}