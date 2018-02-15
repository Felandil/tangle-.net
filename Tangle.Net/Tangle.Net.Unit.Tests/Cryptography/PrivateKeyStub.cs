namespace Tangle.Net.Unit.Tests.Cryptography
{
  using System.Linq;

  using Tangle.Net.Cryptography;
  using Tangle.Net.Entity;

  /// <summary>
  /// The private key stub.
  /// </summary>
  public class PrivateKeyStub : IPrivateKey
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
    /// Gets the value.
    /// </summary>
    public string Value
    {
      get
      {
        return Seed.Random().Value;
      }
    }

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