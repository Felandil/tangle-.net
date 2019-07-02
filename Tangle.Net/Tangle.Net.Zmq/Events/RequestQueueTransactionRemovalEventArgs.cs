namespace Tangle.Net.Zmq.Events
{
  using Tangle.Net.Entity;

  public class RequestQueueTransactionRemovalEventArgs : ZmqEventArgs
  {
    private Hash removedTransactionHash;

    /// <inheritdoc />
    public RequestQueueTransactionRemovalEventArgs(string message)
      : base(message)
    {
    }

    public Hash RemovedTransactionHash
    {
      get
      {
        if (this.removedTransactionHash == null)
        {
          this.removedTransactionHash = new Hash(this.Message.Split(' ')[1]);
        }

        return this.removedTransactionHash;
      }
    }
  }
}