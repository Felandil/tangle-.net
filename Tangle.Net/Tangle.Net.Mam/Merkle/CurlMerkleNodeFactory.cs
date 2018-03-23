namespace Tangle.Net.Mam.Merkle
{
  using Tangle.Net.Cryptography;
  using Tangle.Net.Cryptography.Curl;
  using Tangle.Net.Entity;

  /// <summary>
  /// The curl merkle node factory.
  /// </summary>
  public class CurlMerkleNodeFactory : IMerkleNodeFactory
  {
    #region Constructors and Destructors

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

    #endregion

    #region Properties

    /// <summary>
    /// Gets or sets the curl.
    /// </summary>
    private AbstractCurl Curl { get; set; }

    #endregion

    #region Public Methods and Operators

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
      var hashTrits = new int[AbstractCurl.HashLength];
      var leftNodeTrits = leftNode.Hash.ToTrits();

      this.Curl.Reset();
      this.Curl.Absorb(leftNodeTrits);
      this.Curl.Absorb(rightNode == null ? leftNodeTrits : rightNode.Hash.ToTrits());
      this.Curl.Squeeze(hashTrits);

      var hash = new Hash(Converter.TritsToTrytes(hashTrits));

      return new MerkleNode(leftNode, rightNode) { Hash = hash };
    }

    #endregion
  }
}