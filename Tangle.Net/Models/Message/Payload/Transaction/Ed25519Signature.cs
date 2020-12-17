namespace Tangle.Net.Models.Message.Payload.Transaction
{
  using Newtonsoft.Json;

  public class Ed25519Signature : PayloadBase
  {
    [JsonProperty("publicKey")]
    public string PublicKey { get; set; }

    [JsonProperty("signature")]
    public string Signature { get; set; }
  }
}