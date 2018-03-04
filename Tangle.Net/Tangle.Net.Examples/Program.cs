namespace Tangle.Net.Examples
{
  using System.Configuration;
  using System.Web.Configuration;

  using RestSharp;

  using Tangle.Net.Examples.Examples;
  using Tangle.Net.ProofOfWork;
  using Tangle.Net.Repository;
  using Tangle.Net.Repository.Client;

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
      var nodeUri = ConfigurationManager.AppSettings["nodeUri"]; // The node URI is saved in the projects App.config ... let's get it 

      var iotaClient = new RestIotaClient(new RestClient(nodeUri));
      var powService = new PoWService(new CpuPowDiver()); // the examples will use the CPU to do PoW for a transaction.

      // you could also use a remote node for PoW, if the node supports it. Uncomment this line and comment the line above to do so. Don't forget to change the node Uri in App.Config
      // var powService = new RestPoWService(iotaClient); 

      var repository = new RestIotaRepository(iotaClient, powService); // compose the repository. all these steps would normally be done via DI container (ninject for example)

      var example = new SendTrytesExample(repository); // get the example to execute. Change this to any example you want

      example.Execute();
    }
  }
}