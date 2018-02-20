namespace Tangle.Net.Mam
{
  using Tangle.Net.Entity;

  /// <summary>
  /// The Mask interface.
  /// </summary>
  public interface IMask
  {
    /// <summary>
    /// The hash.
    /// </summary>
    /// <param name="key">
    /// The key.
    /// </param>
    /// <param name="salt">
    /// The seed.
    /// </param>
    /// <returns>
    /// The <see cref="Hash"/>.
    /// </returns>
    Hash Hash(TryteString key, TryteString salt = null);

    /// <summary>
    /// The mask.
    /// </summary>
    /// <param name="payload">
    /// The payload.
    /// </param>
    /// <param name="key">
    /// The key.
    /// </param>
    /// <returns>
    /// The <see cref="TryteString"/>.
    /// </returns>
    TryteString Mask(TryteString payload, TryteString key);

    /// <summary>
    /// The unmask.
    /// </summary>
    /// <param name="payload">
    /// The payload.
    /// </param>
    /// <param name="key">
    /// The key.
    /// </param>
    /// <returns>
    /// The <see cref="TryteString"/>.
    /// </returns>
    TryteString Unmask(TryteString payload, TryteString key);
  }
}