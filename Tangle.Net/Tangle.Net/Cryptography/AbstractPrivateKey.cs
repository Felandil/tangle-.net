namespace Tangle.Net.Cryptography
{
  using Tangle.Net.Entity;

  /// <summary>
  /// The PrivateKey interface.
  /// </summary>
  public abstract class AbstractPrivateKey : TryteString
  {
    /// <summary>
    /// The chunk length.
    /// </summary>
    public const int ChunkLength = 2187;

    /// <inheritdoc />
    protected AbstractPrivateKey(string privateKey, int securityLevel, int keyIndex)
      : base(privateKey)
    {
      this.SecurityLevel = securityLevel;
      this.KeyIndex = keyIndex;
    }

    /// <summary>
    /// Gets the digest.
    /// </summary>
    public abstract Digest Digest { get; }

    /// <summary>
    /// Gets the security level.
    /// </summary>
    protected int SecurityLevel { get; }

    /// <summary>
    /// Gets the key index.
    /// </summary>
    protected int KeyIndex { get; }

    /// <summary>
    /// The sign input transactions.
    /// </summary>
    /// <param name="bundle">
    /// The bundle.
    /// </param>
    /// <param name="startIndex">
    /// The start index.
    /// </param>
    public abstract void SignInputTransactions(Bundle bundle, int startIndex);
  }
}