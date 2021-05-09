using System.Threading.Tasks;
using Tangle.Net.Entity;

namespace Tangle.Net.Api.ClientDefinitions
{
  public interface INodeClient
  {
    Task<bool> IsNodeHealthy();

    Task<NodeInfo> GetNodeInfoAsync();
  }
}