namespace Tangle.Net.Models.Message.Payload.Transaction
{
  using System.Collections.Generic;

  using Newtonsoft.Json;

  public class TransactionEssence
  {
    [JsonProperty("inputs")]
    public List<UTXOInput> Inputs { get; set; }

    [JsonProperty("outputs")]
    public List<SigLockedSingleOutput> Outputs { get; set; }

    [JsonProperty("payload")]
    public IndexationPayload Payload { get; set; }
  }
}