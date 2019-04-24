namespace Tangle.Net.Console
{
  using System;
  using System.Collections.Generic;
  using System.IO;
  using System.Linq;

  using Tangle.Net.Entity;
  using Tangle.Net.ProofOfWork.Service;
  using Tangle.Net.Repository;
  using Tangle.Net.Repository.Client;
  using Tangle.Net.Utils;

  /// <summary>
  /// The program.
  /// </summary>
  internal static class Program
  {
    /// <summary>
    /// The main.
    /// </summary>
    /// <param name="args">
    /// The args.
    /// </param>
    private static void Main(string[] args)
    {
      var repository = new RestIotaRepository(
        new FallbackIotaClient(
          new List<string>
            {
              "https://nodes.devnet.thetangle.org:443",
              "http://node04.iotatoken.nl:14265",
              "http://node05.iotatoken.nl:16265",
              "https://nodes.thetangle.org:443",
              "http://iota1.heidger.eu:14265",
              "https://nodes.iota.cafe:443",
              "https://potato.iotasalad.org:14265",
              "https://durian.iotasalad.org:14265",
              "https://turnip.iotasalad.org:14265",
              "https://nodes.iota.fm:443",
              "https://tuna.iotasalad.org:14265",
              "https://iotanode2.jlld.at:443",
              "https://node.iota.moe:443",
              "https://wallet1.iota.town:443",
              "https://wallet2.iota.town:443",
              "http://node03.iotatoken.nl:15265",
              "https://node.iota-tangle.io:14265",
              "https://pow4.iota.community:443",
              "https://dyn.tangle-nodes.com:443",
              "https://pow5.iota.community:443",
            },
          5000),
        new PoWSrvService());

      try
      {
        repository.AddNeighbors(new List<string> { "udp://8.8.8.8:14265" });
      }
      catch (IotaApiException e)
      {

      }


      Console.WriteLine("Done");
      Console.ReadKey();
    }
  }
}