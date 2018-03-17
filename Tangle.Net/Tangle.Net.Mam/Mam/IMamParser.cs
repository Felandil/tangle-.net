namespace Tangle.Net.Mam.Mam
{
  using Tangle.Net.Entity;

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
    /// <param name="channelKey">
    /// The channel key.
    /// </param>
    /// <param name="securityLevel">
    /// The security Level.
    /// </param>
    /// <returns>
    /// The <see cref="UnmaskedAuthenticatedMessage"/>.
    /// </returns>
    UnmaskedAuthenticatedMessage Unmask(Bundle payload, TryteString channelKey, int securityLevel);
  }
}