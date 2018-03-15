namespace Tangle.Net.Mam.Unit.Tests.Mam
{
  using Microsoft.VisualStudio.TestTools.UnitTesting;

  using Tangle.Net.Cryptography;
  using Tangle.Net.Entity;
  using Tangle.Net.Mam.Mam;
  using Tangle.Net.Mam.Merkle;

  /// <summary>
  /// The curl mam factory test.
  /// </summary>
  [TestClass]
  public class CurlMamFactoryTest
  {
    /// <summary>
    /// The test mam creation.
    /// </summary>
    [TestMethod]
    public void TestMamCreationAndDecryption()
    {
      var seed = new Seed("L9DRGFPYDMGVLH9ZCEWHXNEPC9TQQSA9W9FZVYXLBMJTHJC9HZDONEJMMVJVEMHWCIBLAUYBAUFQOMYSN");
      var factory = new CurlMerkleTreeFactory(new CurlMerkleNodeFactory(new Curl()), new CurlMerkleLeafFactory(new AddressGenerator(seed)));
      var startIndex = 3;
      var count = 4;
      var tree = factory.Create(seed, startIndex, count, SecurityLevel.Medium);
      var nextRootTree = factory.Create(seed, startIndex + count, count, SecurityLevel.Medium);

      Assert.AreEqual("AUMVYYX9SZPVCBE9BYXSMCDAUCLTCADKLQPHTLW9JYCFZUPSQWEBJUIDDSZWFFTBVZTAZRPWFLCWUCTZN", tree.Root.Hash.Value);

      var mamFactory = new CurlMamFactory(new Curl(), new CurlMask());
      var mam = mamFactory.Create(
        tree,
        0,
        TryteString.FromUtf8String("Hello everyone!"),
        nextRootTree.Root.Hash,
        new TryteString("AVGBMN9RNYJPVKRXVHPIUCZAPVEZWLPVVVDOBYXY9ASRCXWXJYIRUPYDAILAZZFPASDGCPVDAKKCUXSOC"));

      Assert.AreEqual("YSMYHNG9NVXMXTWWPMHAOFRPBDWXEQCGUOIUNFTGVGKOXJWJTKIUYJVOA9BRTVYKXZB9YLMNQZZOYNVJC", mam.Address.Value);

      var decrypt = new CurlMamParser(new CurlMask());
      var unmaskedAuthenticatedMessage = decrypt.Unmask(
        mam.Payload,
        new TryteString("AVGBMN9RNYJPVKRXVHPIUCZAPVEZWLPVVVDOBYXY9ASRCXWXJYIRUPYDAILAZZFPASDGCPVDAKKCUXSOC"));

      Assert.AreEqual("Hello everyone!", unmaskedAuthenticatedMessage.Message.ToUtf8String());
      // Assert.AreEqual(nextRootTree.Root.Hash.Value, unmaskedAuthenticatedMessage.Root.Value);
      // Assert.AreEqual(mam.Root.Value, unmaskedAuthenticatedMessage.Root.Value);
    }
  }
}