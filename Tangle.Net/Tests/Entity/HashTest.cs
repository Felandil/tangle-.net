namespace Tangle.Net.Tests.Entity
{
  using System;

  using Microsoft.VisualStudio.TestTools.UnitTesting;

  using Tangle.Net.Source.Entity;

  /// <summary>
  /// The hash test.
  /// </summary>
  [TestClass]
  public class HashTest
  {
    #region Public Methods and Operators

    /// <summary>
    /// The test empty hash generation.
    /// </summary>
    [TestMethod]
    public void TestEmptyHashGeneration()
    {
      var emptyHash = Hash.Empty;
      Assert.AreEqual("999999999999999999999999999999999999999999999999999999999999999999999999999999999", emptyHash.Value);
    }

    /// <summary>
    /// The test given value is of incorrect length should throw exception.
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void TestGivenValueIsOfIncorrectLengthShouldThrowException()
    {
      var hash = new Hash("JHAGS");
    }

    #endregion
  }
}