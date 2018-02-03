namespace Tangle.Net.ProofOfWork
{
  using Tangle.Net.Entity;

  /// <summary>
  /// The PoWDiver interface.
  /// </summary>
  public interface IPoWDiver
  {
    #region Public Methods and Operators

    /// <summary>
    /// The do pow.
    /// </summary>
    /// <param name="trytes">
    /// The trytes.
    /// </param>
    /// <param name="minWeightMagnitude">
    /// The min weight magnitude.
    /// </param>
    /// <returns>
    /// The <see cref="TransactionTrytes"/>.
    /// </returns>
    TransactionTrytes DoPow(TransactionTrytes trytes, int minWeightMagnitude);

    #endregion
  }
}