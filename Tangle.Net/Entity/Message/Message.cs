namespace Tangle.Net.Entity.Message
{
  using System;
  using System.Collections.Generic;
  using System.Linq;

  using Newtonsoft.Json;

  using Tangle.Net.Utils;

  public class Message<T>
    where T : Payload.Payload

  {
    [JsonProperty("networkId")]
    public string NetworkId { get; set; }

    [JsonProperty("nonce")]
    public string Nonce { get; set; }

    [JsonProperty("parentMessageIds")]
    public List<string> ParentMessageIds { get; set; }

    [JsonProperty("payload")]
    public T Payload { get; set; }

    public byte[] Serialize()
    {
      var result = new List<byte>();
      var parentMessageIds = this.ParentMessageIds.Select(id => id.HexToBytes()).ToList();

      result.AddRange(BitConverter.GetBytes(long.Parse(this.NetworkId ?? "0")));
      parentMessageIds.ForEach(result.AddRange);
      result.AddRange(this.Payload.Serialize());
      result.AddRange(BitConverter.GetBytes(long.Parse(this.Nonce ?? "0")));

      return result.ToArray();
    }

    public static Message<T> Deserialize(byte[] message)
    {
      var networkId = BitConverter.ToInt64(message.Take(8).ToArray(), 0);
      var parentMessageIds = new List<string>();
      var pointer = 8;

      parentMessageIds.Add(message.Skip(pointer).Take(32).ToHex());
      pointer += 32;

      parentMessageIds.Add(message.Skip(pointer).Take(32).ToHex());
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
                 ParentMessageIds = parentMessageIds,
                 Payload = payload
               };
    }
  }
}