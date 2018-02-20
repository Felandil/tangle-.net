using Tangle.Net.Entity;

namespace Tangle.Net.Mam.Mam
{
  public class UnmaskedAuthenticatedMessage
  {
    /// <summary>
    /// Gets or sets the next channel key.
    /// </summary>
    public Hash NextChannelKey { get; set; }

    public TryteString Message { get; set; }

    public Hash NextRoot { get; set; }

    public Hash Root { get; set; }
  }
}
