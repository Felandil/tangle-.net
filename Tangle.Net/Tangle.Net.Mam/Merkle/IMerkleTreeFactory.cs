namespace Tangle.Net.Mam.Merkle
{
  using System.Collections.Generic;

  using Tangle.Net.Entity;

  /// <summary>
  /// The MerkleTreeFactory interface.
  /// </summary>
  public interface IMerkleTreeFactory
  {
    #region Public Methods and Operators

    /// <summary>
    /// The create.
    /// </summary>
    /// <param name="seed">
    /// The seed.
    /// </param>
    /// <param name="startIndex">
    /// The startIndex.
    /// </param>
    /// <param name="count">
    /// The count.
    /// </param>
    /// <param name="securityLevel">
    /// The security level.
    /// </param>
    /// <returns>
    /// The <see cref="MerkleTree"/>.
    /// </returns>
    MerkleTree Create(Seed seed, int startIndex, int count, int securityLevel);

    /// <summary>
    /// The from branch.
    /// </summary>
    /// <param name="siblings">
    /// The siblings.
    /// </param>
    /// <param name="address">
    /// The address.
    /// </param>
    /// <param name="index">
    /// The index.
    /// </param>
    /// <returns>
    /// The <see cref="MerkleTree"/>.
    /// </returns>
    Hash RecalculateRoot(int[] siblings, int[] address, int index);

    #endregion
  }
}