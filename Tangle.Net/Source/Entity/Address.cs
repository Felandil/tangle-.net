namespace Tangle.Net.Source.Entity
{
  using System;

  using Tangle.Net.Source.Utils;

  /// <summary>
  /// The address.
  /// </summary>
  public class Address : TryteString
  {
    #region Constants

    /// <summary>
    /// The length.
    /// </summary>
    public new const int Length = 81;

    #endregion

    #region Constructors and Destructors

    /// <summary>
    /// Initializes a new instance of the <see cref="Address"/> class.
    /// </summary>
    /// <param name="value">
    /// The value.
    /// </param>
    /// <exception cref="ArgumentException">
    /// </exception>
    public Address(string value)
      : base(value)
    {
      if (!InputValidator.IsAddress(value))
      {
        throw new ArgumentException("Given value is no address. Address should be of length " + Length);
      }
    }

    #endregion

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

    #endregion
  }
}