namespace Tangle.Net.Source.Entity
{
  using System;
  using System.Collections;
  using System.Collections.Generic;

  using Tangle.Net.Source.Cryptography;
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
    /// Gets the chunkLength.
    /// </summary>
    public int TrytesLength
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
    /// The chunkLength.
    /// </param>
    /// <returns>
    /// The <see cref="TryteString"/>.
    /// </returns>
    public TryteString GetChunk(int offset, int length)
    {
      return new TryteString(this.Value.Substring(offset, length));
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
    /// <typeparam name="T">
    /// The TryteString object to return
    /// </typeparam>
    /// <returns>
    /// The <see cref="T"/>.
    /// </returns>
    public T GetChunk<T>(int offset, int length) where T : TryteString, new()
    {
      return (T)Activator.CreateInstance(typeof(T), this.Value.Substring(offset, length));
    }

    /// <summary>
    /// The get chunks.
    /// </summary>
    /// <param name="chunkLength">
    /// The chunkLength.
    /// </param>
    /// <returns>
    /// The <see cref="IEnumerable"/>.
    /// </returns>
    public List<TryteString> GetChunks(int chunkLength)
    {
      var chunks = new List<TryteString>();
      for (var i = 0; i * chunkLength < this.Value.Length; i++)
      {
        var offset = i * chunkLength;
        chunks.Add(this.GetChunk(offset, offset + chunkLength > this.Value.Length ? this.Value.Length - offset : chunkLength));
      }

      return chunks;
    }

    /// <summary>
    /// The to trits.
    /// </summary>
    /// <returns>
    /// The <see cref="int[]"/>.
    /// </returns>
    public int[] ToTrits()
    {
      return Converter.TrytesToTrits(this.Value);
    }

    #endregion

    #region Methods

    /// <summary>
    /// The pad value.
    /// </summary>
    /// <param name="length">
    /// The length.
    /// </param>
    protected void Pad(int length)
    {
      while (this.TrytesLength < length)
      {
        this.Value += '9';
      }
    }

    #endregion
  }
}