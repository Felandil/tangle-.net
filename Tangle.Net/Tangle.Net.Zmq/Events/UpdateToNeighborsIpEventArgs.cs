namespace Tangle.Net.Zmq.Events
{
  public class UpdateToNeighborsIpEventArgs : ZmqEventArgs
  {
    private string neighborHostname;

    /// <inheritdoc />
    public UpdateToNeighborsIpEventArgs(string message)
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