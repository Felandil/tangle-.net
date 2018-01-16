namespace Tangle.Net.Tests.Entity
{
  using System;

  using Microsoft.VisualStudio.TestTools.UnitTesting;

  using Tangle.Net.Source.Entity;

  /// <summary>
  /// The digest test.
  /// </summary>
  [TestClass]
  public class DigestTest
  {
    #region Public Methods and Operators

    /// <summary>
    /// The test digest is of incorrect length should throw exception.
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void TestDigestIsOfIncorrectLengthShouldThrowException()
    {
      var digest = new Digest("JAHGSASASAFUIGHAUIH", 1, 1);
    }

    /// <summary>
    /// The test digest is of incorrect length with key index should throw exception.
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void TestDigestIsOfIncorrectLengthWithKeyIndexShouldThrowException()
    {
      var digest = new Digest("JAHGSASASAFUIGHAUIH", 1, 1);
    }

    [TestMethod]
    public void TestKeyIndexIsGivenShouldSetProperly()
    {
      var digest = new Digest("9XV9RJGFJJZWITDPKSQXRTHCKJAIZZY9BYLBEQUXUNCLITRQDR9CCD99AANMXYEKD9GLJGVB9HIAGRIBQ", 1, 1);
      Assert.AreEqual(1, digest.KeyIndex);
    }

    #endregion
  }
}