namespace Tangle.Net.Zmq.Events
{
  public class NeighborDnsValidationsEventArgs : ZmqEventArgs
  {
    private string neighborHostname;

    private string neighborIpAddress;

    public NeighborDnsValidationsEventArgs(string message)
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

    public string NeighborIpAddress
    {
      get
      {
        if (string.IsNullOrEmpty(this.neighborIpAddress))
        {
          this.neighborIpAddress = this.Message.Split(' ')[2];
        }

        return this.neighborIpAddress;
      }
    }
  }
}