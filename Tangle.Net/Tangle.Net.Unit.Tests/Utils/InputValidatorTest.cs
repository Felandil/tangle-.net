namespace Tangle.Net.Unit.Tests.Utils
{
  using Microsoft.VisualStudio.TestTools.UnitTesting;

  using Tangle.Net.Utils;

  /// <summary>
  /// The input validator test.
  /// </summary>
  [TestClass]
  public class InputValidatorTest
  {
    #region Public Methods and Operators

    /// <summary>
    /// The test input is no tryte string should return false.
    /// </summary>
    [TestMethod]
    public void TestInputIsNoTryteStringShouldReturnFalse()
    {
      Assert.IsFalse(InputValidator.IsTrytes("I am not a tryte string at all."));
    }

    /// <summary>
    /// The test input is tryte string but has incorrect length should return false.
    /// </summary>
    [TestMethod]
    public void TestInputIsTryteStringButHasIncorrectLengthShouldReturnFalse()
    {
      Assert.IsFalse(InputValidator.IsTrytes("RBTC9D9DCDEAUCFDCDADEAMBHAFA", 81));
    }

    /// <summary>
    /// The test input is tryte string but has incorrect length should return true.
    /// </summary>
    [TestMethod]
    public void TestInputIsTryteStringButHasIncorrectLengthShouldReturnTrue()
    {
      Assert.IsTrue(InputValidator.IsTrytes("RBTC9D9DCDEAUCFDCDADEAMBHAFA", "RBTC9D9DCDEAUCFDCDADEAMBHAFA".Length));
    }

    /// <summary>
    /// The test input is tryte string should return true.
    /// </summary>
    [TestMethod]
    public void TestInputIsTryteStringShouldReturnTrue()
    {
      Assert.IsTrue(InputValidator.IsTrytes("RBTC9D9DCDEAUCFDCDADEAMBHAFA"));
    }

    #endregion
  }
}