namespace Tangle.Net.Api.Responses.Message
{
  using Newtonsoft.Json;

  public class MessageIdResponse
  {
    [JsonProperty("messageId")]
    public string MessageId { get; set; }
  }
}