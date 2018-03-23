namespace Tangle.Net.Mam.Merkle
{
  using System.Collections.Generic;

  using Tangle.Net.Cryptography;
  using Tangle.Net.Entity;

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
      var value = string.Empty;
      foreach (var merkleNode in this.Leaves)
      {
        value += merkleNode.Hash.Value;
      }

      return new TryteString(value);
    }
  }
}