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
    /// The test tree has only one leaf should set leaf as root node.
    /// </summary>
    [TestMethod]
    public void TestTreeHasOnlyOneLeafShouldSetLeafAsRootNode()
    {
      var factory = new CurlMerkleTreeFactory(new CurlMerkleNodeFactory(new Curl()), new CurlMerkleLeafFactory(new AddressGeneratorStub()));
      var tree = factory.Create(Seed.Random(), 0, 1, SecurityLevel.Medium);

      Assert.IsNotNull(tree.Root);
      Assert.AreEqual(0, tree.Nodes.Count);
      Assert.AreEqual(0, tree.Leaves.Count);
    }

    #endregion
  }
}