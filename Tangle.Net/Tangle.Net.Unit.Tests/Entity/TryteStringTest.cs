namespace Tangle.Net.Unit.Tests.Entity
{
  using System;
  using System.Text;

  using Microsoft.VisualStudio.TestTools.UnitTesting;

  using Tangle.Net.Entity;
  using Tangle.Net.Utils;

  /// <summary>
  /// The tryte string test.
  /// </summary>
  [TestClass]
  public class TryteStringTest
  {
    #region Public Methods and Operators

    /// <summary>
    /// The test chunks are sliced correctly.
    /// </summary>
    [TestMethod]
    public void TestChunksAreSlicedCorrectly()
    {
      var tryteString = new TryteString("IAMGROOTU");
      var chunks = tryteString.GetChunks(2);

      Assert.AreEqual(5, chunks.Count);
    }

    /// <summary>
    /// The test from byte array and back.
    /// </summary>
    [TestMethod]
    public void TestFromByteArrayAndBack()
    {
      var bytes = Encoding.UTF8.GetBytes("┬");
      var tryteString = bytes.ToTrytes();
      var bytesBack = tryteString.ToBytes();

      for (var i = 0; i < bytes.Length; i++)
      {
        Assert.AreEqual(bytes[i], bytesBack[i]);
      }

      tryteString = TryteString.FromBytes(bytes);
      bytesBack = tryteString.ToBytes();

      for (var i = 0; i < bytes.Length; i++)
      {
        Assert.AreEqual(bytes[i], bytesBack[i]);
      }

    }

    /// <summary>
    /// The test given string is no tryte string should throw exception.
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void TestGivenStringIsNoTryteStringShouldThrowException()
    {
      var tryteString = new TryteString("jasda87678");
    }

    /// <summary>
    /// The test given string is trytes string should set value.
    /// </summary>
    [TestMethod]
    public void TestGivenStringIsTrytesStringShouldSetValue()
    {
      var tryteString = new TryteString("IAMGROOT");
      Assert.AreEqual("IAMGROOT", tryteString.Value);
    }

    /// <summary>
    /// The test utf 8 string conversion.
    /// </summary>
    [TestMethod]
    public void TestUtf8StringConversion()
    {
      var stringValue = "┬";
      var tryteString = TryteString.FromUtf8String(stringValue);

      Assert.AreEqual(stringValue, tryteString.ToUtf8String());
    }

    #endregion
  }
}