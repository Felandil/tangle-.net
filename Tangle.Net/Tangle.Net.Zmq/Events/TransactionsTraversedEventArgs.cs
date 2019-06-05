namespace Tangle.Net.Zmq.Events
{
  public class TransactionsTraversedEventArgs : ZmqEventArgs
  {
    private int transactionsTraversed;

    public TransactionsTraversedEventArgs(string message)
      : base(message)
    {
    }

    public int TransactionsTraversed
    {
      get
      {
        if (this.transactionsTraversed == 0)
        {
          this.transactionsTraversed = int.Parse(this.Message.Split(' ')[1]);
        }

        return this.transactionsTraversed;
      }
    }
  }
}