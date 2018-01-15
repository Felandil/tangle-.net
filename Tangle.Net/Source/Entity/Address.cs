namespace Tangle.Net.Source.Entity
{
  /// <summary>
  /// The address.
  /// </summary>
  public class Address
  {
    #region Public Properties

    /// <summary>
    /// Gets or sets the balance.
    /// </summary>
    public long Balance { get; set; }

    /// <summary>
    /// Gets or sets the key index.
    /// </summary>
    public int KeyIndex { get; set; }

    /// <summary>
    /// Gets or sets the security level.
    /// </summary>
    public int SecurityLevel { get; set; }

    /// <summary>
    /// Gets or sets the trytes.
    /// </summary>
    public string Trytes { get; set; }

    #endregion
  }
}