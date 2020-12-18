namespace Tangle.Net.Entity.Message.Payload.Transaction
{
  using Newtonsoft.Json;

  public class SigLockedSingleOutput : PayloadType
  {
    [JsonProperty("address")]
    public Ed25519Address Address { get; set; }

    [JsonProperty("amount")]
    public long Amount { get; set; }
  }
}