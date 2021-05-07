namespace Tangle.Net.Entity.Message.Payload
{
  using System;
  using System.Collections.Generic;
  using System.Linq;

  using Newtonsoft.Json;

  using Tangle.Net.Entity.Message.Payload.Transaction;

  public class TransactionPayload<TBlock> : Payload
    where TBlock : UnlockBlock
  {
    public TransactionPayload()
    {
      this.Type = TransactionPayloadType;
    }

    [JsonProperty("essence")]
    public TransactionEssence Essence { get; set; }

    [JsonProperty("unlockBlocks")]
    public List<TBlock> UnlockBlocks { get; set; }

    public static TransactionPayload<TBlock> Deserialize(byte[] payload)
    {
      var payloadType = BitConverter.ToInt32(payload.Take(4).ToArray(), 0);
      var pointer = 4;
      if (payloadType != TransactionPayloadType)
      {
        throw new Exception($"Payload Type ({payloadType}) is not a transaction payload!");
      }
      return new TransactionPayload<TBlock>();
    }

    /// <inheritdoc />
    protected override byte[] SerializeImplementation()
    {
      var serialized = new List<byte>();
      serialized.AddRange(BitConverter.GetBytes(this.Type));
      serialized.AddRange(this.Essence.Serialize());
      serialized.AddRange(this.SerializeUnlockBlocks());

      return serialized.ToArray();
    }

    public byte[] SerializeUnlockBlocks()
    {
      var serialized = new List<byte>();
      serialized.AddRange(BitConverter.GetBytes((short)this.UnlockBlocks.Count));
      foreach (var unlockBlock in this.UnlockBlocks)
      {
        serialized.AddRange(unlockBlock.Serialize());
      }

      return serialized.ToArray();
    }
  }
}