namespace Tangle.Net.Api.Responses
{
  using Newtonsoft.Json;

  public class MessageIdResponse
  {
    [JsonProperty("messageId")]
    public string MessageId { get; set; }
  }
}