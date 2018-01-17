namespace Tangle.Net.Source.Cryptography
{
  /// <summary>
  /// The Curl interface.
  /// </summary>
  public interface ICurl
  {
    #region Public Methods and Operators

    /// <summary>
    /// The absorb.
    /// </summary>
    /// <param name="trits">
    /// The trits.
    /// </param>
    void Absorb(int[] trits);

    /// <summary>
    /// The squeeze.
    /// </summary>
    /// <param name="trits">
    /// The trits.
    /// </param>
    void Squeeze(int[] trits);

    #endregion
  }
}