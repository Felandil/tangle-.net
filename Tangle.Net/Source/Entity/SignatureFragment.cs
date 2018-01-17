namespace Tangle.Net.Source.Entity
{
  using System;

  /// <summary>
  /// The signature fragment.
  /// </summary>
  public class SignatureFragment : TryteString
  {
    #region Constants

    /// <summary>
    /// The length.
    /// </summary>
    public const int Length = 2187;

    #endregion

    #region Constructors and Destructors

    /// <summary>
    /// Initializes a new instance of the <see cref="SignatureFragment"/> class.
    /// </summary>
    public SignatureFragment()
      : this(string.Empty)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SignatureFragment"/> class.
    /// </summary>
    /// <param name="value">
    /// The value.
    /// </param>
    public SignatureFragment(string value)
      : base(value)
    {
      if (this.TrytesLength > Length)
      {
        throw new ArgumentException("Tag length must not be longer than " + Length);
      }

      if (this.TrytesLength < Length)
      {
        this.Pad(Length);
      }
    }

    #endregion
  }
}