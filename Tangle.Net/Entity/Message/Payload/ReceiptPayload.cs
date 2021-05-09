using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Tangle.Net.Entity.Ed25519;
using Tangle.Net.Entity.Message.Payload.Receipt;
using Tangle.Net.Entity.Message.Payload.Transaction;
using Tangle.Net.Utils;

namespace Tangle.Net.Entity.Message.Payload
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

      serialized.AddRange(this.Transaction.Serialize());

      return serialized.ToArray();
    }

    public static ReceiptPayload Deserialize(byte[] payload)
    {
      var payloadType = BitConverter.ToInt32(payload.Take(4).ToArray(), 0);
      var pointer = 4;
      if (payloadType != ReceiptPayloadType)
      {
        throw new Exception($"Payload Type ({payloadType}) is not a receipt payload!");
      }

      var migratedAt = BitConverter.ToInt32(payload.Skip(pointer).Take(4).ToArray(), 0);
      pointer += 4;

      var final = payload[pointer] == 1;
      pointer += 1;

      var fundCount = BitConverter.ToInt16(payload.Skip(pointer).Take(2).ToArray(), 0);
      pointer += 2;

      var funds = new List<MigratedFunds>();
      for (var i = 0; i < fundCount; i++)
      {
        var tailTransactionHash = payload.Skip(pointer).Take(32).ToHex();
        pointer += 32;

        var address = Ed25519Address.Deserialize(payload.Skip(pointer).Take(33).ToArray());
        pointer += 33;

        var deposit = BitConverter.ToInt64(payload.Skip(pointer).Take(8).ToArray(), 0);
        pointer += 8;

        funds.Add(new MigratedFunds { Address = address, Deposit = deposit, TailTransactionHash =  tailTransactionHash});
      }

      var transactionLength = BitConverter.ToInt32(payload.Skip(pointer).Take(4).ToArray(), 0);
      pointer += 4;

      var transaction =
        Payload.Deserialize<TreasuryTransactionPayload>(payload.Skip(pointer).Take(transactionLength).ToArray());

      return new ReceiptPayload
      {
        Final = final,
        Funds = funds,
        MigratedAt = migratedAt,
        Transaction = transaction,
        Type = payloadType
      };
    }
  }
}