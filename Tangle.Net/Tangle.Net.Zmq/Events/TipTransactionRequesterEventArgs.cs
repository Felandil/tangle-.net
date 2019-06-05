namespace Tangle.Net.Zmq.Events
{
  public class TipTransactionRequesterEventArgs : ZmqEventArgs
  {
    private int storedLedgerTransactions;

    private int tipTransactionsRequested;

    private int tipTransactionsToBroadcast;

    private int tipTransactionsToProcess;

    private int tipTransactionsToRequest;

    /// <inheritdoc />
    public TipTransactionRequesterEventArgs(string message)
      : base(message)
    {
    }

    public int StoredLedgerTransactions
    {
      get
      {
        if (this.storedLedgerTransactions == 0)
        {
          this.ParseMessage();
        }

        return this.storedLedgerTransactions;
      }
    }

    public int TipTransactionsRequested
    {
      get
      {
        if (this.tipTransactionsRequested == 0)
        {
          this.ParseMessage();
        }

        return this.tipTransactionsRequested;
      }
    }

    public int TipTransactionsToBroadcast
    {
      get
      {
        if (this.tipTransactionsToBroadcast == 0)
        {
          this.ParseMessage();
        }

        return this.tipTransactionsToBroadcast;
      }
    }

    public int TipTransactionsToProcess
    {
      get
      {
        if (this.tipTransactionsToProcess == 0)
        {
          this.ParseMessage();
        }

        return this.tipTransactionsToProcess;
      }
    }

    public int TipTransactionsToRequest
    {
      get
      {
        if (this.tipTransactionsToRequest == 0)
        {
          this.ParseMessage();
        }

        return this.tipTransactionsToRequest;
      }
    }

    private void ParseMessage()
    {
      var splitMessage = this.Message.Split(' ');

      this.tipTransactionsToProcess = int.Parse(splitMessage[1]);
      this.tipTransactionsToBroadcast = int.Parse(splitMessage[2]);
      this.tipTransactionsToRequest = int.Parse(splitMessage[3]);
      this.tipTransactionsRequested = int.Parse(splitMessage[4]);
      this.storedLedgerTransactions = int.Parse(splitMessage[5]);
    }
  }
}