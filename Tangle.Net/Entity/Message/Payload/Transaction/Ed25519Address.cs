using System;
using System.Linq;

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

    public static Ed25519Address Deserialize(byte[] payload)
    {
      var type = payload[0];
      var address = payload.Skip(1).Take(32).ToHex();

      return new Ed25519Address {Address = address, Type = type};
    }
  }
}