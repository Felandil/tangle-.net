namespace Tangle.Net.Api
{
  using System.Threading.Tasks;

  using Tangle.Net.Api.Responses;
  using Tangle.Net.Models;
  using Tangle.Net.Models.MessagePayload;

  public interface IClient
  {
    Task<MessageIdResponse> SendMessageAsync(string payload, string index);

    Task<MessageIdResponse> SendMessageAsync<T>(Message<T> message)
      where T : PayloadBase;

    Task<Message<T>> GetMessageAsync<T>(string messageId) where T : PayloadBase;

    Task<TipsResponse> GetTipsAsync();
  }
}