namespace Tangle.Net.Cryptography
{
  using System;
  using System.Collections.Generic;
  using System.Linq;

  using Org.BouncyCastle.Math;

  using Tangle.Net.Utils;

  /// <summary>
  /// The converter.
  /// </summary>
  public static class Converter
  {
    #region Constants

    /// <summary>
    /// The radix.
    /// </summary>
    public const int Radix = 3;

    /// <summary>
    /// The max trit value.
    /// </summary>
    private const int MaxTritValue = 1;

    /// <summary>
    /// The min trit value.
    /// </summary>
    private const int MinTritValue = -1;

    #endregion

    #region Static Fields

    /// <summary>
    /// The trytes lookup.
    /// </summary>
    private static readonly Dictionary<char, int[]> TrytesLookup = new Dictionary<char, int[]>
                                                                     {
                                                                       { '9', new[] { 0, 0, 0 } }, 
                                                                       { 'A', new[] { 1, 0, 0 } }, 
                                                                       { 'B', new[] { -1, 1, 0 } }, 
                                                                       { 'C', new[] { 0, 1, 0 } }, 
                                                                       { 'D', new[] { 1, 1, 0 } }, 
                                                                       { 'E', new[] { -1, -1, 1 } }, 
                                                                       { 'F', new[] { 0, -1, 1 } }, 
                                                                       { 'G', new[] { 1, -1, 1 } }, 
                                                                       { 'H', new[] { -1, 0, 1 } }, 
                                                                       { 'I', new[] { 0, 0, 1 } }, 
                                                                       { 'J', new[] { 1, 0, 1 } }, 
                                                                       { 'K', new[] { -1, 1, 1 } }, 
                                                                       { 'L', new[] { 0, 1, 1 } }, 
                                                                       { 'M', new[] { 1, 1, 1 } }, 
                                                                       { 'N', new[] { -1, -1, -1 } }, 
                                                                       { 'O', new[] { 0, -1, -1 } }, 
                                                                       { 'P', new[] { 1, -1, -1 } }, 
                                                                       { 'Q', new[] { -1, 0, -1 } }, 
                                                                       { 'R', new[] { 0, 0, -1 } }, 
                                                                       { 'S', new[] { 1, 0, -1 } }, 
                                                                       { 'T', new[] { -1, 1, -1 } }, 
                                                                       { 'U', new[] { 0, 1, -1 } }, 
                                                                       { 'V', new[] { 1, 1, -1 } }, 
                                                                       { 'W', new[] { -1, -1, 0 } }, 
                                                                       { 'X', new[] { 0, -1, 0 } }, 
                                                                       { 'Y', new[] { 1, -1, 0 } }, 
                                                                       { 'Z', new[] { -1, 0, 0 } }
                                                                     };

    #endregion

    #region Public Methods and Operators

    /// <summary>
    /// The increment.
    /// </summary>
    /// <param name="leftTrits">
    /// The left Trits.
    /// </param>
    /// <param name="rightTrits">
    /// The right Trits.
    /// </param>
    /// <returns>
    /// The <see cref="int[]"/>.
    /// </returns>
    public static int[] AddTrits(int[] leftTrits, int[] rightTrits)
    {
      var outputTrits = new int[leftTrits.Length > rightTrits.Length ? leftTrits.Length : rightTrits.Length];
      var carry = 0;

      for (var i = 0; i < outputTrits.Length; i++)
      {
        var a_i = i < leftTrits.Length ? leftTrits[i] : 0;
        var b_i = i < rightTrits.Length ? rightTrits[i] : 0;
        var f_a = FullAdd(a_i, b_i, carry);
        outputTrits[i] = f_a[0];
        carry = f_a[1];
      }

      return outputTrits;
    }

    /// <summary>
    /// The convert bigint to bytes.
    /// </summary>
    /// <param name="value">
    /// The value.
    /// </param>
    /// <returns>
    /// The <see cref="byte[]"/>.
    /// </returns>
    public static byte[] ConvertBigIntToBytes(BigInteger value)
    {
      var result = new byte[Kerl.ByteHashLength];
      var bytes = value.ToByteArray();
      var isNegative = value.CompareTo(BigInteger.Zero) < 0;

      var i = 0;
      while (i + bytes.Length < Kerl.ByteHashLength)
      {
        if (isNegative)
        {
          result[i++] = 255;
        }
        else
        {
          result[i++] = 0;
        }
      }

      for (var j = bytes.Length; j-- > 0;)
      {
        result[i++] = bytes[bytes.Length - 1 - j];
      }

      return result;
    }

