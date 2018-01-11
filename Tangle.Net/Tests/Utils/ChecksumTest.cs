namespace Tangle.Net.Tests.Utils
{
  using Microsoft.VisualStudio.TestTools.UnitTesting;

  using Tangle.Net.Source.Utils;

  /// <summary>
  /// The checksum test.
  /// </summary>
  [TestClass]
  public class ChecksumTest
  {
    #region Public Methods and Operators

    /// <summary>
    /// The test address has valid checksum should return true.
    /// </summary>
    [TestMethod]
    public void TestAddressHasValidChecksumShouldReturnTrue()
    {
      Assert.IsTrue(Checksum.HasValidChecksum("UYEEERFQYTPFAHIPXDQAQYWYMSMCLMGBTYAXLWFRFFWPYFOICOVLK9A9VYNCKK9TQUNBTARCEQXJHD9VYXOEDEOMRC"));
    }

    /// <summary>
    /// The test address is 81 characters logn should return address.
    /// </summary>
    [TestMethod]
    public void TestAddressIs81CharactersLognShouldReturnAddress()
    {
      var address = Checksum.Strip("RBTC9D9DCDEAUCFDCDADEAMBHAFAHKAJDHAODHADHDAD9KAHAJDADHJSGDJHSDGSDPODHAUDUAHDJAHAB");
      Assert.AreEqual("RBTC9D9DCDEAUCFDCDADEAMBHAFAHKAJDHAODHADHDAD9KAHAJDADHJSGDJHSDGSDPODHAUDUAHDJAHAB", address);
    }

    /// <summary>
    /// The test address is longer than 81 should return substring.
    /// </summary>
    [TestMethod]
    public void TestAddressIsLongerThan81ShouldReturnSubstring()
    {
      var address = Checksum.Strip("RBTC9D9DCDEAUCFDCDADEAMBHAFAHKAJDHAODHADHDAD9KAHAJDADHJSGDJHSDGSDPODHAUDUAHDJAHAB999999999");
      Assert.AreEqual("RBTC9D9DCDEAUCFDCDADEAMBHAFAHKAJDHAODHADHDAD9KAHAJDADHJSGDJHSDGSDPODHAUDUAHDJAHAB", address);
    }

    #endregion
  }
}