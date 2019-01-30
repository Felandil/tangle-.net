namespace Tangle.Net.Entity
{
  using System;
  using System.Collections.Generic;
  using System.Diagnostics.CodeAnalysis;
  using System.Linq;
  using System.Text;

  using Tangle.Net.Cryptography;
  using Tangle.Net.Utils;

  /// <summary>
  /// The tryte string.
  /// </summary>
  public class TryteString
  {
    /// <summary>
    /// The tryte alphabet.
    /// </summary>
    public const string TryteAlphabet = "9ABCDEFGHIJKLMNOPQRSTUVWXYZ";

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
        throw new ArgumentException(
          "Given string does contain invalid characters. Use 'From string' if you meant to convert a string to tryteString");
      }

      this.Value = trytes;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:Tangle.Net.Entity.TryteString" /> class.
    /// </summary>
    public TryteString()
      : this(string.Empty)
    {
    }

    /// <summary>
    /// Gets the chunkLength.
    /// </summary>
    public int TrytesLength => this.Value.Length;

    /// <summary>
    /// Gets or sets the value.
    /// </summary>
    public string Value { get; protected set; }

    /// <summary>
    /// The from string.
    /// </summary>
    /// <param name="input">
    /// The input.
    /// </param>
    /// <returns>
    /// The <see cref="TryteString"/>.
    /// </returns>
    public static TryteString FromAsciiString(string input)
    {
      return new TryteString(FromAsciiToTryteString(input));
    }

    /// <summary>
    /// The from utf 8 string.
    /// </summary>
    /// <param name="input">
    /// The input.
    /// </param>
    /// <returns>
    /// The <see cref="TryteString"/>.
    /// </returns>
    public static TryteString FromUtf8String(string input)
    {
      return new TryteString(BytesToTrytes(Encoding.UTF8.GetBytes(input)));
    }

    /// <summary>
    /// The from bytes.
    /// </summary>
    /// <param name="bytes">
    /// The bytes.
    /// </param>
    /// <returns>
    /// The <see cref="TryteString"/>.
    /// </returns>
    public static TryteString FromBytes(IEnumerable<byte> bytes)
    {
      return new TryteString(BytesToTrytes(bytes));
    }

    /// <summary>
    /// The get empty.
    /// </summary>
    /// <param name="length">
    /// The length.
    /// </param>
    /// <returns>
    /// The <see cref="TryteString"/>.
    /// </returns>
    public static TryteString GetEmpty(int length)
    {
      return new TryteString(new string('9', length));
    }

    /// <summary>
    /// The concat.
    /// </summary>
    /// <param name="with">
    /// The with.
    /// </param>
    /// <returns>
    /// The <see cref="TryteString"/>.
    /// </returns>
    public TryteString Concat(TryteString with)
    {
      return new TryteString(this.Value + with.Value);
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
    public T GetChunk<T>(int offset, int length)
      where T : TryteString, new()
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
    /// The to string.
    /// </summary>
    /// <returns>
    /// The <see cref="string"/>.
    /// </returns>
    public string ToAsciiString()
    {
      return Encoding.ASCII.GetString(this.TextToBytes());
    }

    /// <summary>
    /// The to bytes.
    /// </summary>
    /// <returns>
    /// The <see cref="byte[]"/>.
    /// </returns>
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1407:ArithmeticExpressionsMustDeclarePrecedence", Justification = "Reviewed. Suppression is OK here.")]
    public byte[] ToBytes()
    {
      var messageTrytes = this.Value;
      messageTrytes += messageTrytes.Length % 2 == 1 ? "9" : string.Empty;

      return TrytesToBytes(messageTrytes);
    }

    /// <summary>
    /// The to string.
    /// </summary>
    /// <returns>
    /// The <see cref="string"/>.
    /// </returns>
    public override string ToString()
    {
      return this.Value;
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

    /// <summary>
    /// The to utf 8 string.
    /// </summary>
    /// <returns>
    /// The <see cref="string"/>.
    /// </returns>
    public string ToUtf8String()
    {
      return Encoding.UTF8.GetString(this.TextToBytes());
    }

    /// <summary>
    /// The bytes to trytes.
    /// </summary>
    /// <param name="bytes">
    /// The byte values.
    /// </param>
    /// <returns>
    /// The <see cref="string"/>.
    /// </returns>
    internal static string BytesToTrytes(IEnumerable<byte> bytes)
    {
      return (from byteValue in bytes
              let firstValue = byteValue % 27
              let secondValue = (byteValue - firstValue) / 27
              select $"{TryteAlphabet[firstValue]}{TryteAlphabet[secondValue]}").Aggregate(
        string.Empty,
        (current, trytesValue) => current + trytesValue);
    }

    /// <summary>
    /// The from ascii to tryte string.
    /// </summary>
    /// <param name="input">
    /// The input.
    /// </param>
    /// <returns>
    /// The <see cref="string"/>.
    /// </returns>
    protected static string FromAsciiToTryteString(string input)
    {
      if (input.Any(c => c > 255))
      {
        throw new ArgumentException("Detected non ASCII string input.");
      }

      return BytesToTrytes(Encoding.ASCII.GetBytes(input));
    }

    /// <summary>
    /// The pad value.
    /// </summary>
    /// <param name="length">
    /// The length.
    /// </param>
    protected void Pad(int length)
    {
      this.Value = this.Value.PadRight(length, '9');
    }

    /// <summary>
    /// The trytes to bytes.
    /// </summary>
    /// <param name="messageTrytes">
    /// The message trytes.
    /// </param>
    /// <returns>
    /// The <see cref="byte[]"/>.
    /// </returns>
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1407:ArithmeticExpressionsMustDeclarePrecedence", Justification = "Reviewed. Suppression is OK here.")]
    private static byte[] TrytesToBytes(string messageTrytes)
    {
      var byteResult = new List<byte>();
      for (var i = 0; i < messageTrytes.Length; i += 2)
      {
        var byteValue = TryteAlphabet.IndexOf(messageTrytes[i]) + TryteAlphabet.IndexOf(messageTrytes[i + 1]) * 27;
        byteResult.Add((byte)byteValue);
      }

      return byteResult.ToArray();
    }

    /// <summary>
    /// The text to bytes.
    /// When converting text, we need to trim all 9's at the end of a TryteString
    /// </summary>
    /// <returns>
    /// The <see cref="byte[]"/>.
    /// </returns>
    private byte[] TextToBytes()
    {
      var messageTrytes = this.Value;

      if (messageTrytes.Last() != '9')
      {
        return TrytesToBytes(messageTrytes);
      }

      messageTrytes = messageTrytes.TrimEnd('9');
      messageTrytes += messageTrytes.Length % 2 == 1 ? "9" : string.Empty;

      return TrytesToBytes(messageTrytes);
    }
  }
}