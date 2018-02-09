namespace Tangle.Net.Mam.Unit.Tests.Merkle
{
  using Microsoft.VisualStudio.TestTools.UnitTesting;

  using Tangle.Net.Cryptography;
  using Tangle.Net.Entity;
  using Tangle.Net.Mam.Merkle;
  using Tangle.Net.Unit.Tests.Cryptography;

  /// <summary>
  /// The merkle tree factory tests.
  /// </summary>
  [TestClass]
  public class MerkleTreeFactoryTests
  {
    #region Public Methods and Operators

    /// <summary>
    /// The test tree has only more than one leaf should create nodes.
    /// </summary>
    [TestMethod]
    public void TestTreeHasOnlyMoreThanOneLeafShouldCreateNodes()
    {
      var factory = new CurlMerkleTreeFactory(new CurlMerkleNodeFactory(new Curl()), new CurlMerkleLeafFactory(new AddressGeneratorStub()));
      var random = new Seed("L9DRGFPYDMGVLH9ZCEWHXNEPC9TQQSA9W9FZVYXLBMJTHJC9HZDONEJMMVJVEMHWCIBLAUYBAUFQOMYSN");
      var tree = factory.Create(random, 0, 10, SecurityLevel.Medium);

      Assert.IsNotNull(tree.Root);
      Assert.AreEqual(10, tree.Size);
    }

    /// <summary>
    /// The test tree has only one leaf should set leaf as root node.
    /// </summary>
    [TestMethod]
    public void TestTreeHasOnlyOneLeafShouldSetLeafAsRootNode()
    {
      var factory = new CurlMerkleTreeFactory(new CurlMerkleNodeFactory(new Curl()), new CurlMerkleLeafFactory(new AddressGeneratorStub()));
      var tree = factory.Create(Seed.Random(), 0, 1, SecurityLevel.Medium);

      Assert.IsNotNull(tree.Root);
    }

    #endregion
  }
}