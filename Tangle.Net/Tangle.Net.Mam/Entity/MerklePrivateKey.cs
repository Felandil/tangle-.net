namespace Tangle.Net.Mam.Entity
{
  using Tangle.Net.Cryptography;
  using Tangle.Net.Entity;

  /// <summary>
  /// The merkle private key.
  /// </summary>
  public class MerklePrivateKey : AbstractPrivateKey
  {
    /// <inheritdoc />
    public MerklePrivateKey(string privateKey, int securityLevel, int keyIndex, string digest)
      : base(privateKey, securityLevel, keyIndex)
    {
      this.Digest = new Digest(digest, keyIndex, securityLevel);
    }

    /// <inheritdoc />
    public override Digest Digest { get; }

    /// <inheritdoc />
    public override void SignInputTransactions(Bundle bundle, int startIndex)
    {
    }
  }
}