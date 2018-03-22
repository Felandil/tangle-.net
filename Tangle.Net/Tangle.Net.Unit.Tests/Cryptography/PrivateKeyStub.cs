namespace Tangle.Net.Unit.Tests.Cryptography
{
  using System.Linq;

  using Tangle.Net.Cryptography;
  using Tangle.Net.Entity;

  /// <summary>
  /// The private key stub.
  /// </summary>
  public class PrivateKeyStub : AbstractPrivateKey
  {
    /// <inheritdoc />
    public PrivateKeyStub()
      : base(string.Empty, Net.Cryptography.SecurityLevel.Low, 0)
    {
    }

    /// <summary>
    /// Gets the digest.
    /// </summary>
    public override Digest Digest { get; }

    /// <summary>
    /// Gets the value.
    /// </summary>
    public new string Value => Seed.Random().Value;

    /// <summary>
    /// The sign input transactions.
    /// </summary>
    /// <param name="bundle">
    /// The bundle.
    /// </param>
    /// <param name="startIndex">
    /// The start index.
    /// </param>
    public override void SignInputTransactions(Bundle bundle, int startIndex)
    {
      for (var i = startIndex; i < bundle.Transactions.Count(); i++)
      {
        bundle.Transactions[i].Fragment = new Fragment("SOMESIGNATUREFRAGMENTWILLBEGENERATEDHERE");
      }
    }
  }
}