namespace Tangle.Net.Console
{
  using System;
  using System.Collections.Generic;
  using System.Linq;

  using RestSharp;

  using Tangle.Net.Source.Cryptography;
  using Tangle.Net.Source.Entity;
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
      var repository = new RestIotaRepository(new RestClient("http://iri1.iota.fm:80"));

      var tips = repository.GetTips();
      var inclusionsStates =
        repository.GetInclusionStates(
          new List<Hash> { new Hash("HG9KCXQZGQDVTFGRHOZDZ99RMKGVRIQXEKXWXTPWYRGXQQVFVMTLQLUPJSIDONDEURVKHMBPRYGP99999") },
          tips.Hashes.GetRange(0, 10));

      var transactionTrytes =
        repository.GetTrytes(new List<Hash> { new Hash("HG9KCXQZGQDVTFGRHOZDZ99RMKGVRIQXEKXWXTPWYRGXQQVFVMTLQLUPJSIDONDEURVKHMBPRYGP99999") });

      var transactionData = transactionTrytes.Select(t => Transaction.FromTrytes(t)).ToList();

      var transactionsToApprove = repository.GetTransactionsToApprove();

      var transactions =
        repository.FindTransactionsByAddresses(
          new List<Address> { new Address("GVZSJANZQULQICZFXJHHAFJTWEITWKQYJKU9TYFA9AFJLVIYOUCFQRYTLKRGCVY9KPOCCHK99TTKQGXA9") });

      var balances =
        repository.GetBalances(
          new List<Address>
            {
              new Address("GVZSJANZQULQICZFXJHHAFJTWEITWKQYJKU9TYFA9AFJLVIYOUCFQRYTLKRGCVY9KPOCCHK99TTKQGXA9"),
              new Address("HBBYKAKTILIPVUKFOTSLHGENPTXYBNKXZFQFR9VQFWNBMTQNRVOUKPVPRNBSZVVILMAFBKOTBLGLWLOHQ999999999")
            });

      var nodeInfo = repository.GetNodeInfo();

      var neighbours = repository.GetNeighbors();

      Console.WriteLine("Done");
      Console.ReadKey();
    }

    #endregion
  }
}