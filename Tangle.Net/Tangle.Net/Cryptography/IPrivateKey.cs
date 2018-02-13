namespace Tangle.Net.Cryptography
{
  using Tangle.Net.Entity;

  /// <summary>
  /// The PrivateKey interface.
  /// </summary>
  public interface IPrivateKey
  {
    #region Public Properties

    /// <summary>
    /// Gets the digest.
    /// </summary>
    Digest Digest { get; }

    /// <summary>
    /// Gets the security level.
    /// </summary>
    int SecurityLevel { get; }

    /// <summary>
    /// Gets the value.
    /// </summary>
    string Value { get; }

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
    void SignInputTransactions(Bundle bundle, int startIndex);

    #endregion
  }
}