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
      var repository = new RestIotaRepository(new RestClient("https://field.deviota.com:443"), new PoWSrvService());

      var bundle = new Bundle();
      bundle.AddTransfer(new Transfer
                           {
                             Address = new Address(Hash.Empty.Value),
                             Tag = new Tag("MYCSHARPPI"),
                             Timestamp = Timestamp.UnixSecondsTimestamp,
                             Message = TryteString.FromUtf8String("Hello from PiDiver #1!")
                           });

      bundle.AddTransfer(new Transfer
                           {
                             Address = new Address(Hash.Empty.Value),
                             Tag = new Tag("MYCSHARPPI"),
                             Timestamp = Timestamp.UnixSecondsTimestamp,
                             Message = TryteString.FromUtf8String("Hello from PiDiver #2!")
                           });

      bundle.Finalize();
      bundle.Sign();

      repository.SendTrytes(bundle.Transactions);


      Console.WriteLine("Done");
      Console.ReadKey();
    }

    #endregion
  }
}