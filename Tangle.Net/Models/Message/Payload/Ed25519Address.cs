namespace Tangle.Net.Models.Message.Payload
{
  using Newtonsoft.Json;

  public class Ed25519Address
  {
    [JsonProperty("address")]
    public string Address { get; set; }
  }
}