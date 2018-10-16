namespace Tangle.Net.Repository.Client
{
  using System;

  public class CircuitOpenException : Exception
  {
    public CircuitOpenException(string message)
      : base(message)
    {
    }
  }
}