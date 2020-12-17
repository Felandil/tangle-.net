namespace Tangle.Net.Models.Message.Payload.Transaction
{
  using Newtonsoft.Json;

  public class Ed25519Address : PayloadBase
  {
    [JsonProperty("address")]
    public string Address { get; set; }
  }
}