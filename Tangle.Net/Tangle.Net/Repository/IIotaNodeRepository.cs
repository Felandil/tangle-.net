namespace Tangle.Net.Repository
{
  using System.Collections.Generic;
  using System.Threading.Tasks;

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
    /// The add neighbor async.
    /// </summary>
    /// <param name="neighbors">
    /// The neighbors.
    /// </param>
    /// <returns>
    /// The <see cref="Task"/>.
    /// </returns>
    Task<AddNeighborsResponse> AddNeighborAsync(IEnumerable<Neighbor> neighbors);

    /// <summary>
    /// The get neighbors.
    /// </summary>
    /// <returns>
    /// The <see cref="NeighborList"/>.
    /// </returns>
    NeighborList GetNeighbors();

    /// <summary>
    /// The get neighbors async.
    /// </summary>
    /// <returns>
    /// The <see cref="Task"/>.
    /// </returns>
    Task<NeighborList> GetNeighborsAsync();

    /// <summary>
    /// The get node info.
    /// </summary>
    /// <returns>
    /// The <see cref="NodeInfo"/>.
    /// </returns>
    NodeInfo GetNodeInfo();

    /// <summary>
    /// The get node info async.
    /// </summary>
    /// <returns>
    /// The <see cref="Task"/>.
    /// </returns>
    Task<NodeInfo> GetNodeInfoAsync();

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

    /// <summary>
    /// The remove neighbors async.
    /// </summary>
    /// <param name="neighbors">
    /// The neighbors.
    /// </param>
    /// <returns>
    /// The <see cref="Task"/>.
    /// </returns>
    Task<RemoveNeighborsResponse> RemoveNeighborsAsync(IEnumerable<Neighbor> neighbors);

    #endregion
  }
}