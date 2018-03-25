namespace Tangle.Net.ProofOfWork
{
  using Tangle.Net.Entity;

  /// <summary>
  /// The PoWDiver interface.
  /// </summary>
  public interface IPearlDiver
  {
    #region Public Methods and Operators

    /// <summary>
    /// The do pow.
    /// </summary>
    /// <param name="trits">
    /// The trits.
    /// </param>
    /// <param name="minWeightMagnitude">
    /// The min weight magnitude.
    /// </param>
    /// <returns>
    /// The <see cref="TransactionTrytes"/>.
    /// </returns>
    int[] Search(int[] trits, int minWeightMagnitude);

    #endregion
  }
}