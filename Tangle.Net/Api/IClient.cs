namespace Tangle.Net.Api
{
  using System.Threading.Tasks;

  using Tangle.Net.Api.Responses;

  public interface IClient
  {
    Task<MessageIdResponse> SendMessageAsync(string payload, string index);

    Task<TipsResponse> GetTipsAsync();
  }
}