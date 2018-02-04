namespace Tangle.Net.Unit.Tests.Utils
{
  using System;

  using Microsoft.VisualStudio.TestTools.UnitTesting;

  using Tangle.Net.Entity;

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
      var tryteString = TryteString.FromAsciiString("Hello from C#!?");
      Assert.AreEqual("RBTC9D9DCDEAUCFDCDADEAMBHAFAIB", tryteString.Value);
    }

    /// <summary>
    /// The test conversion from trytes.
    /// </summary>
    [TestMethod]
    public void TestConversionFromTrytes()
    {
      var trytes = new TryteString("RBTC9D9DCDEAUCFDCDADEAMBHAFA");
      var output = trytes.ToAsciiString();
      Assert.AreEqual("Hello from C#!", output);
    }

    /// <summary>
    /// The test conversion of single ascii char.
    /// </summary>
    [TestMethod]
    public void TestConversionOfSingleAsciiChar()
    {
      var tryteString = TryteString.FromAsciiString("Z");
      Assert.AreEqual("IC", tryteString.Value);
    }

    /// <summary>
    /// The test non ascii input throws exception.
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void TestNonAsciiInputThrowsException()
    {
      TryteString.FromAsciiString("\u03a0");
    }

    #endregion
  }
}