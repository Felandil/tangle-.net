using System.Threading.Tasks;
using Tangle.Net.Api.Response;

namespace Tangle.Net.Api.ClientDefinitions
{
  public interface ITipsClient
  {
    Task<TipsResponse> GetTipsAsync();
  }
}