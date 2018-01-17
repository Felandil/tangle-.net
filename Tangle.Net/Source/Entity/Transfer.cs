namespace Tangle.Net.Source.Entity
{
  /// <summary>
  /// The transfer.
  /// </summary>
  public class Transfer
  {
    #region Constructors and Destructors

    /// <summary>
    /// Initializes a new instance of the <see cref="Transfer"/> class.
    /// </summary>
    public Transfer()
    {
      this.Tag = new Tag();
    }

    #endregion

    #region Public Properties

    /// <summary>
    /// Gets or sets the address.
    /// </summary>
    public Address Address { get; set; }

    /// <summary>
    /// Gets or sets the message.
    /// </summary>
    public TryteString Message { get; set; }

    /// <summary>
    /// Gets or sets the tag.
    /// </summary>
    public Tag Tag { get; set; }

    /// <summary>
    /// Gets or sets the timestamp.
    /// </summary>
    public int Timestamp { get; set; }

    #endregion
  }
}