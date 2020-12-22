namespace Tangle.Net.Entity.Message
{
  using System;
  using System.Collections.Generic;
  using System.Linq;

  using Newtonsoft.Json;

  using Tangle.Net.Entity.Message.Payload;
  using Tangle.Net.Utils;

  public class Message<T>
    where T : Payload.Payload

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

    public static Message<T> Deserialize(byte[] message)
    {
      var networkId = BitConverter.ToInt64(message.Take(8).ToArray(), 0);
      var pointer = 8;

      var parent1MessageId = message.Skip(pointer).Take(32).ToHex();
      pointer += 32;

      var parent2MessageId = message.Skip(pointer).Take(32).ToHex();
      pointer += 32;

      var payloadLength = BitConverter.ToInt32(message.Skip(pointer).Take(4).ToArray(), 0);
      pointer += 4;

      var payload = Message.Payload.Payload.Deserialize<T>(message.Skip(pointer).Take(payloadLength).ToArray());
      pointer += payloadLength;

      var nonce = BitConverter.ToInt64(message.Skip(pointer).Take(8).ToArray(), 0);
      pointer += 8;

      if (pointer != message.Length)
      {
        throw new Exception($"Message data length {pointer} has unused data {message.Length - pointer}");
      }

      return new Message<T>
               {
                 NetworkId = networkId.ToString(),
                 Nonce = nonce.ToString(),
                 Parent1MessageId = parent1MessageId,
                 Parent2MessageId = parent2MessageId,
                 Payload = payload
               };
    }
  }
}