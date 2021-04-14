namespace Tangle.Net.Entity.Message
{
  using Newtonsoft.Json;
  using System.Collections.Generic;

  public class MessageMetadata
  {
    [JsonProperty("isSolid")]
    public bool IsSolid { get; set; }

    [JsonProperty("messageId")]
    public string MessageId { get; set; }

    [JsonProperty("parentMessageIds")]
    public List<string> ParentMessageIds { get; set; }

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