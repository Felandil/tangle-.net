namespace Tangle.Net.Models.Message.Payload.Transaction
{
  using Newtonsoft.Json;

  public class SigLockedSingleOutput : PayloadBase
  {
    [JsonProperty("address")]
    public Ed25519Address Address { get; set; }

    [JsonProperty("amount")]
    public long Amount { get; set; }
  }
}