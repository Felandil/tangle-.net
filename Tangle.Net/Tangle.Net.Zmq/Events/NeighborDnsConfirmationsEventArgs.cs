namespace Tangle.Net.Zmq.Events
{
  public class NeighborDnsConfirmationsEventArgs : ZmqEventArgs
  {
    private string neighborHostname;

    /// <inheritdoc />
    public NeighborDnsConfirmationsEventArgs(string message)
      : base(message)
    {
    }

    public string NeighborHostname
    {
      get
      {
        if (string.IsNullOrEmpty(this.neighborHostname))
        {
          this.neighborHostname = this.Message.Split(' ')[1];
        }

        return this.neighborHostname;
      }
    }
  }
}