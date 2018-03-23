namespace Tangle.Net.Mam.Unit.Tests.Merkle
{
  using Microsoft.VisualStudio.TestTools.UnitTesting;

  using Tangle.Net.Cryptography;
  using Tangle.Net.Cryptography.Curl;
  using Tangle.Net.Entity;
  using Tangle.Net.Mam.Merkle;
  using Tangle.Net.Unit.Tests.Cryptography;

  /// <summary>
  /// The merkle node tests.
  /// </summary>
  [TestClass]
  public class MerkleNodeFactoryTests
  {
    #region Fields

    /// <summary>
    /// The left node.
    /// </summary>
    private readonly MerkleNode leftNode = new MerkleNode
                                             {
                                               Hash =
                                                 new Hash(
                                                 "HG9KCXQZGQDVTFGRHOZDZ99RMKGVRIQXEKXWXTPWYRGXQQVFVMTLQLUPJSIDONDEURVKHMBPRYGP99999")
                                             };

    /// <summary>
    /// The right node.
    /// </summary>
    private readonly MerkleNode rightNode = new MerkleNode
                                              {
                                                Hash =
                                                  new Hash(
                                                  "HHZSJANZQULQICZFXJHHAFJTWEITWKQYJKU9TYFA9AFJLVIYOUCFQRYTLKRGCVY9KPOCCHK99TTKQGXA9")
                                              };

    #endregion

    #region Properties

    /// <summary>
    /// Gets or sets the node factory.
    /// </summary>
    private IMerkleNodeFactory NodeFactory { get; set; }

    #endregion

    #region Public Methods and Operators

    /// <summary>
    /// The setup.
    /// </summary>
    [TestInitialize]
    public void Setup()
    {
      this.NodeFactory = new CurlMerkleNodeFactory(new Curl());
    }

    /// <summary>
    /// The test both nodes are given should combine hashes.
    /// </summary>
    [TestMethod]
    public void TestBothNodesAreGivenShouldCombineHashes()
    {
      var node = this.NodeFactory.Create(this.leftNode, this.rightNode);
      Assert.AreEqual("S9UDQJRKLTXZIUGOKNVBFSJRJYENJUCGOAZGVCNVSXCNVJOEMDWEBMVYL9YNMLPPFUUBAKFVXGJVO9OJA", node.Hash.Value);
      Assert.AreEqual("S9UDQJRKLTXZIUGOKNVBFSJRJYENJUCGOAZGVCNVSXCNVJOEMDWEBMVYL9YNMLPPFUUBAKFVXGJVO9OJA", node.LeftNode.ParentNode.Hash.Value);
      Assert.AreEqual("S9UDQJRKLTXZIUGOKNVBFSJRJYENJUCGOAZGVCNVSXCNVJOEMDWEBMVYL9YNMLPPFUUBAKFVXGJVO9OJA", node.RightNode.ParentNode.Hash.Value);
    }

    /// <summary>
    /// The test only left node is given should combine with left node hash.
    /// </summary>
    [TestMethod]
    public void TestOnlyLeftNodeIsGivenShouldCombineWithLeftNodeHash()
    {
      var node = this.NodeFactory.Create(this.leftNode);
      Assert.AreEqual("9ZAO9ZKQZPVRVPLVEVXYEWMTVMEQNQBE9JKM9IQEBLJBIYWSUCZGFIHVCKYTAHLV9GEVMFON9NVWRDGAM", node.Hash.Value);
      Assert.AreEqual("9ZAO9ZKQZPVRVPLVEVXYEWMTVMEQNQBE9JKM9IQEBLJBIYWSUCZGFIHVCKYTAHLV9GEVMFON9NVWRDGAM", node.LeftNode.ParentNode.Hash.Value);
    }

    #endregion
  }
}