namespace Tangle.Net.Examples
{
  using System;
  using System.Configuration;
  using System.Diagnostics;
  using System.Threading;
  using System.Threading.Tasks;
  using System.Web.Configuration;

  using RestSharp;

  using Tangle.Net.Entity;
  using Tangle.Net.Examples.Examples;
  using Tangle.Net.Examples.Examples.Api;
  using Tangle.Net.Examples.Examples.Mam;
  using Tangle.Net.ProofOfWork;
  using Tangle.Net.Repository;
  using Tangle.Net.Repository.Client;
  using Tangle.Net.Repository.Factory;
  using Tangle.Net.Zmq;
  using Tangle.Net.Zmq.Events;

  /// <summary>
  /// The program.
  /// </summary>
  internal class Program
  {
    /// <summary>
    /// The main.
    /// </summary>
    /// <param name="args">
    /// The args.
    /// </param>
    internal static void Main(string[] args)
    {
      ZmqIriListener.TransactionTrytesReceived += (sender, eventArgs) =>
        {
          //Console.WriteLine(eventArgs.TransactionTrytes.Value);
          Console.WriteLine("-----------------------");
          Console.WriteLine(eventArgs.Transaction.Hash);
        };

      var tokenSource = ZmqIriListener.Listen("tcp://zmq.devnet.iota.org:5556", MessageType.TransactionTrytes);

      var stopwatch = new Stopwatch();
      stopwatch.Start();

      while (stopwatch.ElapsedMilliseconds < 60000)
      {
        Thread.Sleep(100);
      }

      tokenSource.Cancel();

      //ExecuteApiExample();

      // new MamFlowExample().Execute().Wait();
      // new MamFetchExample().Execute().Wait();
      // new MamPublishExample().Execute().Wait();

      Console.ReadKey();
    }

    /// <summary>
    /// The execute api example.
    /// </summary>
    private static void ExecuteApiExample()
    {
      var nodeUri = ConfigurationManager.AppSettings["nodeUri"]; // The node URI is saved in the projects App.config ... let's get it 

      var iotaClient = new RestIotaClient(new RestClient(nodeUri));
      var powService = new PoWService(new CpuPearlDiver()); // the examples will use the CPU to do PoW for a transaction.

      // you could also use a remote node for PoW, if the node supports it. Uncomment this line and comment the line above to do so. Don't forget to change the node Uri in App.Config
      // var powService = new RestPoWService(iotaClient); 

      var repository = new RestIotaRepository(
        iotaClient,
        powService); // compose the repository. all these steps would normally be done via DI container (ninject for example)

      // var factory = new RestIotaRepositoryFactory(); In an async context it is also possible to create a repository via factory as shown here. 
      // var repository = await factory.CreateAsync(); This automatically picks a healthy node.

      var example = new GetAccountDataExample(Utils.Repository); // get the example to execute. Change this to any example you want

      var result = example.Execute();

      Console.WriteLine(result); // print the example result to console. Set a breakpoint here if you want to see the result in debug mode.
    }
  }
}