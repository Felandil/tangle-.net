namespace Tangle.Net.Console
{
  using System;
  using System.Collections.Generic;
  using System.Diagnostics;
  using System.Linq;

  using RestSharp;

  using Tangle.Net.Cryptography;
  using Tangle.Net.Entity;
  using Tangle.Net.ProofOfWork;
  using Tangle.Net.Repository;
  using Tangle.Net.Repository.Factory;
  using Tangle.Net.Utils;

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
      var factory = new RestIotaRepositoryFactory();
      var repo = factory.CreateAsync(true).Result;
      var repository = new RestIotaRepository(new RestClient("http://localhost:14265"), new PoWService(new CpuPowDiver()));
      var acc = repository.GetAccountData(Seed.Random(), true, SecurityLevel.Medium, 0);

      var seed = Seed.Random();
      var addressGenerator = new AddressGenerator(seed);

      var addresses = addressGenerator.GetAddresses(0, 6);
      var addressesWithSpentInformation = repository.WereAddressesSpentFrom(addresses);

      var transactionStackCounter = 10;

      for (var i = 1; i <= transactionStackCounter; i++)
      {
        var stopwatch = new Stopwatch();
        stopwatch.Start();
        var transfers = new List<Transfer>();
        for (var j = 1; j <= i; j++)
        {
          transfers.Add(new Transfer
          {
            Address =
              new Address("YTXCUUWTXIXVRQIDSECVFRTKAFOEZITGDPLWYVUVFURMNVDPIRXEIQN9JHNFNVKVJMQVMA9GDZJROTSFZHIVJOVAEC") { Balance = 0 },
            Message = TryteString.FromAsciiString("Hello world! With " + i + " transactions."),
            Tag = new Tag("CSHARP"),
            Timestamp = Timestamp.UnixSecondsTimestamp
          });
        }

        var bundle = new Bundle();

        transfers.ForEach(bundle.AddTransfer);

        bundle.Finalize();
        bundle.Sign();

        var resultTransactions = repository.SendTrytes(bundle.Transactions, 27, 14);
        Console.WriteLine("Finished sending bundle with {0} transactions. Time elapsed: {1} seconds.", i, stopwatch.ElapsedMilliseconds / 1000);
      }


      var accountData = repository.GetAccountData(new Seed("SOMESEEDHERE"), true, SecurityLevel.Medium, 0);

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