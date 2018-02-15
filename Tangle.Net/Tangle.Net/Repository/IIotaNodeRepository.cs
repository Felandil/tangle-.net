namespace Tangle.Net.Repository
{
  using System.Collections.Generic;

  using Tangle.Net.Repository.DataTransfer;
  using Tangle.Net.Repository.Responses;

  /// <summary>
  /// The IotaNodeRepository interface.
  /// </summary>
  public interface IIotaNodeRepository
  {
    #region Public Methods and Operators

    /// <summary>
    /// The add neighbor.
    /// </summary>
    /// <param name="neighbors">
    /// The neighbors.
    /// </param>
    /// <returns>
    /// The <see cref="AddNeighborsResponse"/>.
    /// </returns>
    AddNeighborsResponse AddNeighbor(IEnumerable<Neighbor> neighbors);

    /// <summary>
    /// The get neighbors.
    /// </summary>
    /// <returns>
    /// The <see cref="NeighborList"/>.
    /// </returns>
    NeighborList GetNeighbors();

    /// <summary>
    /// The get node info.
    /// </summary>
    /// <returns>
    /// The <see cref="NodeInfo"/>.
    /// </returns>
    NodeInfo GetNodeInfo();

    /// <summary>
    /// The remove neighbors.
    /// </summary>
    /// <param name="neighbors">
    /// The neighbors.
    /// </param>
    /// <returns>
    /// The <see cref="RemoveNeighborsResponse"/>.
    /// </returns>
    RemoveNeighborsResponse RemoveNeighbors(IEnumerable<Neighbor> neighbors);

    #endregion
  }
}