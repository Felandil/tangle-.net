using System.Threading.Tasks;
using Tangle.Net.Api.Responses;

namespace Tangle.Net.Api.ClientDefinitions
{
  public interface ITipsClient
  {
    Task<TipsResponse> GetTipsAsync();
  }
}