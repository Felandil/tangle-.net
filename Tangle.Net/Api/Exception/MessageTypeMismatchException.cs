using Tangle.Net.Entity.Message.Payload;

namespace Tangle.Net.Api.Exception
{
  public class MessageTypeMismatchException : System.Exception
  {
    public MessageTypeMismatchException(Payload expected) : base($"Type mismatch. Message is not of type {expected}. Please check if the payload you specified is correct.")
    {
    }
  }
}