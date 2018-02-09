namespace Tangle.Net.Mam.Merkle
{
  /// <summary>
  /// The merkle tree.
  /// </summary>
  public class MerkleTree
  {
    #region Public Properties

    /// <summary>
    /// Gets or sets the root.
    /// </summary>
    public MerkleNode Root { get; set; }

    /// <summary>
    /// Gets the size.
    /// </summary>
    public int Size
    {
      get
      {
        return this.Root.Size;
      }
    }

    #endregion
  }
}