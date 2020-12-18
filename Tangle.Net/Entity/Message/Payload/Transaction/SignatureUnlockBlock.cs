namespace Tangle.Net.Entity.Message.Payload.Transaction
{
  using Newtonsoft.Json;

  public class SignatureUnlockBlock : PayloadType
  {
    [JsonProperty("signature")]
    public Ed25519Signature Signature { get; set; }
  }
}