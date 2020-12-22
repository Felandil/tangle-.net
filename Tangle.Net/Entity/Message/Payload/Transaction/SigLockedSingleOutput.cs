namespace Tangle.Net.Entity.Message.Payload.Transaction
{
  using System;
  using System.Collections.Generic;

  using Newtonsoft.Json;

  public class SigLockedSingleOutput : PayloadType, ISerializable
  {
    public SigLockedSingleOutput()
    {
      this.Type = 0;
    }

    [JsonProperty("address")]
    public Ed25519Address Address { get; set; }

    [JsonProperty("amount")]
    public long Amount { get; set; }

    public byte[] Serialize()
    {
      var serialized = new List<byte>();

      serialized.Add((byte)this.Type);
      serialized.AddRange(this.Address.Serialize());
      serialized.AddRange(BitConverter.GetBytes(this.Amount));

      return serialized.ToArray();
    }
  }
}