namespace Tangle.Net.Cryptography.Curl
{
  using System.Linq;

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

    #region Fields

    /// <summary>
    /// The trit state.
    /// </summary>
    public int[] State;

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
    /// The rate.
    /// </summary>
    /// <param name="length">
    /// The length.
    /// </param>
    /// <returns>
    /// The <see cref="int[]"/>.
    /// </returns>
    public int[] Rate(int length)
    {
      return this.State.Take(length).ToArray();
    }

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