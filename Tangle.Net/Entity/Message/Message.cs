namespace Tangle.Net.Entity.Message
{
  using System;
  using System.Collections.Generic;

  using Newtonsoft.Json;

  using Tangle.Net.Entity.Message.Payload;
  using Tangle.Net.Utils;

  public class Message<T>
    where T : PayloadBase

  {
    [JsonProperty("networkId")]
    public string NetworkId { get; set; }

    [JsonProperty("nonce")]
    public string Nonce { get; set; }

    [JsonProperty("parent1MessageId")]
    public string Parent1MessageId { get; set; }

    [JsonProperty("parent2MessageId")]
    public string Parent2MessageId { get; set; }

    [JsonProperty("payload")]
    public T Payload { get; set; }

    public byte[] Serialize()
    {
      var result = new List<byte>();

      result.AddRange(BitConverter.GetBytes(long.Parse(this.NetworkId ?? "0")));
      result.AddRange(this.Parent1MessageId.HexToBytes());
      result.AddRange(this.Parent2MessageId.HexToBytes());
      result.AddRange(this.Payload.Serialize());
      result.AddRange(BitConverter.GetBytes(long.Parse(this.Nonce ?? "0")));

      return result.ToArray();
    }
  }
}