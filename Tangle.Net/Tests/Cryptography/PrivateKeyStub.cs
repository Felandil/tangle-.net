namespace Tangle.Net.Tests.Cryptography
{
  using System.Collections.Generic;
  using System.Linq;

  using Tangle.Net.Source.Cryptography;
  using Tangle.Net.Source.Entity;

  /// <summary>
  /// The private key stub.
  /// </summary>
  internal class PrivateKeyStub : IPrivateKey
  {
    #region Public Methods and Operators

    /// <summary>
    /// The sign input transactions.
    /// </summary>
    /// <param name="transactions">
    /// The transactions.
    /// </param>
    /// <param name="startIndex">
    /// The start index.
    /// </param>
    public void SignInputTransactions(List<Transaction> transactions, int startIndex)
    {
      for (var i = startIndex; i < transactions.Count(); i++)
      {
        transactions[i].SignatureFragment = "SOMESIGNATUREFRAGMENTWILLBEGENERATEDHERE";
      }
    }

    #endregion
  }
}