namespace Tangle.Net.Api.Responses
{
  using Newtonsoft.Json;

  public class MessageIdResponse
  {
    [JsonProperty(PropertyName = "messageId")]
    public string MessageId { get; set; }
  }
}