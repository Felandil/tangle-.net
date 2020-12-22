namespace Tangle.Net.Entity.Message.Payload.Transaction
{
  using System.Collections.Generic;

  using Newtonsoft.Json;

  using Tangle.Net.Utils;

  public class Ed25519Signature : PayloadType, ISerializable
  {
    public Ed25519Signature()
    {
      this.Type = 1;
    }

    [JsonProperty("publicKey")]
    public string PublicKey { get; set; }

    [JsonProperty("signature")]
    public string Signature { get; set; }

    /// <inheritdoc />
    public byte[] Serialize()
    {
      var serialized = new List<byte>();

      serialized.Add((byte)this.Type);
      serialized.AddRange(this.PublicKey.HexToBytes());
      serialized.AddRange(this.Signature.HexToBytes());

      return serialized.ToArray();
    }
  }
}