namespace Tangle.Net.Mam.Merkle
{
  using Tangle.Net.Entity;

  /// <summary>
  /// The curl merkle tree factory.
  /// </summary>
  public class CurlMerkleTreeFactory : IMerkleTreeFactory
  {
    #region Constructors and Destructors

    /// <summary>
    /// Initializes a new instance of the <see cref="CurlMerkleTreeFactory"/> class.
    /// </summary>
    /// <param name="nodeFactory">
    /// The node factory.
    /// </param>
    /// <param name="leafFactory">
    /// The leaf Factory.
    /// </param>
    public CurlMerkleTreeFactory(IMerkleNodeFactory nodeFactory, IMerkleLeafFactory leafFactory)
    {
      this.NodeFactory = nodeFactory;
      this.LeafFactory = leafFactory;
    }

    #endregion

    #region Properties

    /// <summary>
    /// Gets or sets the leaf factory.
    /// </summary>
    private IMerkleLeafFactory LeafFactory { get; set; }

    /// <summary>
    /// Gets or sets the node factory.
    /// </summary>
    private IMerkleNodeFactory NodeFactory { get; set; }

    #endregion

    #region Public Methods and Operators

    /// <summary>
    /// The create.
    /// </summary>
    /// <param name="seed">
    /// The seed.
    /// </param>
    /// <param name="startIndex">
    /// The startIndex.
    /// </param>
    /// <param name="count">
    /// The count.
    /// </param>
    /// <param name="securityLevel">
    /// The security level.
    /// </param>
    /// <returns>
    /// The <see cref="MerkleTree"/>.
    /// </returns>
    public MerkleTree Create(Seed seed, int startIndex, int count, int securityLevel)
    {
      var tree = new MerkleTree();
      var leaves = this.LeafFactory.Create(startIndex, count);

      if (leaves.Count == 1)
      {
        tree.Root = leaves[0];
      }

      return tree;

      // const key = Crypto.signing.key(seed, index, security);
      // const digests = Crypto.signing.digests(key);
      // const address = Crypto.signing.address(digests);
      ////var address = Crypto.converter.trytes(addressTrits);
      // this.key = key;
      // this.hash = new Hash(address);
      // this.size = () => 1;
      // this.get = () => this;
    }

    #endregion
  }
}