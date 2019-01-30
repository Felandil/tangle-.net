namespace Tangle.Net.Mam.Merkle
{
  using System.Collections.Generic;

  using Tangle.Net.Cryptography;
  using Tangle.Net.Cryptography.Curl;
  using Tangle.Net.Entity;
  using Tangle.Net.Utils;

  /// <summary>
  /// The curl merkle tree factory.
  /// </summary>
  public class CurlMerkleTreeFactory : IMerkleTreeFactory
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="CurlMerkleTreeFactory"/> class.
    /// </summary>
    /// <param name="nodeFactory">
    /// The node factory.
    /// </param>
    /// <param name="leafFactory">
    /// The leaf Factory.
    /// </param>
    /// <param name="curl">
    /// The curl.
    /// </param>
    public CurlMerkleTreeFactory(IMerkleNodeFactory nodeFactory, IMerkleLeafFactory leafFactory, AbstractCurl curl)
    {
      this.NodeFactory = nodeFactory;
      this.LeafFactory = leafFactory;
      this.Curl = curl;
    }

    /// <summary>
    /// The default.
    /// </summary>
    public static CurlMerkleTreeFactory Default =>
      new CurlMerkleTreeFactory(CurlMerkleNodeFactory.Default, CurlMerkleLeafFactory.Default, new Curl(CurlMode.CurlP81));

    /// <summary>
    /// Gets or sets the leaf factory.
    /// </summary>
    private IMerkleLeafFactory LeafFactory { get; set; }

    /// <summary>
    /// Gets the curl.
    /// </summary>
    private AbstractCurl Curl { get; }

    /// <summary>
    /// Gets or sets the node factory.
    /// </summary>
    private IMerkleNodeFactory NodeFactory { get; set; }

    /// <inheritdoc />
    public MerkleTree Create(Seed seed, int startIndex, int count, int securityLevel)
    {
      var leaves = this.LeafFactory.Create(seed, securityLevel, startIndex, count);
      return new MerkleTree { Root = this.BuildTree(leaves) };
    }

    /// <inheritdoc />
    public Hash RecalculateRoot(int[] siblings, int[] address, int index)
    {
      var tempLeave = new MerkleNode { Hash = new Hash(Converter.TritsToTrytes(siblings)) };
      return this.NodeFactory.Create(tempLeave).Hash;

      //var i = 1;
      //foreach (var chunk in siblings.GetChunks(Constants.TritHashLength))
      //{
      //  this.Curl.Reset();

      //  if ((i & index) == 0)
      //  {
      //    this.Curl.Absorb(address);
      //    this.Curl.Absorb(chunk);
      //  }
      //  else
      //  {
      //    this.Curl.Absorb(chunk);
      //    this.Curl.Absorb(address);
      //  }

      //  i <<= 1;

      //  address = this.Curl.Rate(Constants.TritHashLength);
      //}


      //return new Hash(Converter.TritsToTrytes(this.Curl.Rate(Constants.TritHashLength)));
    }

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
  }
}