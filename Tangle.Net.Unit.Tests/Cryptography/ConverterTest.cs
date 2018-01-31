namespace Tangle.Net.Unit.Tests.Cryptography
{
  using System;
  using System.Linq;

  using Microsoft.VisualStudio.TestTools.UnitTesting;

  using Org.BouncyCastle.Math;
  using Org.BouncyCastle.Utilities;

  using Tangle.Net.Cryptography;

  /// <summary>
  /// The converter test.
  /// </summary>
  [TestClass]
  public class ConverterTest
  {
    #region Static Fields

    /// <summary>
    /// The seed.
    /// </summary>
    private static readonly Random Seed = new Random();

    #endregion

    #region Public Methods and Operators

    /// <summary>
    /// The get random trits.
    /// </summary>
    /// <param name="length">
    /// The length.
    /// </param>
    /// <returns>
    /// The <see cref="int[]"/>.
    /// </returns>
    public static int[] GetRandomTrits(int length)
    {
      var trits = new int[length];

      for (int i = 0; i < length; i++)
      {
        trits[i] = Seed.Next(3) - 1;
      }

      return trits;
    }

    /// <summary>
    /// The bytes from big int.
    /// </summary>
    [TestMethod]
    public void BytesFromBigInt()
    {
      var bigInteger =
        new BigInteger("-39661267093385976646699885460151224333890761109786635767964779188997717436995426392349825946870259974898439594016");
      var outBytes = Converter.ConvertBigIntToBytes(bigInteger);
      var outBigInteger = Converter.ConvertBytesToBigInt(outBytes);
      Assert.IsTrue(bigInteger.Equals(outBigInteger));
    }

    /// <summary>
    /// The generate bytes from big int.
    /// </summary>
    [TestMethod]
    public void GenerateBytesFromBigInt()
    {
      for (var i = 0; i < 100; i++)
      {
        const int ByteSize = 48;
        var outBytes = new byte[ByteSize];
        Seed.NextBytes(outBytes);
        var bigInt = new BigInteger(outBytes);
      }
    }

    /// <summary>
    /// The loop rand bytes from big int.
    /// </summary>
    [TestMethod]
    public void LoopRandBytesFromBigInt()
    {
      const int ByteSize = 48;
      const int TritSize = 243;
      var inBytes = new byte[ByteSize];
      for (var i = 0; i < 10000; i++)
      {
        Seed.NextBytes(inBytes);
        var inBigInteger = Converter.ConvertBytesToBigInt(inBytes);
        var trits = Converter.ConvertBigIntToTrits(inBigInteger, TritSize);
        var outBigInteger = Converter.ConvertTritsToBigInt(trits, 0, TritSize);
        var outBytes = Converter.ConvertBigIntToBytes(outBigInteger);

        for (var y = 0; y < inBytes.Length; y++)
        {
          Assert.IsTrue(inBytes[y] == outBytes[y]);
        }
      }
    }

    /// <summary>
    /// The loop rand trits from big int.
    /// </summary>
    [TestMethod]
    public void LoopRandTritsFromBigInt()
    {
      const int TritSize = 243;
      for (var i = 0; i < 10000; i++)
      {
        var inTrits = GetRandomTrits(TritSize);
        inTrits[242] = 0;

        var inBigInteger = Converter.ConvertTritsToBigInt(inTrits, 0, TritSize);
        var bytes = Converter.ConvertBigIntToBytes(inBigInteger);
        var outBigInteger = Converter.ConvertBytesToBigInt(bytes);
        var outTrits = Converter.ConvertBigIntToTrits(outBigInteger, TritSize);

        Assert.IsTrue(Arrays.AreEqual(inTrits, outTrits));
      }
    }

    /// <summary>
    /// The test convert trytes to trits.
    /// </summary>
    [TestMethod]
    public void TestConvertTrytesToTritsAndBack()
    {
      string expected = "9ABCDEFGHIJKLMNOPQRSTUVWXYZ";
      var value = Converter.TrytesToTrits(expected);
      var convertedExpected = Converter.TritsToTrytes(value);

      Assert.AreEqual(expected, convertedExpected);
    }

    /// <summary>
    /// The test int coverts to trits.
    /// </summary>
    [TestMethod]
    public void TestIntConvertsToTrits()
    {
      var value = Converter.IntToTrits(9, 3);
      var expected = new[] { 0, 0, 1 };

      for (var i = 0; i < expected.Count(); i++)
      {
        Assert.AreEqual(expected[i], value[i]);
      }
    }

    /// <summary>
    /// The test trits convert to int.
    /// </summary>
    [TestMethod]
    public void TestTritsConvertToInt()
    {
      var value = Converter.TritsToInt(new[] { 0, 0, 1 });
      Assert.AreEqual(9, value);
    }

    #endregion
  }
}