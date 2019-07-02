namespace Tangle.Net.Zmq.Events
{
  using Tangle.Net.Entity;

  public class TransactionConfirmedEventArgs : ZmqEventArgs
  {
    private Address address;

    private Hash branch;

    private Hash bundleHash;

    private Hash hash;

    private int milestone;

    private Hash trunk;

    public TransactionConfirmedEventArgs(string message)
      : base(message)
    {
    }

    public Address Address
    {
      get
      {
        if (this.address == null)
        {
          this.ParseMessage();
        }

        return this.address;
      }
    }

    public Hash Branch
    {
      get
      {
        if (this.branch == null)
        {
          this.ParseMessage();
        }

        return this.branch;
      }
    }

    public Hash BundleHash
    {
      get
      {
        if (this.bundleHash == null)
        {
          this.ParseMessage();
        }

        return this.bundleHash;
      }
    }

    public Hash Hash
    {
      get
      {
        if (this.hash == null)
        {
          this.ParseMessage();
        }

        return this.hash;
      }
    }

    public int Milestone
    {
      get
      {
        if (this.milestone == 0)
        {
          this.ParseMessage();
        }

        return this.milestone;
      }
    }

    public Hash Trunk
    {
      get
      {
        if (this.trunk == null)
        {
          this.ParseMessage();
        }

        return this.trunk;
      }
    }

    private void ParseMessage()
    {
      var splitMessage = this.Message.Split(' ');

      this.address = new Address(splitMessage[3]);
      this.branch = new Hash(splitMessage[5]);
      this.bundleHash = new Hash(splitMessage[6]);
      this.hash = new Hash(splitMessage[2]);
      this.milestone = int.Parse(splitMessage[1]);
      this.trunk = new Hash(splitMessage[4]);
    }
  }
}