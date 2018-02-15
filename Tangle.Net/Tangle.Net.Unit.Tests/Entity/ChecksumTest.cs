namespace Tangle.Net.Unit.Tests.Entity
{
  using System;

  using Microsoft.VisualStudio.TestTools.UnitTesting;

  using Tangle.Net.Entity;

  /// <summary>
  /// The checksum test.
  /// </summary>
  [TestClass]
  public class ChecksumTest
  {
    #region Public Methods and Operators

    /// <summary>
    /// The test given checksum is of invalid length should throw exception.
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void TestGivenChecksumIsOfInvalidLengthShouldThrowException()
    {
      var checksum = new Checksum("AB");
    }

    #endregion
  }
}