    /// <summary>
    /// The convert big int to trits.
    /// </summary>
    /// <param name="value">
    /// The value.
    /// </param>
    /// <param name="size">
    /// The size.
    /// </param>
    /// <returns>
    /// The <see cref="int[]"/>.
    /// </returns>
    public static int[] ConvertBigIntToTrits(BigInteger value, int size)
    {
      var isNegative = value.CompareTo(BigInteger.Zero) < 0;

      var destination = new int[size];

      var absoluteValue = isNegative ? value.Abs() : value;
      for (var i = 0; i < size; i++)
      {
        var divRemainder = absoluteValue.DivideAndRemainder(BigInteger.ValueOf(Radix));
        var remainder = divRemainder[1].IntValue;
        absoluteValue = divRemainder[0];

        if (remainder > MaxTritValue)
        {
          remainder = MinTritValue;
          absoluteValue = absoluteValue.Add(BigInteger.One);
        }

        destination[i] = remainder;
      }

      if (isNegative)
      {
        for (var i = 0; i < size; i++)
        {
          destination[i] = -destination[i];
        }
      }

      return destination;
    }

    /// <summary>
    /// The convert bytes to big int.
    /// </summary>
    /// <param name="bytes">
    /// The bytes.
    /// </param>
    /// <returns>
    /// The <see cref="BigInteger"/>.
    /// </returns>
    public static BigInteger ConvertBytesToBigInt(IEnumerable<byte> bytes)
    {
      return new BigInteger(bytes.ToArray());
    }

    /// <summary>
    /// The convert bytes to trits.
    /// </summary>
    /// <param name="bytes">
    /// The bytes.
    /// </param>
    /// <returns>
    /// The <see cref="int[]"/>.
    /// </returns>
    public static int[] ConvertBytesToTrits(IEnumerable<byte> bytes)
    {
      return ConvertBigIntToTrits(ConvertBytesToBigInt(bytes), Kerl.HashLength);
    }

    /// <summary>
    /// The convert trits to bigint.
    /// </summary>
    /// <param name="trits">
    /// The trits.
    /// </param>
    /// <param name="offset">
    /// The offset.
    /// </param>
    /// <param name="size">
    /// The size.
    /// </param>
    /// <returns>
    /// The <see cref="BigInteger"/>.
    /// </returns>
    public static BigInteger ConvertTritsToBigInt(int[] trits, int offset, int size)
    {
      var value = BigInteger.Zero;

      for (var i = size; i-- > 0;)
      {
        value = value.Multiply(BigInteger.ValueOf(Radix)).Add(BigInteger.ValueOf(trits[offset + i]));
      }

      return value;
    }

    /// <summary>
    /// The convert trits to bytes.
    /// </summary>
    /// <param name="trits">
    /// The trits.
    /// </param>
    /// <returns>
    /// The <see cref="byte[]"/>.
    /// </returns>
    public static byte[] ConvertTritsToBytes(int[] trits)
    {
      return ConvertBigIntToBytes(ConvertTritsToBigInt(trits, 0, Kerl.HashLength));
    }

    /// <summary>
    /// The increment.
    /// </summary>
    /// <param name="trits">
    /// The trits.
    /// </param>
    /// <param name="size">
    /// The size.
    /// </param>
    public static void Increment(int[] trits, int size)
    {
      for (var i = 0; i < size; i++)
      {
        if (++trits[i] > MaxTritValue)
        {
          trits[i] = MinTritValue;
        }
        else
        {
          break;
        }
      }
    }

