namespace Tangle.Net.Mam.Services
{
  using Tangle.Net.Entity;
  using Tangle.Net.Mam.Entity;

  /// <summary>
  /// The MamParser interface.
  /// </summary>
  public interface IMamParser
  {
    /// <summary>
    /// The unmask.
    /// </summary>
    /// <param name="payload">
    /// The payload.
    /// </param>
    /// <param name="root">
    /// The root.
    /// </param>
    /// <param name="channelKey">
    /// The channel key.
    /// </param>
    /// <returns>
    /// The <see cref="UnmaskedAuthenticatedMessage"/>.
    /// </returns>
    UnmaskedAuthenticatedMessage Unmask(Bundle payload, TryteString root, TryteString channelKey);
  }
}