namespace Tangle.Net.Repository.DataTransfer
{
  using System.Collections.Generic;

  using Tangle.Net.Entity;

  /// <summary>
  /// The tip hash list.
  /// </summary>
  public class TipHashList
  {
    #region Public Properties

    /// <summary>
    /// Gets or sets the duration.
    /// </summary>
    public int Duration { get; set; }

    /// <summary>
    /// Gets or sets the hashes.
    /// </summary>
    public List<Hash> Hashes { get; set; }

    #endregion
  }
}