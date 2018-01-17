namespace Tangle.Net.Source.Cryptography
{
  /// <summary>
  /// The Curl interface.
  /// </summary>
  public abstract class AbstractCurl
  {
    #region Constants

    /// <summary>
    /// The hash length.
    /// </summary>
    public const int HashLength = 243;

    #endregion

    #region Public Methods and Operators

    /// <summary>
    /// The absorb.
    /// </summary>
    /// <param name="trits">
    /// The trits.
    /// </param>
    public abstract void Absorb(int[] trits);

    /// <summary>
    /// The reset.
    /// </summary>
    public abstract void Reset();

    /// <summary>
    /// The squeeze.
    /// </summary>
    /// <param name="trits">
    /// The trits.
    /// </param>
    public abstract void Squeeze(int[] trits);

    #endregion
  }
}