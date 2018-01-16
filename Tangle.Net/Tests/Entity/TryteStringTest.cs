namespace Tangle.Net.Tests.Entity
{
  using System;

  using Microsoft.VisualStudio.TestTools.UnitTesting;

  using Tangle.Net.Source.Entity;

  /// <summary>
  /// The tryte string test.
  /// </summary>
  [TestClass]
  public class TryteStringTest
  {
    #region Public Methods and Operators

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

    #endregion
  }
}