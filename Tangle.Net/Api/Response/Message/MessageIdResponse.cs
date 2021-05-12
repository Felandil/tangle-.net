using Newtonsoft.Json;

namespace Tangle.Net.Api.Response.Message
{
  public class MessageIdResponse
  {
    [JsonProperty("messageId")]
    public string MessageId { get; set; }
  }
}