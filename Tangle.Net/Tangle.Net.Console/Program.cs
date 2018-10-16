namespace Tangle.Net.Console
{
  using System;
  using System.Collections.Generic;
  using System.Diagnostics;
  using System.Linq;

  using RestSharp;

  using Tangle.Net.Cryptography;
  using Tangle.Net.Cryptography.Curl;
  using Tangle.Net.Cryptography.Signing;
  using Tangle.Net.Entity;
  using Tangle.Net.Mam.Entity;
  using Tangle.Net.Mam.Merkle;
  using Tangle.Net.Mam.Services;
  using Tangle.Net.ProofOfWork;
  using Tangle.Net.ProofOfWork.Service;
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
      var repository = new RestIotaRepository(new RestClient("https://field.deviota.com:443"));

      var nodeInfo = repository.GetNodeInfo();
      var transactionHash = new Hash("QJSQBHNSVOEYZSWRCBXJTPEBW9PT9GRLYZFJYT9AFJTUXJLLBRUTNQJUKARETCWPHFGXCTYDTSOU99999");

      var inclusionStates = repository.GetInclusionStates(
        new List<Hash> { transactionHash },
        new List<Hash> { new Hash(nodeInfo.LatestSolidSubtangleMilestone) });

      Console.WriteLine($"Transaction confirmed: {inclusionStates.States.FirstOrDefault(s => s.Key.Value == transactionHash.Value).Value}");

      var transactionTrytes = repository.GetTrytes(new List<Hash> { transactionHash });
      var transaction = Transaction.FromTrytes(transactionTrytes.First());

      Console.WriteLine(transaction.Fragment.ToUtf8String());

      Console.WriteLine("Done");
      Console.ReadKey();
    }

    #endregion
  }
}