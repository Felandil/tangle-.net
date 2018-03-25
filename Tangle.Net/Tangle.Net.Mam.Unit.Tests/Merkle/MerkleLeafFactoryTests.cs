namespace Tangle.Net.Mam.Unit.Tests.Merkle
{
  using Microsoft.VisualStudio.TestTools.UnitTesting;

  using Tangle.Net.Cryptography;
  using Tangle.Net.Cryptography.Curl;
  using Tangle.Net.Cryptography.Signing;
  using Tangle.Net.Entity;
  using Tangle.Net.Mam.Merkle;

  /// <summary>
  ///   The merkle leaf factory tests.
  /// </summary>
  [TestClass]
  public class MerkleLeafFactoryTests
  {
    /// <summary>
    ///   The test leaf factory sets properties accordingly.
    /// </summary>
    [TestMethod]
    public void TestLeafFactorySetsPropertiesAccordingly()
    {
      var seed = new Seed("L9DRGFPYDMGVLH9ZCEWHXNEPC9TQQSA9W9FZVYXLBMJTHJC9HZDONEJMMVJVEMHWCIBLAUYBAUFQOMYSN");

      var addressGenerator = MerkleAddressGenerator.Default;
      var address = addressGenerator.GetAddress(seed, SecurityLevel.Medium, 0);
      var factory = new CurlMerkleLeafFactory(addressGenerator);
      var leaves = factory.Create(seed, SecurityLevel.Medium, 0, 1);

      Assert.AreEqual("MYNGVRBGNNKWHSTPOUOCBFWXKIUGZEYRCS9NGO9RYKLSOYAEBPOTNNONK9EVJTXQHYLOCRGCJWTTETSYA", leaves[0].Hash.Value);
      Assert.AreEqual(address.PrivateKey.Value, leaves[0].Key.Value);
      Assert.AreEqual(1, leaves[0].Size);
    }
  }
}