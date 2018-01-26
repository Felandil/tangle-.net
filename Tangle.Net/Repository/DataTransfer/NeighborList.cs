namespace Tangle.Net.Repository.DataTransfer
{
  using System.Collections.Generic;

  /// <summary>
  /// The neighbor list.
  /// </summary>
  public class NeighborList
  {
    #region Public Properties

    /// <summary>
    /// Gets or sets the duration.
    /// </summary>
    public int Duration { get; set; }

    /// <summary>
    /// Gets or sets the neighbors.
    /// </summary>
    public List<Neighbor> Neighbors { get; set; }

    #endregion
  }
}