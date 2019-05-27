namespace Tangle.Net.Zmq
{
  using System;
  using System.Threading;
  using System.Threading.Tasks;

  using Tangle.Net.Zmq.Events;

  using ZeroMQ;

  public static class ZmqIriListener
  {
    public static event EventHandler<IriCacheInformationEventArgs> IriCacheInformation;

    public static event EventHandler<NeighborDnsConfirmationsEventArgs> NeighborDnsConfirmations;

    public static event EventHandler<NeighborDnsValidationsEventArgs> NeighborDnsValidations;

    public static event EventHandler<TipTransactionRequesterEventArgs> TipTransactionRequester;

    public static event EventHandler<TransactionConfirmedEventArgs> TransactionConfirmed;

    public static event EventHandler<TransactionsTraversedEventArgs> TransactionsTraversed;

    public static event EventHandler<TransactionTrytesReceivedEventArgs> TransactionTrytesReceived;

    public static event EventHandler<UpdateToNeighborsIpEventArgs> UpdateToNeighborsIp;

    public static CancellationTokenSource Listen(string uri, string messageType)
    {
      var source = new CancellationTokenSource();
      var token = source.Token;

      Task.Factory.StartNew(
        () =>
          {
            using (var context = new ZContext())
            {
              using (var socket = new ZSocket(context, ZSocketType.SUB))
              {
                socket.Subscribe(messageType);
                socket.Connect(uri);

                while (!token.IsCancellationRequested)
                {
                  var message = socket.ReceiveMessage();
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
      var splitMessage = message.Split(' ');
      var sourceName = "ZmqIriListener";

      switch (splitMessage[0])
      {
        case MessageType.TransactionTrytes:
          TransactionTrytesReceived?.Invoke(sourceName, new TransactionTrytesReceivedEventArgs(message));
          break;
        case MessageType.TransactionConfirmed:
          TransactionConfirmed?.Invoke(sourceName, new TransactionConfirmedEventArgs(message));
          break;
        case MessageType.TransactionsTraversed:
          TransactionsTraversed?.Invoke(sourceName, new TransactionsTraversedEventArgs(message));
          break;
        case MessageType.NeighborDnsValidations:
          NeighborDnsValidations?.Invoke(sourceName, new NeighborDnsValidationsEventArgs(message));
          break;
        case MessageType.NeighborDnsConfirmations:
          NeighborDnsConfirmations?.Invoke(sourceName, new NeighborDnsConfirmationsEventArgs(message));
          break;
        case MessageType.UpdateToNeighborsIp:
          UpdateToNeighborsIp?.Invoke(sourceName, new UpdateToNeighborsIpEventArgs(message));
          break;
        case MessageType.IriCacheInformation:
          IriCacheInformation?.Invoke(sourceName, new IriCacheInformationEventArgs(message));
          break;
        case MessageType.TipTransactionRequester:
          TipTransactionRequester?.Invoke(sourceName, new TipTransactionRequesterEventArgs(message));
          break;
        default:
          break;
      }
    }
  }
}