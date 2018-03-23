namespace Tangle.Net.Mam.Merkle
{
  using System.Collections.Generic;

  using Tangle.Net.Entity;

  /// <summary>
  /// The MerkleLeafFactory interface.
  /// </summary>
  public interface IMerkleLeafFactory
  {
    #region Public Methods and Operators

    /// <summary>
    /// The create.
    /// </summary>
    /// <param name="seed">
    /// The seed.
    /// </param>
    /// <param name="securityLevel">
    /// The security Level.
    /// </param>
    /// <param name="startIndex">
    /// The start Index.
    /// </param>
    /// <param name="count">
    /// The count.
    /// </param>
    /// <returns>
    /// The <see cref="MerkleNode"/>.
    /// </returns>
    List<MerkleNode> Create(Seed seed, int securityLevel, int startIndex, int count);

    #endregion
  }
}