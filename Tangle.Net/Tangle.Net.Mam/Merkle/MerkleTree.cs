namespace Tangle.Net.Mam.Merkle
{
  using System;
  using System.Collections.Generic;

  using Tangle.Net.Cryptography;
  using Tangle.Net.Entity;

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

    /// <summary>
    /// The compute root.
    /// </summary>
    /// <param name="address">
    /// The address.
    /// </param>
    /// <param name="nodeHashes">
    /// The node hashes.
    /// </param>
    /// <param name="index">
    /// The index.
    /// </param>
    /// <param name="curl">
    /// The curl.
    /// </param>
    /// <returns>
    /// The <see cref="Hash"/>.
    /// </returns>
    public static Hash ComputeRoot(Address address, IEnumerable<Hash> nodeHashes, int index, AbstractCurl curl)
    {
      var rootHash = address.ToTrits();
      var i = 1;

      foreach (var nodeHash in nodeHashes)
      {
        curl.Reset();
        if ((i & index) == 0)
        {
          curl.Absorb(rootHash);
          curl.Absorb(nodeHash.ToTrits());        
        }
        else
        {
          curl.Absorb(nodeHash.ToTrits());
          curl.Absorb(rootHash);
        }

        i <<= 1;
        curl.Squeeze(rootHash);
      }

      return new Hash(Converter.TritsToTrytes(rootHash));
    }

    #region Public Methods and Operators

    /// <summary>
    /// The get leaves by key index.
    /// </summary>
    /// <param name="index">
    /// The index.
    /// </param>
    /// <returns>
    /// The <see cref="Tuple"/>.
    /// </returns>
    public MerkleSubTree GetSubtreeByIndex(int index)
    {
      var leaves = new List<MerkleNode>();
      var node = this.Root;
      IPrivateKey key = null;
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