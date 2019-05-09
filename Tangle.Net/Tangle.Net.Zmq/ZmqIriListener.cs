namespace Tangle.Net.Zmq
{
  using System;
  using System.Threading;
  using System.Threading.Tasks;

  using Tangle.Net.Entity;
  using Tangle.Net.Zmq.Events;

  using ZeroMQ;

  public static class ZmqIriListener
  {
    public static event EventHandler<TransactionTrytesReceivedEventArgs> TransactionTrytesReceived;

    public static CancellationTokenSource Listen(string uri, string messageType)
    {
      var source = new CancellationTokenSource();
      var token = source.Token;

      Task.Factory.StartNew(
        () =>
          {
            using (var context = new ZContext())
            {
              using (var requester = new ZSocket(context, ZSocketType.SUB))
              {
                requester.Subscribe(messageType);
                requester.Connect(uri);

                while (!token.IsCancellationRequested)
                {
                  var message = requester.ReceiveMessage();
                  HandleMessage(message.PopString());
                }
              }
            }
          },
        token);

      return source;
    }

    private static void HandleMessage(string message)
    {
      var dividedMessage = message.Split(' ');

      switch (dividedMessage[0])
      {
        case MessageType.TransactionTrytes:
          TransactionTrytesReceived?.Invoke(
            "ZmqIriListener",
            new TransactionTrytesReceivedEventArgs(new TransactionTrytes(dividedMessage[1])));
          break;
        default:
          break;
      }
    }
  }
}