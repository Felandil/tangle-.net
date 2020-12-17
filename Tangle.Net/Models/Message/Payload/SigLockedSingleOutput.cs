namespace Tangle.Net.Models.Message.Payload
{
  using Newtonsoft.Json;

  public class SigLockedSingleOutput
  {
    [JsonProperty("address")]
    public Ed25519Address Address { get; set; }

    [JsonProperty("amount")]
    public long Amount { get; set; }
  }
}