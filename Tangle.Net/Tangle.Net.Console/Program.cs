// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Program.cs" company="Felandil IT">
//    Copyright (c) 2008 -2017 Felandil IT. All rights reserved.
//  </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Tangle.Net.Console
{
  using System;
  using System.Collections.Generic;

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
      var repository = new RestIotaRepository("https://nodes.iota.cafe:443");
      var nodeInfo = repository.GetNodeInfo();
      var neighbours = repository.GetNeighbors();
      var transactions = repository.GetTransactionsByAddresses(new List<string> { "GVZSJANZQULQICZFXJHHAFJTWEITWKQYJKU9TYFA9AFJLVIYOUCFQRYTLKRGCVY9KPOCCHK99TTKQGXA9" });
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