namespace Tangle.Net.Mam
{
  /// <summary>
  /// The Mask interface.
  /// </summary>
  public interface IMask
  {
    #region Public Methods and Operators

    /// <summary>
    /// The mask.
    /// </summary>
    /// <param name="payload">
    /// The payload.
    /// </param>
    /// <param name="key">
    /// The auth id trits.
    /// </param>
    /// <returns>
    /// The <see cref="int[]"/>.
    /// </returns>
    int[] Mask(int[] payload, int[] key);

    /// <summary>
    /// The unmask.
    /// </summary>
    /// <param name="payload">
    /// The masked cipher.
    /// </param>
    /// <param name="key">
    /// The auth id trits.
    /// </param>
    /// <returns>
    /// The <see cref="int[]"/>.
    /// </returns>
    int[] Unmask(int[] payload, int[] key);

    #endregion
  }
}