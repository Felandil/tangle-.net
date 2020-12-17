namespace Tangle.Net.Models.Message.Payload
{
  using Newtonsoft.Json;
  using Newtonsoft.Json.Serialization;

  public class TransactionPayload : PayloadBase
  {
    [JsonProperty("essence")]
    public TransactionEssence Essence { get; set; }
  }
}