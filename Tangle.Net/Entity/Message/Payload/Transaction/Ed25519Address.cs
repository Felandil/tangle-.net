namespace Tangle.Net.Entity.Message.Payload.Transaction
{
  using Newtonsoft.Json;

  public class Ed25519Address : PayloadType
  {
    [JsonProperty("address")]
    public string Address { get; set; }
  }
}