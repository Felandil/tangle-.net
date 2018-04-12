namespace Tangle.Net.Mam.Merkle
{
  using System.Collections.Generic;
  using System.Linq;

  using Tangle.Net.Cryptography;
  using Tangle.Net.Entity;
  using Tangle.Net.Utils;

  /// <summary>
  /// The merkle sub tree.
  /// </summary>
  public class MerkleSubTree
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="MerkleSubTree"/> class.
    /// </summary>
    public MerkleSubTree()
    {
      this.Leaves = new List<MerkleNode>();
    }

    /// <summary>
    /// Gets or sets the key.
    /// </summary>
    public AbstractPrivateKey Key { get; set; }

    /// <summary>
    /// Gets or sets the leaves.
    /// </summary>
    public List<MerkleNode> Leaves { get; set; }

    /// <summary>
    /// The to tryte string.
    /// </summary>
    /// <returns>
    /// The <see cref="TryteString"/>.
    /// </returns>
    public TryteString ToTryteString()
    {
      return this.Leaves.Select(l => l.Hash).Merge();
    }
  }
}