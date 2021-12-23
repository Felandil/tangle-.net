using System.Collections.Generic;
using System.Linq;
using Isopoh.Cryptography.Blake2b;
using Newtonsoft.Json;
using Tangle.Net.Entity.Message;
using Tangle.Net.Entity.Message.Payload;
using Tangle.Net.Utils;

namespace Tangle.Net.Entity.Ed25519
{
  public class Ed25519Address : PayloadType, ISerializable
  {
    public Ed25519Address()
    {
      this.Type = 0;
    }

    public static Ed25519Address FromPublicKey(byte[] publicKey)
    {
      var addressHash = Blake2B.ComputeHash(publicKey, new Blake2BConfig { OutputSizeInBytes = 32 }, null);
      return new Ed25519Address
      {
        Address = addressHash.ToHex(),
        PublicKey = publicKey
      };
    }

    [JsonProperty("address")]
    public string Address { get; set; }

    [JsonProperty("balance")]
    public long Balance { get; set; }

    [JsonProperty("dustAllowed")]
    public bool DustAllowed { get; set; }

    [JsonIgnore]
    public byte[] PublicKey { get; private set; }

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