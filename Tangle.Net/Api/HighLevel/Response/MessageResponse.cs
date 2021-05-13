using Tangle.Net.Entity.Message;
using Tangle.Net.Entity.Message.Payload;

namespace Tangle.Net.Api.HighLevel.Response
{
  public class MessageResponse<T> where T : Payload
  {
    public MessageResponse(Message<T> message, string messageId)
    {
      Message = message;
      MessageId = messageId;
    }

    public Message<T> Message { get; }
    public string MessageId { get; }
  }
}