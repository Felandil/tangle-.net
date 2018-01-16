namespace Tangle.Net.Source.Entity
{
  using System;

  using Tangle.Net.Source.Utils;

  /// <summary>
  /// The tryte string.
  /// </summary>
  public class TryteString
  {
    #region Constructors and Destructors

    /// <summary>
    /// Initializes a new instance of the <see cref="TryteString"/> class.
    /// </summary>
    /// <param name="trytes">
    /// The trytes.
    /// </param>
    /// <exception cref="ArgumentException">
    /// Thrown if on or more characters of the given string are no trytes
    /// </exception>
    public TryteString(string trytes)
    {
      if (!InputValidator.IsTrytes(trytes))
      {
        throw new ArgumentException("Given string does contain invalid characters. Use 'From string' if you meant to convert a string to tryteString");
      }

      this.Value = trytes;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="TryteString"/> class.
    /// </summary>
    public TryteString()
      : this(string.Empty)
    {
    }

    #endregion

    #region Public Properties

    /// <summary>
    /// Gets the length.
    /// </summary>
    public int Length
    {
      get
      {
        return this.Value.Length;
      }
    }

    /// <summary>
    /// Gets or sets the value.
    /// </summary>
    public string Value { get; protected set; }

    #endregion

    #region Public Methods and Operators

    /// <summary>
    /// The from string.
    /// </summary>
    /// <param name="input">
    /// The input.
    /// </param>
    /// <returns>
    /// The <see cref="TryteString"/>.
    /// </returns>
    public static TryteString FromString(string input)
    {
      return new TryteString(AsciiToTrytes.FromString(input));
    }

    /// <summary>
    /// The get chunk.
    /// </summary>
    /// <param name="offset">
    /// The offset.
    /// </param>
    /// <param name="length">
    /// The length.
    /// </param>
    /// <returns>
    /// The <see cref="TryteString"/>.
    /// </returns>
    public TryteString GetChunk(int offset, int length)
    {
      return new TryteString(this.Value.Substring(offset, length));
    }

    #endregion
  }
}