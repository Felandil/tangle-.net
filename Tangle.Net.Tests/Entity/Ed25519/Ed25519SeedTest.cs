using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tangle.Net.Crypto.Bip44;
using Tangle.Net.Entity.Ed25519;
using Tangle.Net.Utils;

namespace Tangle.Net.Tests.Entity.Ed25519
{
  [TestClass]
  public class Ed25519SeedTest
  {
    [TestMethod]
    public void TestSeedGenerationFromMnemonic()
    {
      var seed = Ed25519Seed.FromMnemonic("witch collapse practice feed shame open despair creek road again ice least");

      var expected = new byte[]
      {
        205, 207, 133, 190, 162, 105, 181, 176, 78, 164, 207,
        31, 198, 111, 187, 209, 191, 233, 158, 80, 14, 240,
        49, 109, 253, 90, 174, 183, 55, 155, 94, 31, 189,
        176, 2, 220, 158, 61, 232, 126, 203, 212, 88, 48,
        65, 112, 11, 98, 103, 147, 73, 39, 18, 250, 159,
        171, 177, 193, 124, 98, 165, 226, 151, 173
      };

      CollectionAssert.AreEqual(expected, seed.SecretKey);
    }

    [TestMethod]
    public void TestKeyPairGeneration()
    {
      var seed = Ed25519Seed.FromMnemonic("witch collapse practice feed shame open despair creek road again ice least");

      var expectedPrivateKey = new byte[]
      {
        205, 207, 133, 190, 162, 105, 181, 176, 78, 164, 207,
        31, 198, 111, 187, 209, 191, 233, 158, 80, 14, 240,
        49, 109, 253, 90, 174, 183, 55, 155, 94, 31, 47,
        105, 159, 170, 174, 177, 49, 97, 153, 60, 98, 162,
        162, 163, 90, 84, 146, 55, 23, 96, 1, 4, 119,
        221, 38, 180, 84, 149, 189, 66, 197, 72
      };

      var expectedPublicKey = new byte[]
      {
        47, 105, 159, 170, 174, 177, 49, 97,
        153, 60, 98, 162, 162, 163, 90, 84,
        146, 55, 23, 96, 1, 4, 119, 221,
        38, 180, 84, 149, 189, 66, 197, 72
      };

      CollectionAssert.AreEqual(expectedPublicKey, seed.KeyPair.PublicKey);
      CollectionAssert.AreEqual(expectedPrivateKey, seed.KeyPair.PrivateKey);
    }

    [TestMethod]
    public void TestSubseedGenerationFromPath()
    {
      var seed = Ed25519Seed.FromMnemonic("witch collapse practice feed shame open despair creek road again ice least");
      var path = Bip44AddressGenerator.GenerateAddress(1, 1, false);

      var subseed = seed.GenerateSeedFromPath(path);

      var expectedSeed = "56b7b7c582f3ebdfbe904ead6b3455a2a4595029a39126e6c2ebbadd98702ecd";

      Assert.AreEqual(expectedSeed, subseed.SecretKey.ToHex());
    }
  }
}
