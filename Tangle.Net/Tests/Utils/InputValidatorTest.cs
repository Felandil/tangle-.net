namespace Tangle.Net.Tests.Utils
{
  using System.Collections.Generic;

  using Microsoft.VisualStudio.TestTools.UnitTesting;

  using Tangle.Net.Source.Entity;
  using Tangle.Net.Source.Utils;

  /// <summary>
  /// The input validator test.
  /// </summary>
  [TestClass]
  public class InputValidatorTest
  {
    #region Public Methods and Operators

    /// <summary>
    /// The test hash has correct length should return true.
    /// </summary>
    [TestMethod]
    public void TestHashHasCorrectLengthShouldReturnTrue()
    {
      Assert.IsTrue(InputValidator.IsHash("RBTC9D9DCDEAUCFDCDADEAMBHAFAHKAJDHAODHADHDAD9KAHAJDADHJSGDJHSDGSDPODHAUDUAHDJAHAB"));
    }

    /// <summary>
    /// The test hash has incorrect length should return false.
    /// </summary>
    [TestMethod]
    public void TestHashHasIncorrectLengthShouldReturnFalse()
    {
      Assert.IsFalse(InputValidator.IsHash("RBTC9D9DCDEAUCFDCDADEAMBHAFA"));
    }

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

    /// <summary>
    /// The test transfers addresses are malformed should return false.
    /// </summary>
    [TestMethod]
    public void TestTransfersAddressesAreMalformedShouldReturnFalse()
    {
      var transfers = new List<Transfer> { new Transfer { Address = "RBTC9D9DCDEAUCFDCDADEAMBHAFA" } };
      Assert.IsFalse(InputValidator.IsTransfersArray(transfers));
    }

    /// <summary>
    /// The test transfers messages are malformed should return false.
    /// </summary>
    [TestMethod]
    public void TestTransfersMessagesAreMalformedShouldReturnFalse()
    {
      var transfers = new List<Transfer>
                        {
                          new Transfer
                            {
                              Address =
                                "RBTC9D9DCDEAUCFDCDADEAMBHAFAHKAJDHAODHADHDAD9KAHAJDADHJSGDJHSDGSDPODHAUDUAHDJAHAB",
                              Message = "666 aaa"
                            }
                        };
      Assert.IsFalse(InputValidator.IsTransfersArray(transfers));
    }

    /// <summary>
    /// The test transfers tags are malformed should return false.
    /// </summary>
    [TestMethod]
    public void TestTransfersTagsAreMalformedShouldReturnFalse()
    {
      var transfers = new List<Transfer>
                        {
                          new Transfer
                            {
                              Address =
                                "RBTC9D9DCDEAUCFDCDADEAMBHAFAHKAJDHAODHADHDAD9KAHAJDADHJSGDJHSDGSDPODHAUDUAHDJAHAB",
                              Message = "RBTC9D9DCDEAUCFDCDADEAMBHAFA",
                              Tag = "RBTC9D9DCDEAUCFDCDADEAMBHAFA"
                            }
                        };
      Assert.IsFalse(InputValidator.IsTransfersArray(transfers));
    }

    /// <summary>
    /// The test transfers are correctly formed should return true.
    /// </summary>
    [TestMethod]
    public void TestTransfersAreCorrectlyFormedShouldReturnTrue()
    {
      var transfers = new List<Transfer>
                        {
                          new Transfer
                            {
                              Address =
                                "RBTC9D9DCDEAUCFDCDADEAMBHAFAHKAJDHAODHADHDAD9KAHAJDADHJSGDJHSDGSDPODHAUDUAHDJAHAB",
                              Message = "RBTC9D9DCDEAUCFDCDADEAMBHAFA",
                              Tag = "RBTC9D9DCDEAUCFDCDADEAMBHAF"
                            }
                        };
      Assert.IsTrue(InputValidator.IsTransfersArray(transfers));
    }

    #endregion
  }
}