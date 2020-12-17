namespace Tangle.Net.Models.Message.Payload
{
  using Newtonsoft.Json;

  public class TransactionPayload : PayloadBase
  {
    [JsonProperty("essence")]
    public TransactionEssence Essence { get; set; }
  }
}