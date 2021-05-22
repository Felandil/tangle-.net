using Tangle.Net.Entity.Ed25519;

namespace Tangle.Net.Entity.Message.Payload.Transaction
{
  using System;
  using System.Collections.Generic;

  using Newtonsoft.Json;

  public class SigLockedOutput : PayloadType, ISerializable
  {
    public const int SigLockedSingleOutputType = 0;
    public const int SigLockedDustAllowanceOutput = 1;

    public SigLockedOutput()
    {
      this.Type = 0;
    }

    public SigLockedOutput(int type)
    {
      if (type != SigLockedSingleOutputType && type != SigLockedDustAllowanceOutput)
      {
        throw new ArgumentException("Invalid Type. Type must be 0 (SigLockedSingleOutputType) or 1 (SigLockedDustAllowanceOutput)", nameof(type));
      }

      this.Type = type;
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