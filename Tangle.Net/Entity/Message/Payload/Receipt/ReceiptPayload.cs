using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Tangle.Net.Utils;

namespace Tangle.Net.Entity.Message.Payload.Receipt
{
  public class ReceiptPayload : Payload
  {
    [JsonProperty("migratedAt")] 
    public int MigratedAt { get; set; }

    [JsonProperty("final")] 
    public bool Final { get; set; }

    [JsonProperty("funds")]
    public List<MigratedFunds> Funds { get; set; }

    [JsonProperty("transaction")] 
    public TreasuryTransactionPayload Transaction { get; set; }

    protected override byte[] SerializeImplementation()
    {
      var serialized = new List<byte>();
      serialized.AddRange(BitConverter.GetBytes(this.Type));
      serialized.AddRange(BitConverter.GetBytes(this.MigratedAt));
      serialized.Add((byte)(this.Final ? 1 : 0));
      serialized.AddRange(BitConverter.GetBytes((short) this.Funds.Count));
      foreach (var fund in this.Funds)
      {
        serialized.AddRange(fund.TailTransactionHash.HexToBytes());
        serialized.AddRange(fund.Address.Serialize());
        serialized.AddRange(BitConverter.GetBytes(fund.Deposit));
      }


      return serialized.ToArray();
    }
  }
}