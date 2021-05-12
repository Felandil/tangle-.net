using Isopoh.Cryptography.Blake2b;

namespace Tangle.Net.Entity.Message.Payload.Transaction
{
  using System;
  using System.Collections.Generic;

  using Newtonsoft.Json;

  public class TransactionEssence : PayloadType, ISerializable
  {
    public const int TransactionEssenceType = 0;
    public TransactionEssence()
    {
      this.Type = 0;
    }

    [JsonProperty("inputs")]
    public List<UTXOInput> Inputs { get; set; }

    [JsonProperty("outputs")]
    public List<SigLockedSingleOutput> Outputs { get; set; }

    [JsonProperty("payload")]
    public IndexationPayload Payload { get; set; }

    public byte[] Serialize()
    {
      var serialized = new List<byte> {(byte) this.Type};
      serialized.AddRange(this.SerializeInputs());
      serialized.AddRange(this.SerializeOutputs());
      serialized.AddRange(this.Payload.Serialize());

      return serialized.ToArray();
    }

    public byte[] SerializeOutputs()
    {
      var serializedOutputs = new List<byte>();
      serializedOutputs.AddRange(BitConverter.GetBytes((short)this.Outputs.Count));
      foreach (var output in this.Outputs)
      {
        serializedOutputs.AddRange(output.Serialize());
      }
      
      return serializedOutputs.ToArray();
    }

    public byte[] SerializeInputs()
    {
      var serializedInputs = new List<byte>();
      serializedInputs.AddRange(BitConverter.GetBytes((short)this.Inputs.Count));
      foreach (var input in this.Inputs)
      {
        serializedInputs.AddRange(input.Serialize());
      }

      return serializedInputs.ToArray();
    }

    public byte[] CalculateHash()
    {
      return Blake2B.ComputeHash(this.Serialize(), new Blake2BConfig { OutputSizeInBytes = 32 }, null);
    }
  }
}