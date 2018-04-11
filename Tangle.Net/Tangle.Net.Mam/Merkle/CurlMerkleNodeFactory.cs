namespace Tangle.Net.Mam.Merkle
{
  using Tangle.Net.Cryptography;
  using Tangle.Net.Cryptography.Curl;
  using Tangle.Net.Entity;
  using Tangle.Net.Utils;

  /// <summary>
  ///   The curl merkle node factory.
  /// </summary>
  public class CurlMerkleNodeFactory : IMerkleNodeFactory
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="CurlMerkleNodeFactory"/> class.
    /// </summary>
    /// <param name="curl">
    /// The curl.
    /// </param>
    public CurlMerkleNodeFactory(AbstractCurl curl)
    {
      this.Curl = curl;
    }

    /// <summary>
    /// The default.
    /// </summary>
    public static CurlMerkleNodeFactory Default => new CurlMerkleNodeFactory(new Curl(CurlMode.CurlP27));

    /// <summary>
    ///   Gets the curl.
    /// </summary>
    private AbstractCurl Curl { get; }

    /// <summary>
    /// The create.
    /// </summary>
    /// <param name="leftNode">
    /// The left node.
    /// </param>
    /// <param name="rightNode">
    /// The right node.
    /// </param>
    /// <returns>
    /// The <see cref="MerkleNode"/>.
    /// </returns>
    public MerkleNode Create(MerkleNode leftNode, MerkleNode rightNode = null)
    {
      var hashTrits = new int[Constants.TritHashLength];
      var leftNodeTrits = leftNode.Hash.ToTrits();
      Hash hash;

      if (rightNode != null)
      {
        this.Curl.Reset();
        this.Curl.Absorb(leftNodeTrits);
        this.Curl.Absorb(rightNode.Hash.ToTrits());
        this.Curl.Squeeze(hashTrits);
        hash = new Hash(Converter.TritsToTrytes(hashTrits));
      }
      else
      {
        hash = leftNode.Hash;
      }

      return new MerkleNode(leftNode, rightNode) { Hash = hash };
    }
  }
}