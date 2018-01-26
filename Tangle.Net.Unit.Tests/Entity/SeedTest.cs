namespace Tangle.Net.Unit.Tests.Entity
{
  using System;

  using Microsoft.VisualStudio.TestTools.UnitTesting;

  using Tangle.Net.Entity;

  /// <summary>
  /// The seed test.
  /// </summary>
  [TestClass]
  public class SeedTest
  {
    #region Public Methods and Operators

    /// <summary>
    /// The test seed is too short should throw exception.
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void TestSeedIsTooShortShouldThrowException()
    {
      var seed = new Seed("IAMTOOSHORT");
    }

    /// <summary>
    /// The test seed is too long should throw exception.
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void TestSeedIsTooLongShouldThrowException()
    {
      var seed = new Seed("KHWHSTISMVVSDCOMHVFIFCTINWZT9EHJUATYSMCXDSMZXPL9KXREBBYHJGRBCYVGPJQEHEDPXLBDJNQNXHAGSAHGS");
    }

    /// <summary>
    /// The test seed is correctly formed should create new seed object.
    /// </summary>
    [TestMethod]
    public void TestSeedIsCorrectlyFormedShouldCreateNewSeedObject()
    {
      var seed = new Seed("9XV9RJGFJJZWITDPKSQXRTHCKJAIZZY9BYLBEQUXUNCLITRQDR9CCD99AANMXYEKD9GLJGVB9HIAGRIBQ");
      Assert.AreEqual("9XV9RJGFJJZWITDPKSQXRTHCKJAIZZY9BYLBEQUXUNCLITRQDR9CCD99AANMXYEKD9GLJGVB9HIAGRIBQ", seed.Value);
    }

    /// <summary>
    /// The test random seed generator creates properly formed seed values.
    /// </summary>
    [TestMethod]
    public void TestRandomSeedGeneratorCreatesProperlyFormedSeedValues()
    {
      var tempStore = string.Empty;

      for (var i = 0; i < 10; i++)
      {
        var seed = Seed.Random();
        Assert.AreEqual(81, seed.Value.Length);
        Assert.AreNotEqual(tempStore, seed.Value);
        tempStore = seed.Value;
      }
    }

    #endregion
  }
}