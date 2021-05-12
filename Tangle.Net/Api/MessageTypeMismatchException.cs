using System;
using Tangle.Net.Entity.Message.Payload;

namespace Tangle.Net.Api
{
  public class MessageTypeMismatchException : Exception
  {
    public MessageTypeMismatchException(Payload expected) : base($"Type mismatch. Message is not of type {expected}. Please check if the payload you specified is correct.")
    {
    }
  }
}