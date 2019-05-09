namespace Tangle.Net.Zmq.Events
{
  using System;

  using Tangle.Net.Entity;

  public class TransactionTrytesReceivedEventArgs : EventArgs
  {
    private Transaction transaction;

    public TransactionTrytesReceivedEventArgs(TransactionTrytes transactionTrytes)
    {
      this.TransactionTrytes = transactionTrytes;
    }

    public Transaction Transaction => this.transaction ?? (this.transaction = Transaction.FromTrytes(this.TransactionTrytes));

    public TransactionTrytes TransactionTrytes { get; }

    public static TransactionTrytesReceivedEventArgs FromZmqMessage(string message)
    {
      return new TransactionTrytesReceivedEventArgs(new TransactionTrytes(message.Split(' ')[1]));
    }
  }
}