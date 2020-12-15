namespace Tangle.Net.Api.Responses
{
  using Newtonsoft.Json;

  public class TipsResponse
  {
    [JsonProperty(PropertyName = "tip1MessageId")]
    public string TipOneMessageId { get; set; }

    [JsonProperty(PropertyName = "tip2MessageId")]
    public string TipTwoMessageId { get; set; }
  }
}