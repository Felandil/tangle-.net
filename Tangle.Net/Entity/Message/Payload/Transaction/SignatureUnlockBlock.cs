namespace Tangle.Net.Entity.Message.Payload.Transaction
{
  using Newtonsoft.Json;

  public class SignatureUnlockBlock : PayloadBase
  {
    [JsonProperty("signature")]
    public Ed25519Signature Signature { get; set; }

    /// <inheritdoc />
    protected override byte[] SerializeImplementation()
    {
      return new byte[] { };
    }
  }
}