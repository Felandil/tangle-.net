﻿namespace Tangle.Net.Entity.Message.Payload
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Text;

  using Newtonsoft.Json;

  using Tangle.Net.Utils;

  public class IndexationPayload : Payload
  {
    public IndexationPayload()
    {
      this.Type = IndexationPayloadType;
    }

    [JsonProperty("data")]
    public string Data { get; set; }

    [JsonProperty("index")]
    public string Index { get; set; }

    /// <inheritdoc />
    protected override byte[] SerializeImplementation()
    {
      var serialized = new List<byte>();
      serialized.AddRange(BitConverter.GetBytes(this.Type));
      serialized.AddRange(BitConverter.GetBytes((short)(this.Index.Length / 2)));
      serialized.AddRange(this.Index.HexToBytes());
      serialized.AddRange(BitConverter.GetBytes(this.Data.Length / 2));
      serialized.AddRange(this.Data.HexToBytes());

      return serialized.ToArray();
    }

    public static IndexationPayload Deserialize(byte[] payload)
    {
      var payloadType = BitConverter.ToInt32(payload.Take(4).ToArray(), 0);
      var pointer = 4;
      if (payloadType != IndexationPayloadType)
      {
        throw new Exception($"Payload Type ({payloadType}) is not an indexation payload!");
      }

      var indexLength = BitConverter.ToInt16(payload.Skip(pointer).Take(2).ToArray(), 0);
      pointer += 2;

      var index = payload.Skip(pointer).Take(indexLength).ToHex();
      pointer += indexLength;

      var dataLength = BitConverter.ToInt32(payload.Skip(pointer).Take(4).ToArray(), 0);
      pointer += 4;

      var data = payload.Skip(pointer).Take(dataLength).ToHex();

      return new IndexationPayload { Data = data, Index = index };
    }
  }
}