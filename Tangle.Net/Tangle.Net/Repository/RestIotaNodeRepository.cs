namespace Tangle.Net.Repository
{
  using System.Collections.Generic;
  using System.Linq;
  using System.Threading.Tasks;

  using Tangle.Net.Repository.DataTransfer;
  using Tangle.Net.Repository.Responses;

  /// <inheritdoc />
  public partial class RestIotaRepository : IIotaRepository
  {
    /// <inheritdoc />
    public virtual AddNeighborsResponse AddNeighbors(IEnumerable<string> neighborUris)
    {
      return this.Client.ExecuteParameterizedCommand<AddNeighborsResponse>(
        new Dictionary<string, object> { { "command", CommandType.AddNeighbors }, { "uris", neighborUris.ToList() } });
    }


    /// <inheritdoc />
    public virtual async Task<AddNeighborsResponse> AddNeighborsAsync(IEnumerable<string> neighborUris)
    {
      return await this.Client.ExecuteParameterizedCommandAsync<AddNeighborsResponse>(
               new Dictionary<string, object> { { "command", CommandType.AddNeighbors }, { "uris", neighborUris.ToList() } });
    }

    /// <inheritdoc />
    public virtual NeighborList GetNeighbors()
    {
      return this.Client.ExecuteParameterlessCommand<NeighborList>(CommandType.GetNeighbors);
    }

    /// <inheritdoc />
    public virtual async Task<NeighborList> GetNeighborsAsync()
    {
      return await this.Client.ExecuteParameterlessCommandAsync<NeighborList>(CommandType.GetNeighbors);
    }

    /// <inheritdoc />
    public virtual NodeInfo GetNodeInfo()
    {
      return this.Client.ExecuteParameterlessCommand<NodeInfo>(CommandType.GetNodeInfo);
    }

    /// <inheritdoc />
    public virtual async Task<NodeInfo> GetNodeInfoAsync()
    {
      return await this.Client.ExecuteParameterlessCommandAsync<NodeInfo>(CommandType.GetNodeInfo);
    }

    /// <inheritdoc />
    public virtual RemoveNeighborsResponse RemoveNeighbors(IEnumerable<string> neighborUris)
    {
      return this.Client.ExecuteParameterizedCommand<RemoveNeighborsResponse>(
        new Dictionary<string, object>
          {
            { "command", CommandType.RemoveNeighbors },
            { "uris", neighborUris.ToList() }
          });
    }

    /// <inheritdoc />
    public virtual async Task<RemoveNeighborsResponse> RemoveNeighborsAsync(IEnumerable<string> neighborUris)
    {
      return await this.Client.ExecuteParameterizedCommandAsync<RemoveNeighborsResponse>(
               new Dictionary<string, object>
                 {
                   { "command", CommandType.RemoveNeighbors },
                   { "uris", neighborUris.ToList() }
                 });
    }
  }
}