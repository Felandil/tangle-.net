namespace Tangle.Net.Console
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Net;

  using RestSharp;

  using Tangle.Net.Source.Cryptography;
  using Tangle.Net.Source.Entity;
  using Tangle.Net.Source.Repository;
  using Tangle.Net.Source.Utils;

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
      var repository = new RestIotaRepository(new RestClient("http://localhost:14265"));

      var latestInclusion =
        repository.GetLatestInclusion(
          new List<Hash> { new Hash("HG9KCXQZGQDVTFGRHOZDZ99RMKGVRIQXEKXWXTPWYRGXQQVFVMTLQLUPJSIDONDEURVKHMBPRYGP99999") });

      var inputs = repository.GetInputs(new Seed("SOMESEEDHERE"), 99900000, SecurityLevel.Medium, 0);

      var newAddresses = repository.GetNewAddresses(Seed.Random(), 0, 5, SecurityLevel.Medium);

      var transactions =
        repository.FindTransactionsByAddresses(
          new List<Address> { new Address("HHZSJANZQULQICZFXJHHAFJTWEITWKQYJKU9TYFA9AFJLVIYOUCFQRYTLKRGCVY9KPOCCHK99TTKQGXA9") });

      var tips = repository.GetTips();
      var inclusionsStates =
        repository.GetInclusionStates(
          new List<Hash> { new Hash("HG9KCXQZGQDVTFGRHOZDZ99RMKGVRIQXEKXWXTPWYRGXQQVFVMTLQLUPJSIDONDEURVKHMBPRYGP99999") },
          tips.Hashes.GetRange(0, 10));

      var transactionTrytes =
        repository.GetTrytes(new List<Hash> { new Hash("HG9KCXQZGQDVTFGRHOZDZ99RMKGVRIQXEKXWXTPWYRGXQQVFVMTLQLUPJSIDONDEURVKHMBPRYGP99999") });

      var transactionData = transactionTrytes.Select(t => Transaction.FromTrytes(t)).ToList();

      var transactionsToApprove = repository.GetTransactionsToApprove();

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