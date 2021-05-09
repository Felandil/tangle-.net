using Tangle.Net.Api.ClientDefinitions;

namespace Tangle.Net.Api
{
  public interface IClient : INodeClient, ITipsClient, IMessageClient, IUtxoClient, IMilestoneClient, IPeerClient
  {
  }
}