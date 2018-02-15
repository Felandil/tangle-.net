namespace Tangle.Net.Mam.Mam
{
  using Tangle.Net.Entity;

  /// <summary>
  /// The masked authenticated message.
  /// </summary>
  public class MaskedAuthenticatedMessage
  {
    /// <summary>
    /// Gets or sets the payload.
    /// </summary>
    public TryteString Payload { get; set; }
  }
}