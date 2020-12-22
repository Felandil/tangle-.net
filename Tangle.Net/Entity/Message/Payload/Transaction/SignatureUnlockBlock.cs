namespace Tangle.Net.Entity.Message.Payload.Transaction
{
  using System.Collections.Generic;

  using Newtonsoft.Json;

  public class SignatureUnlockBlock : UnlockBlock
  {
    public SignatureUnlockBlock()
    {
      this.Type = 0;
    }

    [JsonProperty("signature")]
    public Ed25519Signature Signature { get; set; }

    /// <inheritdoc />
    public override byte[] Serialize()
    {
      var serialized = new List<byte>();

      serialized.Add((byte)this.Type);
      serialized.AddRange(this.Signature.Serialize());

      return serialized.ToArray();
    }
  }
}