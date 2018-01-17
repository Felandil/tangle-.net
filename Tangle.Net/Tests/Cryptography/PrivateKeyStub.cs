namespace Tangle.Net.Tests.Cryptography
{
  using System.Linq;

  using Tangle.Net.Source.Cryptography;
  using Tangle.Net.Source.Entity;

  /// <summary>
  /// The private key stub.
  /// </summary>
  internal class PrivateKeyStub : IPrivateKey
  {
    #region Public Properties

    /// <summary>
    /// Gets the digest.
    /// </summary>
    public Digest Digest { get; private set; }

    /// <summary>
    /// Gets the security level.
    /// </summary>
    public int SecurityLevel { get; private set; }

    /// <summary>
    /// Gets or sets the value.
    /// </summary>
    public string Value { get; set; }

    #endregion

    #region Public Methods and Operators

    /// <summary>
    /// The sign input transactions.
    /// </summary>
    /// <param name="bundle">
    /// The bundle.
    /// </param>
    /// <param name="startIndex">
    /// The start index.
    /// </param>
    public void SignInputTransactions(Bundle bundle, int startIndex)
    {
      for (var i = startIndex; i < bundle.Transactions.Count(); i++)
      {
        bundle.Transactions[i].Fragment = new Fragment("SOMESIGNATUREFRAGMENTWILLBEGENERATEDHERE");
      }
    }

    #endregion
  }
}