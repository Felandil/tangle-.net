namespace Tangle.Net.Entity.Message.Payload.Transaction
{
  using Newtonsoft.Json;

  public class Ed25519Signature : PayloadType
  {
    [JsonProperty("publicKey")]
    public string PublicKey { get; set; }

    [JsonProperty("signature")]
    public string Signature { get; set; }
  }
}