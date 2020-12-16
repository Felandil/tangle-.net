namespace Tangle.Net.Api.Responses
{
  using Newtonsoft.Json;

  public class MessageMetadataResponse
  {
    [JsonProperty("isSolid")]
    public bool IsSolid { get; set; }

    [JsonProperty("messageId")]
    public string MessageId { get; set; }

    [JsonProperty("parent1MessageId")]
    public string Parent1MessageId { get; set; }

    [JsonProperty("parent2MessageId")]
    public string Parent2MessageId { get; set; }

    [JsonProperty("shouldPromote")]
    public bool ShouldPromote { get; set; }

    [JsonProperty("shouldReattach")]
    public bool ShouldReattach { get; set; }

    [JsonProperty("referencedByMilestoneIndex")]
    public int ReferencedByMilestoneIndex { get; set; }

    [JsonProperty("ledgerInclusionState")]
    public string LedgerInclusionState { get; set; }
  }
}