using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tangle.Net.Examples.Examples.Zmq
{
  using System.Diagnostics;
  using System.Threading;

  using Tangle.Net.Zmq;

  public class ZmqTransactionListenerExample
  {
    public static void Execute()
    {
      // Subscribe to transactions event
      // see https://docs.iota.org/docs/iri/0.1/references/zmq-events for all events
      ZmqIriListener.Transactions += (sender, eventArgs) =>
        {
          Console.WriteLine("-----------------------");
          Console.WriteLine(eventArgs.Transaction.Hash);
          Console.WriteLine(eventArgs.Transaction.Address);
        };

      // start listening to the event type (use MessageType.All) to subscribe to all events
      var tokenSource = ZmqIriListener.Listen("tcp://trinity.iota-tangle.io:5556", MessageType.Transactions);

      // listen for 60 seconds
      var stopwatch = new Stopwatch();
      stopwatch.Start();

      while (stopwatch.ElapsedMilliseconds < 600000)
      {
        Thread.Sleep(100);
      }

      // cancel the thread
      tokenSource.Cancel();
    }
  }
}
