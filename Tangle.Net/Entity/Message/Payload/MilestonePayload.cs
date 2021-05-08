namespace Tangle.Net.Entity.Message.Payload
{
  using System;
  using System.Collections.Generic;
  using System.Linq;

  using Newtonsoft.Json;

  using Tangle.Net.Utils;

  public class MilestonePayload : Payload
  {
    public MilestonePayload()
    {
      this.Type = MilestonePayloadType;
    }

    [JsonProperty("inclusionMerkleProof")]
    public string InclusionMerkleProof { get; set; }

    [JsonProperty("index")]
    public int Index { get; set; }

    [JsonProperty("parentMessageIds")]
    public List<string> ParentMessageIds { get; set; }

    [JsonProperty("publicKeys")]
    public List<string> PublicKeys { get; set; }

    [JsonProperty("nextPoWScore")]
    public int NextPoWScore { get; set; }

    [JsonProperty("nextPoWScoreMilestoneIndex")]
    public int NextPoWScoreMilestoneIndex { get; set; }

    [JsonProperty("signatures")]
    public List<string> Signatures { get; set; }

    [JsonProperty("timestamp")]
    public long Timestamp { get; set; }

    /// <inheritdoc />
    protected override byte[] SerializeImplementation()
    {
      var serialized = new List<byte>();
      var parentMessageIds = this.ParentMessageIds.Select(id => id.HexToBytes()).ToList();
      var publicKeys = this.PublicKeys.Select(key => key.HexToBytes()).ToList();
      var signatures = this.Signatures.Select(signature => signature.HexToBytes()).ToList();

      serialized.AddRange(BitConverter.GetBytes(this.Type));
      serialized.AddRange(BitConverter.GetBytes(this.Index));
      serialized.AddRange(BitConverter.GetBytes(this.Timestamp));
      serialized.Add((byte)this.ParentMessageIds.Count);

      parentMessageIds.ForEach(serialized.AddRange);
      serialized.AddRange(this.InclusionMerkleProof.HexToBytes());
      serialized.AddRange(BitConverter.GetBytes(this.NextPoWScore));
      serialized.AddRange(BitConverter.GetBytes(this.NextPoWScoreMilestoneIndex));


      serialized.Add((byte)this.PublicKeys.Count);
      publicKeys.ForEach(serialized.AddRange);
      serialized.Add((byte)(this.Signatures.Count));
      signatures.ForEach(serialized.AddRange);

      return serialized.ToArray();
    }

    public static MilestonePayload Deserialize(byte[] payload)
    {
      var payloadType = BitConverter.ToInt32(payload.Take(4).ToArray(), 0);
      var pointer = 4;
      if (payloadType != MilestonePayloadType)
      {
        throw new Exception($"Payload Type ({payloadType}) is not a milestone payload!");
      }

      var index = BitConverter.ToInt32(payload.Skip(pointer).Take(4).ToArray(), 0);
      pointer += 4;

      var timestamp = BitConverter.ToInt64(payload.Skip(pointer).Take(8).ToArray(), 0);
      pointer += 8;

      var parentMessageIdCount = payload.Skip(pointer).Take(1).First();
      pointer += 1;

      var parentMessageIds = new List<string>();
      for (var i = 0; i < parentMessageIdCount; i++)
      {
        parentMessageIds.Add(payload.Skip(pointer).Take(32).ToHex());
        pointer += 32;
      }

      var inclusionMerkleProof = payload.Skip(pointer).Take(32).ToHex();
      pointer += 32;

      var nextPoWScore = BitConverter.ToInt32(payload.Take(4).ToArray(), 0);
      pointer += 4;

      var nextPoWScoreMilestoneIndex = BitConverter.ToInt32(payload.Take(4).ToArray(), 0);
      pointer += 4;

      var publicKeyCount = payload[pointer];
      pointer += 1;

      var publicKeys = new List<string>();
      for (var i = 0; i < publicKeyCount; i++)
      {
        publicKeys.Add(payload.Skip(pointer).Take(32).ToHex());
        pointer += 32;
      }

      var signatureCount = payload[pointer];
      pointer += 1;

      var signatures = new List<string>();
      for (var i = 0; i < signatureCount; i++)
      {
        signatures.Add(payload.Skip(pointer).Take(64).ToHex());
        pointer += 64;
      }

      return new MilestonePayload
               {
                 Index = index,
                 Timestamp = timestamp,
                 ParentMessageIds = parentMessageIds,
                 InclusionMerkleProof = inclusionMerkleProof,
                 PublicKeys = publicKeys,
                 Signatures = signatures,
                 NextPoWScore = nextPoWScore,
                 NextPoWScoreMilestoneIndex = nextPoWScoreMilestoneIndex
               };
    }
  }
}