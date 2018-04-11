namespace Tangle.Net.Mam.Merkle
{
  using System.Collections.Generic;

  using Tangle.Net.Cryptography;

  /// <summary>
  /// The merkle tree.
  /// </summary>
  public class MerkleTree
  {
    #region Public Properties

    /// <summary>
    /// Gets or sets the root.
    /// </summary>
    public MerkleNode Root { get; set; }

    /// <summary>
    /// Gets the size.
    /// </summary>
    public int Size => this.Root.Size;

    #endregion

    #region Public Methods and Operators

    /// <summary>
    /// The get subtree by index.
    /// </summary>
    /// <param name="index">
    /// The index.
    /// </param>
    /// <returns>
    /// The <see cref="MerkleSubTree"/>.
    /// </returns>
    public MerkleSubTree GetSubtreeByIndex(int index)
    {
      if (this.Size == 1)
      {
        return new MerkleSubTree { Key = this.Root.LeftNode.Key, Leaves = new List<MerkleNode>() };
      }

      var leaves = new List<MerkleNode>();
      var node = this.Root;
      AbstractPrivateKey key = null;
      var size = this.Size;

      if (index < size)
      {
        while (node != null)
        {
          if (node.LeftNode == null)
          {
            key = node.Key;
            break;
          }

          size = node.LeftNode.Size;
          if (index < size)
          {
            leaves.Add(node.RightNode ?? node.LeftNode);
            node = node.LeftNode;
          }
          else
          {
            leaves.Add(node.LeftNode);
            node = node.RightNode;
            index -= size;
          }
        }
      }

      leaves.Reverse();

      return new MerkleSubTree
               {
                 Key = key,
                 Leaves = leaves
               };
    }

    #endregion
  }
}