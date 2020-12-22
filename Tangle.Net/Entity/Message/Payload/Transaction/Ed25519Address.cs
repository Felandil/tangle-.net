namespace Tangle.Net.Entity.Message.Payload.Transaction
{
  using System.Collections.Generic;

  using Newtonsoft.Json;

  using Tangle.Net.Utils;

  public class Ed25519Address : PayloadType, ISerializable
  {
    public Ed25519Address()
    {
      this.Type = 1;
    }

    [JsonProperty("address")]
    public string Address { get; set; }

    public byte[] Serialize()
    {
      var serialized = new List<byte>();

      serialized.Add((byte)this.Type);
      serialized.AddRange(this.Address.HexToBytes());

      return serialized.ToArray();
    }
  }
}