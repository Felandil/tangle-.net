namespace Tangle.Net.Console
{
  using System;
  using System.Collections.Generic;

  using RestSharp;

  using Tangle.Net.Source.Repository;

  /// <summary>
  /// The program.
  /// </summary>
  internal static class Program
  {
    #region Methods

    /// <summary>
    /// The main.
    /// </summary>
    /// <param name="args">
    /// The args.
    /// </param>
    private static void Main(string[] args)
    {
      var repository = new RestIotaRepository(new RestClient("https://localhost:14265"));
      var nodeInfo = repository.GetNodeInfo();
      var neighbours = repository.GetNeighbors();
      var transactions =
        repository.GetTransactionsByAddresses(
          new List<string> { "GVZSJANZQULQICZFXJHHAFJTWEITWKQYJKU9TYFA9AFJLVIYOUCFQRYTLKRGCVY9KPOCCHK99TTKQGXA9" });
      var balances =
        repository.GetBalances(
          new List<string>
            {
              "GVZSJANZQULQICZFXJHHAFJTWEITWKQYJKU9TYFA9AFJLVIYOUCFQRYTLKRGCVY9KPOCCHK99TTKQGXA9", 
              "HBBYKAKTILIPVUKFOTSLHGENPTXYBNKXZFQFR9VQFWNBMTQNRVOUKPVPRNBSZVVILMAFBKOTBLGLWLOHQ999999999"
            }, 
          100);

      Console.WriteLine("Done");
      Console.ReadKey();
    }

    #endregion
  }
}