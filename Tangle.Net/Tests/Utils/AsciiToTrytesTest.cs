namespace Tangle.Net.Tests.Utils
{
  using System;

  using Microsoft.VisualStudio.TestTools.UnitTesting;

  using Tangle.Net.Source.Utils;

  /// <summary>
  /// The ascii to trytes test.
  /// </summary>
  [TestClass]
  public class AsciiToTrytesTest
  {
    #region Public Methods and Operators

    /// <summary>
    /// The test conversion of string.
    /// </summary>
    [TestMethod]
    public void TestConversionFromString()
    {
      var tryteString = AsciiToTrytes.FromString("Hello from C#!?");
      Assert.AreEqual("RBTC9D9DCDEAUCFDCDADEAMBHAFAIB", tryteString);
    }

    /// <summary>
    /// The test conversion from trytes.
    /// </summary>
    [TestMethod]
    public void TestConversionFromTrytes()
    {
      var output = AsciiToTrytes.FromTrytes("RBTC9D9DCDEAUCFDCDADEAMBHAFA");
      Assert.AreEqual("Hello from C#!", output);
    }

    /// <summary>
    /// The test conversion of single ascii char.
    /// </summary>
    [TestMethod]
    public void TestConversionOfSingleAsciiChar()
    {
      var tryteString = AsciiToTrytes.FromString("Z");
      Assert.AreEqual("IC", tryteString);
    }

    /// <summary>
    /// The test non ascii input throws exception.
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void TestNonAsciiInputThrowsException()
    {
      AsciiToTrytes.FromString("\u03a0");
    }

    /// <summary>
    /// The test tryte length is odd should throw exception.
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void TestTryteLengthIsOddShouldThrowException()
    {
      AsciiToTrytes.FromTrytes("A");
    }

    #endregion
  }
}