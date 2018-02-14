namespace Tangle.Net.Mam.Merkle
{
  using System.Collections.Generic;

  using Tangle.Net.Cryptography;
  using Tangle.Net.Entity;

  public class MerkleSubTree
  {
    public MerkleSubTree()
    {
      this.Leaves = new List<MerkleNode>();
    }

    public List<MerkleNode> Leaves { get; set; }
    public IPrivateKey Key { get; set; }

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