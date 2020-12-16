namespace Tangle.Net.Api.Responses
{
  using Newtonsoft.Json;

  public class TipsResponse
  {
    [JsonProperty("tip1MessageId")]
    public string TipOneMessageId { get; set; }

    [JsonProperty("tip2MessageId")]
    public string TipTwoMessageId { get; set; }
  }
}