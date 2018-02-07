namespace Tangle.Net.Mam.Merkle
{
  using Tangle.Net.Cryptography;
  using Tangle.Net.Entity;

  /// <summary>
  /// The merkle node.
  /// </summary>
  public class MerkleNode
  {
    #region Constructors and Destructors

    /// <summary>
    /// Initializes a new instance of the <see cref="MerkleNode"/> class.
    /// </summary>
    public MerkleNode()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="MerkleNode"/> class.
    /// </summary>
    /// <param name="leftNode">
    /// The left node.
    /// </param>
    /// <param name="rightNode">
    /// The right node.
    /// </param>
    public MerkleNode(MerkleNode leftNode, MerkleNode rightNode = null)
    {
      leftNode.ParentNode = this;
      if (rightNode != null)
      {
        rightNode.ParentNode = this;
      }

      this.LeftNode = leftNode;
      this.RightNode = rightNode;
    }

    #endregion

    #region Public Properties

    /// <summary>
    /// Gets or sets the hash.
    /// </summary>
    public Hash Hash { get; set; }

    /// <summary>
    /// Gets or sets the key.
    /// </summary>
    public IPrivateKey Key { get; set; }

    /// <summary>
    /// Gets or sets the left node.
    /// </summary>
    public MerkleNode LeftNode { get; set; }

    /// <summary>
    /// Gets or sets the parent node.
    /// </summary>
    public MerkleNode ParentNode { get; set; }

    /// <summary>
    /// Gets or sets the right node.
    /// </summary>
    public MerkleNode RightNode { get; set; }

    /// <summary>
    /// Gets or sets the size.
    /// </summary>
    public int Size { get; set; }

    #endregion
  }
}