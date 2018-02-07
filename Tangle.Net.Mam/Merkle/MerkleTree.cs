namespace Tangle.Net.Mam.Merkle
{
  using System.Collections.Generic;

  /// <summary>
  /// The merkle tree.
  /// </summary>
  public class MerkleTree
  {
    #region Constructors and Destructors

    /// <summary>
    /// Initializes a new instance of the <see cref="MerkleTree"/> class.
    /// </summary>
    public MerkleTree()
    {
      this.Nodes = new List<MerkleNode>();
      this.Leaves = new List<MerkleNode>();
    }

    #endregion

    #region Public Properties

    /// <summary>
    /// Gets or sets the leaves.
    /// </summary>
    public List<MerkleNode> Leaves { get; set; }

    /// <summary>
    /// Gets or sets the nodes.
    /// </summary>
    public List<MerkleNode> Nodes { get; set; }

    /// <summary>
    /// Gets or sets the root.
    /// </summary>
    public MerkleNode Root { get; set; }

    #endregion
  }
}