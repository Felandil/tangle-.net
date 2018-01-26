namespace Tangle.Net.Entity
{
  /// <summary>
  /// The input.
  /// </summary>
  public class Input
  {
    #region Public Properties

    /// <summary>
    /// Gets or sets the address.
    /// </summary>
    public Address Address { get; set; }

    /// <summary>
    /// Gets or sets the balance.
    /// </summary>
    public long Balance { get; set; }

    /// <summary>
    /// Gets or sets the key index.
    /// </summary>
    public int KeyIndex { get; set; }

    #endregion
  }
}