namespace Tangle.Net.Zmq.Events
{
  using System;

  public class ZmqEventArgs : EventArgs
  {
    public ZmqEventArgs(string message)
    {
      this.Message = message;
    }

    public string Message { get; }
  }
}