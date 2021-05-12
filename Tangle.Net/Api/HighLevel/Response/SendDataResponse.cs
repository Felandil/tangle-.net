using Tangle.Net.Entity.Message;
using Tangle.Net.Entity.Message.Payload;

namespace Tangle.Net.Api.HighLevel.Response
{
  public class SendDataResponse
  {
    public SendDataResponse(Message<IndexationPayload> message, string messageId)
    {
      Message = message;
      MessageId = messageId;
    }

    public Message<IndexationPayload> Message { get; }
    public string MessageId { get; }
  }
}