namespace Tangle.Net.Source.Cryptography
{
  using System;
  using System.Collections.Generic;
  using System.Linq;

  using Org.BouncyCastle.Math;

  /// <summary>
  /// The converter.
  /// </summary>
  public static class Converter
  {
    #region Constants

    /// <summary>
    /// The byte length.
    /// </summary>
    private const int ByteLength = 48;

    /// <summary>
    /// The int length.
    /// </summary>
    private const int IntLength = ByteLength / 4;

    /// <summary>
    /// The max trit value.
    /// </summary>
    private const int MaxTritValue = 1;

    /// <summary>
    /// The min trit value.
    /// </summary>
    private const int MinTritValue = -1;

    /// <summary>
    /// The number of trits in a tryte.
    /// </summary>
    private const int NumberOfTritsInATryte = 3;

    /// <summary>
    /// The radix.
    /// </summary>
    private const int Radix = 3;

    #endregion

    #region Static Fields

    /// <summary>
    /// The half 3.
    /// </summary>
    private static readonly uint[] Half3 = new uint[]
                                             {
                                               0xa5ce8964, 0x9f007669, 0x1484504f, 0x3ade00d9, 0x0c24486e, 0x50979d57, 0x79a4c702, 0x48bbae36, 
                                               0xa9f6808b, 0xaa06a805, 0xa87fabdf, 0x5e69ebef
                                             };

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
      return value.ToByteArray();
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
      var destination = new int[size];
      var absoluteValue = value.CompareTo(BigInteger.Zero) < 0 ? value.Negate() : value;
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

      if (value.CompareTo(BigInteger.Zero) < 0)
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
    public static BigInteger ConvertBytesToBigInt(byte[] bytes)
    {
      return new BigInteger(bytes);
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
    public static int[] ConvertBytesToTrits(byte[] bytes)
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

      return destination.ToArray();
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
        trits.AddRange(TrytesLookup.First(tryteLookup => tryteLookup.Key == tryte).Value);
      }

      return trits.ToArray();
    }

    #endregion

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
  }
}