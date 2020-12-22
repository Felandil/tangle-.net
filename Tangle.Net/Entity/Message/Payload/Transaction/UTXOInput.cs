namespace Tangle.Net.Entity.Message.Payload.Transaction
{
  using System;
  using System.Collections.Generic;
  using System.Text;

  using Newtonsoft.Json;

  using Tangle.Net.Utils;

  // ReSharper disable once InconsistentNaming
  public class UTXOInput : PayloadType, ISerializable
  {
    public UTXOInput()
    {
      this.Type = 0;
    }

    [JsonProperty("transactionId")]
    public string TransactionId { get; set; }

    [JsonProperty("transactionOutputIndex")]
    public short TransactionOutputIndex { get; set; }

    public byte[] Serialize()
    {
      var serialized = new List<byte>();

      serialized.Add((byte)this.Type);
      serialized.AddRange(this.TransactionId.HexToBytes());
      serialized.AddRange(BitConverter.GetBytes(this.TransactionOutputIndex));

      return serialized.ToArray();
    }
  }
}