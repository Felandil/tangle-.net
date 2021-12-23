using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Tangle.Net.Utils;

namespace Tangle.Net.Entity.Message.Payload.Receipt
{
  public class TreasuryTransactionPayload : Payload
  {
    [JsonProperty("input")] 
    public TreasuryInput Input { get; set; }

    [JsonProperty("output")] 
    public TreasuryOutput Output { get; set; }

    protected override byte[] SerializeImplementation()
    {
      var serialized = new List<byte>();
      serialized.AddRange(BitConverter.GetBytes(this.Type));
      serialized.AddRange(BitConverter.GetBytes(this.Input.Type));
      serialized.AddRange(this.Input.MilestoneId.HexToBytes());
      serialized.AddRange(BitConverter.GetBytes(this.Output.Type));
      serialized.AddRange(BitConverter.GetBytes(this.Output.Amount));

      return serialized.ToArray();
    }

    public static TreasuryTransactionPayload Deserialize(byte[] payload)
    {
      var payloadType = BitConverter.ToInt32(payload.Take(4).ToArray(), 0);
      var pointer = 4;
      if (payloadType != TreasuryTransactionPayloadType)
      {
        throw new Exception($"Payload Type ({payloadType}) is not a treasury transaction payload!");
      }

      var inputType = BitConverter.ToInt32(payload.Skip(pointer).Take(4).ToArray(), 0);
      pointer += 4;

      var inputMilestoneId = payload.Skip(pointer).Take(32).ToHex();
      pointer += 32;

      var outputType = BitConverter.ToInt32(payload.Skip(pointer).Take(4).ToArray(), 0);
      pointer += 4;

      var outputAmount = BitConverter.ToInt64(payload.Skip(pointer).Take(8).ToArray(), 0);

      return new TreasuryTransactionPayload
      {
        Input = new TreasuryInput
        {
          MilestoneId = inputMilestoneId,
          Type = inputType
        },
        Output = new TreasuryOutput
        {
          Amount = outputAmount,
          Type = outputType
        },
        Type = payloadType
      };
    }
  }
}