    /// <summary>
    /// The in to trits.
    /// </summary>
    /// <param name="value">
    /// The value.
    /// </param>
    /// <param name="size">
    /// The size.
    /// </param>
    /// <returns>
    /// The <see cref="int[]"/>.
    /// </returns>
    public static int[] IntToTrits(int value, int size)
    {
      var destination = new int[size];
      var absoluteValue = value < 0 ? -value : value;
      var i = 0;

      while (absoluteValue > 0)
      {
        var remainder = absoluteValue % Radix;
        absoluteValue = (int)Math.Floor((double)absoluteValue / Radix);

        if (remainder > MaxTritValue)
        {
          remainder = MinTritValue;
          absoluteValue++;
        }

        destination[i] = remainder;
        i++;
      }

      if (value < 0)
      {
        for (var y = 0; y < destination.Length; y++)
        {
          destination[i] = -destination[i];
        }
      }

      return destination;
    }

    /// <summary>
    /// The trits to int.
    /// </summary>
    /// <param name="trits">
    /// The trits.
    /// </param>
    /// <returns>
    /// The <see cref="int"/>.
    /// </returns>
    public static int TritsToInt(int[] trits)
    {
      var returnValue = 0;

      for (var i = trits.Length; i-- > 0;)
      {
        returnValue = returnValue * 3 + trits[i];
      }

      return returnValue;
    }

    /// <summary>
    /// The trits to trytes.
    /// </summary>
    /// <param name="trits">
    /// The trits.
    /// </param>
    /// <returns>
    /// The <see cref="string"/>.
    /// </returns>
    public static string TritsToTrytes(int[] trits)
    {
      var trytes = string.Empty;

      for (var i = 0; i < trits.Length; i += 3)
      {
        var tritChunk = new[] { trits[i], trits[i + 1], trits[i + 2] };
        trytes = trytes + TrytesLookup.First(tryteLookup => tryteLookup.Value.SequenceEqual(tritChunk)).Key;
      }

      return trytes;
    }

    /// <summary>
    /// The trytes to trits.
    /// </summary>
    /// <param name="trytes">
    /// The trytes.
    /// </param>
    /// <returns>
    /// The <see cref="int[]"/>.
    /// </returns>
    public static int[] TrytesToTrits(string trytes)
    {
      var trits = new List<int>();
      foreach (var tryte in trytes)
      {
        trits.AddRange(TrytesLookup[tryte]);
      }

      return trits.ToArray();
    }

    #endregion

    #region Methods

    /// <summary>
    /// The any.
    /// </summary>
    /// <param name="left">
    /// The left.
    /// </param>
    /// <param name="right">
    /// The right.
    /// </param>
    /// <returns>
    /// The <see cref="int"/>.
    /// </returns>
    private static int Any(int left, int right)
    {
      var s = left + right;

      if (s > 0)
      {
        return 1;
      }

      if (s < 0)
      {
        return -1;
      }

      return 0;
    }

    /// <summary>
    /// The cons.
    /// </summary>
    /// <param name="left">
    /// The left.
    /// </param>
    /// <param name="right">
    /// The right.
    /// </param>
    /// <returns>
    /// The <see cref="int"/>.
    /// </returns>
    private static int Cons(int left, int right)
    {
      return left == right ? left : 0;
    }

    /// <summary>
    /// The full add.
    /// </summary>
    /// <param name="left">
    /// The left.
    /// </param>
    /// <param name="right">
    /// The right.
    /// </param>
    /// <param name="carry">
    /// The carry.
    /// </param>
    /// <returns>
    /// The <see cref="int[]"/>.
    /// </returns>
    private static int[] FullAdd(int left, int right, int carry)
    {
      var s_a = Sum(left, right);
      var c_a = Cons(left, right);
      var c_b = Cons(s_a, carry);
      var c_out = Any(c_a, c_b);
      var s_out = Sum(s_a, carry);

      return new[] { s_out, c_out };
    }

    /// <summary>
    /// The sum.
    /// </summary>
    /// <param name="left">
    /// The left.
    /// </param>
    /// <param name="right">
    /// The right.
    /// </param>
    /// <returns>
    /// The <see cref="int"/>.
    /// </returns>
    private static int Sum(int left, int right)
    {
      var sum = left + right;

      switch (sum)
      {
        case 2:
          return -1;
        case -2:
          return 1;
        default:
          return sum;
      }
    }

    #endregion
  }
}