using System.Threading.Tasks;
using Tangle.Net.Api.HighLevel.Request;
using Tangle.Net.Api.HighLevel.Response;

namespace Tangle.Net.Api.HighLevel
{
  public interface IHighLevelClient
  {
    Task<GetBalanceResponse> GetBalanceAsync(GetBalanceRequest request);

    Task<GetUnspentAddressesResponse> GetUnspentAddressesAsync(GetUnspentAddressesRequest request);
  }
}