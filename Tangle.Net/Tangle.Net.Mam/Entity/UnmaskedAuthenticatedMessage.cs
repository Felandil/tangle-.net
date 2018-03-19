namespace Tangle.Net.Mam.Entity
{
  using Tangle.Net.Entity;

  /// <summary>
  /// The unmasked authenticated message.
  /// </summary>
  public class UnmaskedAuthenticatedMessage
  {
    /// <summary>
    /// Gets or sets the message.
    /// </summary>
    public TryteString Message { get; set; }

    /// <summary>
    /// Gets or sets the next root.
    /// </summary>
    public Hash NextRoot { get; set; }

    /// <summary>
    /// Gets or sets the root.
    /// </summary>
    public Hash Root { get; set; }
  }
}