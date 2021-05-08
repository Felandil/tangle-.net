using System;
using System.Collections.Generic;
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
  }
}