namespace Tangle.Net.Models.Message.MessagePayload
{
  using System.Collections.Generic;

  using Newtonsoft.Json;

  public class MilestonePayload : PayloadBase
  {
    [JsonProperty("inclusionMerkleProof")]
    public string InclusionMerkleProof { get; set; }

    [JsonProperty("index")]
    public int Index { get; set; }

    [JsonProperty("parent1MessageId")]
    public string Parent1MessageId { get; set; }

    [JsonProperty("parent2MessageId")]
    public string Parent2MessageId { get; set; }

    [JsonProperty("publicKeys")]
    public List<string> PublicKeys { get; set; }

    [JsonProperty("signatures")]
    public List<string> Signatures { get; set; }

    [JsonProperty("timestamp")]
    public long Timestamp { get; set; }
  }
}