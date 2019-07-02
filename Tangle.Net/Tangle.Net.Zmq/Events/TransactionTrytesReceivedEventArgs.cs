namespace Tangle.Net.Zmq.Events
{
  using System;

  using Tangle.Net.Entity;

  public class TransactionTrytesReceivedEventArgs : ZmqEventArgs
  {
    private Transaction transaction;

    private TransactionTrytes transactionTrytes;

    public TransactionTrytesReceivedEventArgs(string message) : base(message)
    {
    }

    public Transaction Transaction => this.transaction ?? (this.transaction = Transaction.FromTrytes(this.TransactionTrytes));

    public TransactionTrytes TransactionTrytes =>
      this.transactionTrytes ?? (this.transactionTrytes = new TransactionTrytes(this.Message.Split(' ')[1]));
  }
}