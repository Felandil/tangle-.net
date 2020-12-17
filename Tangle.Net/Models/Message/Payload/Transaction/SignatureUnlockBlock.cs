namespace Tangle.Net.Models.Message.Payload.Transaction
{
  using Newtonsoft.Json;

  public class SignatureUnlockBlock : PayloadBase
  {
    [JsonProperty("signature")]
    public Ed25519Signature Signature { get; set; }
  }
}