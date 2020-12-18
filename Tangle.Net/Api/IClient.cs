namespace Tangle.Net.Api
{
  using System.Threading.Tasks;

  using Tangle.Net.Api.Responses;
  using Tangle.Net.Api.Responses.Message;
  using Tangle.Net.Entity;
  using Tangle.Net.Entity.Message;
  using Tangle.Net.Entity.Message.Payload;

  public interface IClient
  {
    Task<bool> IsNodeHealthy();

    Task<NodeInfo> GetNodeInfoAsync();

    Task<MessageIdResponse> SendDataAsync(string payload, string index);

    Task<MessageIdResponse> SendMessageAsync<T>(Message<T> message)
      where T : Payload;

    Task<Message<T>> GetMessageAsync<T>(string messageId) where T : Payload;

    Task<MessageMetadata> GetMessageMetadataAsync(string messageId);

    Task<MessageRawResponse> GetMessageRawAsync(string messageId);

    Task<MessageChildrenResponse> GetMessageChildrenAsync(string messageId);

    Task<MessageIdsByIndexResponse> GetMessageIdsByIndexAsync(string index);

    Task<TipsResponse> GetTipsAsync();
  }
}