namespace Tangle.Net.Mam.Merkle
{
  using System.Collections.Generic;

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
      var leaves = this.LeafFactory.Create(startIndex, count);
      return new MerkleTree { Root = this.BuildTree(leaves) };
    }

    #endregion

    #region Methods

    /// <summary>
    /// The build tree.
    /// </summary>
    /// <param name="leaves">
    /// The leaves.
    /// </param>
    /// <returns>
    /// The <see cref="MerkleNode"/>.
    /// </returns>
    private MerkleNode BuildTree(IReadOnlyList<MerkleNode> leaves)
    {
      var subnodes = new List<MerkleNode>();
      for (var i = 0; i < leaves.Count; i += 2)
      {
        var right = (i + 1 < leaves.Count) ? leaves[i + 1] : null;
        var parent = this.NodeFactory.Create(leaves[i], right);
        subnodes.Add(parent);
      }

      if (subnodes.Count == 1)
      {
        return subnodes[0];
      }

      return this.BuildTree(subnodes);
    }

    #endregion
  }
}