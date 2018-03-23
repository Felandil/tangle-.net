namespace Tangle.Net.Entity
{
  using System;
  using System.Collections.Generic;
  using System.Linq;

  using Tangle.Net.Cryptography;

  /// <summary>
  /// The signature fragment.
  /// </summary>
  public class Fragment : TryteString
  {
    #region Constants

    /// <summary>
    /// The length.
    /// </summary>
    public const int Length = 2187;

    #endregion

    #region Constructors and Destructors

    /// <summary>
    /// Initializes a new instance of the <see cref="Fragment"/> class.
    /// </summary>
    public Fragment()
      : this(string.Empty)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Fragment"/> class.
    /// </summary>
    /// <param name="trytes">
    /// The trytes.
    /// </param>
    public Fragment(string trytes)
      : base(trytes)
    {
      if (this.TrytesLength > Length)
      {
        throw new ArgumentException("Fragment length must not be longer than " + Length);
      }

      if (this.TrytesLength < Length)
      {
        this.Pad(Length);
      }
    }

    #endregion

    #region Public Properties

    /// <summary>
    /// Gets a value indicating whether is empty.
    /// </summary>
    public bool IsEmpty
    {
      get
      {
        return this.Value.All(c => c == '9');
      }
    }

    #endregion

    #region Public Methods and Operators

    /// <summary>
    /// The from string.
    /// </summary>
    /// <param name="input">
    /// The input.
    /// </param>
    /// <returns>
    /// The <see cref="Fragment"/>.
    /// </returns>
    public static new Fragment FromAsciiString(string input)
    {
      return new Fragment(FromAsciiToTryteString(input));
    }

    /// <summary>
    /// The from utf 8 string.
    /// </summary>
    /// <param name="input">
    /// The input.
    /// </param>
    /// <returns>
    /// The <see cref="Fragment"/>.
    /// </returns>
    public static new Fragment FromUtf8String(string input)
    {
      return new Fragment(TryteString.FromUtf8String(input).Value);
    }

    #endregion
  }
}