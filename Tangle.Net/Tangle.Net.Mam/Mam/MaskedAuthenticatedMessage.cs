namespace Tangle.Net.Mam.Mam
{
  using Tangle.Net.Entity;

  /// <summary>
  /// The masked authenticated message.
  /// </summary>
  public class MaskedAuthenticatedMessage
  {
    /// <summary>
    /// Gets or sets the next channel key.
    /// </summary>
    public Hash NextChannelKey { get; set; }

    /// <summary>
    /// Gets or sets the payload.
    /// </summary>
    public Bundle Payload { get; set; }
  }
}