namespace Tangle.Net.Mam.Merkle
{
  using System.Collections.Generic;

  /// <summary>
  /// The MerkleLeafFactory interface.
  /// </summary>
  public interface IMerkleLeafFactory
  {
    #region Public Methods and Operators

    /// <summary>
    /// The create.
    /// </summary>
    /// <param name="startIndex">
    /// The start Index.
    /// </param>
    /// <param name="count">
    /// The count.
    /// </param>
    /// <returns>
    /// The <see cref="MerkleNode"/>.
    /// </returns>
    List<MerkleNode> Create(int startIndex, int count);

    #endregion
  }
}