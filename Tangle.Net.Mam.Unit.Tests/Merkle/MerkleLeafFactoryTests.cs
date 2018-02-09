using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tangle.Net.Cryptography;
using Tangle.Net.Entity;
using Tangle.Net.Mam.Merkle;

namespace Tangle.Net.Mam.Unit.Tests.Merkle
{
  /// <summary>
  ///   The merkle leaf factory tests.
  /// </summary>
  [TestClass]
  public class MerkleLeafFactoryTests
  {
    #region Public Methods and Operators

    /// <summary>
    ///   The test leaf factory sets properties accordingly.
    /// </summary>
    [TestMethod]
    public void TestLeafFactorySetsPropertiesAccordingly()
    {
      var addressGenerator =
        new AddressGenerator(
          new Seed("L9DRGFPYDMGVLH9ZCEWHXNEPC9TQQSA9W9FZVYXLBMJTHJC9HZDONEJMMVJVEMHWCIBLAUYBAUFQOMYSN"));
      var address = addressGenerator.GetAddress(0);

      var factory = new CurlMerkleLeafFactory(addressGenerator);
      var leaves = factory.Create(0, 1);

      Assert.AreEqual(address.Value, leaves[0].Hash.Value);
      Assert.AreEqual(address.PrivateKey.Value, leaves[0].Key.Value);
      Assert.AreEqual(1, leaves[0].Size);
    }

    #endregion
  }
}