using System.Threading.Tasks;
using Tangle.Net.Api.HighLevel.Request;
using Tangle.Net.Api.HighLevel.Response;
using Tangle.Net.Entity.Message.Payload;

namespace Tangle.Net.Api.HighLevel
{
  public interface IHighLevelClient
  {
    Task<GetBalanceResponse> GetBalanceAsync(GetBalanceRequest request);

    Task<GetUnspentAddressesResponse> GetUnspentAddressesAsync(GetUnspentAddressesRequest request);

    Task<MessageResponse<IndexationPayload>> SendDataAsync(SendDataRequest request);

    Task<RetrieveDataResponse> RetrieveDataAsync(MessageRequest request);

    Task<MessageResponse<T>> ReattachAsync<T>(MessageRequest request) where T : Payload;

    Task<MessageResponse<T>> PromoteAsync<T>(MessageRequest request) where T : Payload;

    Task<MessageResponse<T>> RetryAsync<T>(MessageRequest request) where T : Payload;
  }
}