namespace Tangle.Net.Zmq.Events
{
  using Tangle.Net.Entity;

  public class TransactionsEventArgs : ZmqEventArgs
  {
    private Transaction transaction;

    /// <inheritdoc />
    public TransactionsEventArgs(string message)
      : base(message)
    {
    }

    public Transaction Transaction
    {
      get
      {
        if (this.transaction != null)
        {
          return this.transaction;
        }

        var splitMessage = this.Message.Split(' ');
        this.transaction = new Transaction
                             {
                               Hash = new Hash(splitMessage[1]),
                               Address = new Address(splitMessage[2]),
                               Value = long.Parse(splitMessage[3]),
                               ObsoleteTag = new Tag(splitMessage[4]),
                               Timestamp = long.Parse(splitMessage[5]),
                               CurrentIndex = int.Parse(splitMessage[6]),
                               LastIndex = int.Parse(splitMessage[7]),
                               BundleHash = new Hash(splitMessage[8]),
                               TrunkTransaction = new Hash(splitMessage[9]),
                               BranchTransaction = new Hash(splitMessage[10]),
                               Tag = new Tag(splitMessage[12])
                             };

        return this.transaction;
      }
    }
  }
}