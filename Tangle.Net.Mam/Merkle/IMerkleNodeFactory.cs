namespace Tangle.Net.Mam.Merkle
{
  /// <summary>
  /// The MerkleNodeFactory interface.
  /// </summary>
  public interface IMerkleNodeFactory
  {
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
    MerkleNode Create(MerkleNode leftNode, MerkleNode rightNode = null);

    #endregion
  }
}