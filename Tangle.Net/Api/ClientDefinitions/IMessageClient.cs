using System.Threading.Tasks;
using Tangle.Net.Api.Response.Message;
using Tangle.Net.Entity.Message;
using Tangle.Net.Entity.Message.Payload;

namespace Tangle.Net.Api.ClientDefinitions
{
  public interface IMessageClient
  {
    Task<MessageIdResponse> SendDataAsync(string payload, string index);

    Task<MessageIdResponse> SendMessageAsync<T>(Message<T> message)
      where T : Payload;

    Task<Message<T>> GetMessageAsync<T>(string messageId) where T : Payload;

    Task<MessageMetadata> GetMessageMetadataAsync(string messageId);

    Task<MessageRawResponse> GetMessageRawAsync(string messageId);

    Task<MessageChildrenResponse> GetMessageChildrenAsync(string messageId);

    Task<MessageIdsByIndexResponse> GetMessageIdsByIndexAsync(string index);
  }
}