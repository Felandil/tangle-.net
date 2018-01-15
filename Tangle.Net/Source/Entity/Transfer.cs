namespace Tangle.Net.Source.Entity
{
  /// <summary>
  /// The transfer.
  /// </summary>
  public class Transfer
  {
    #region Public Properties

    /// <summary>
    /// Gets or sets the address.
    /// </summary>
    public string Address { get; set; }

    /// <summary>
    /// Gets or sets the message.
    /// </summary>
    public string Message { get; set; }

    /// <summary>
    /// Gets or sets the tag.
    /// </summary>
    public string Tag { get; set; }

    /// <summary>
    /// Gets or sets the timestamp.
    /// </summary>
    public long Timestamp { get; set; }

    /// <summary>
    /// Gets or sets the value.
    /// </summary>
    public long Value { get; set; }

    #endregion
  }
}