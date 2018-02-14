namespace Tangle.Net.Mam.Merkle
{
  using System.Collections.Generic;

  using Tangle.Net.Cryptography;

  public class MerkleSubTree
  {
    public MerkleSubTree()
    {
      this.Leaves = new List<MerkleNode>();
    }

    public List<MerkleNode> Leaves { get; set; }
    public IPrivateKey Key { get; set; }

    public override string ToString()
    {
      var value = string.Empty;
      foreach (var merkleNode in this.Leaves)
      {
        value += merkleNode.Hash.Value;
      }

      return value;
    }
  }
}