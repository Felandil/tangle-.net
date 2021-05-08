using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Tangle.Net.Entity.Message.Payload.Receipt;
using Tangle.Net.Utils;

namespace Tangle.Net.Entity.Message.Payload
{
  public class MilestonePayload : Payload
  {
    public MilestonePayload()
    {
      Type = MilestonePayloadType;
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

    [JsonProperty("receipt")] 
    public ReceiptPayload Receipt { get; set; }

    /// <inheritdoc />
    protected override byte[] SerializeImplementation()
    {
      var serialized = new List<byte>();
      var parentMessageIds = ParentMessageIds.Select(id => id.HexToBytes()).ToList();
      var publicKeys = PublicKeys.Select(key => key.HexToBytes()).ToList();
      var signatures = Signatures.Select(signature => signature.HexToBytes()).ToList();

      serialized.AddRange(BitConverter.GetBytes(Type));
      serialized.AddRange(BitConverter.GetBytes(Index));
      serialized.AddRange(BitConverter.GetBytes(Timestamp));
      serialized.Add((byte) ParentMessageIds.Count);

      parentMessageIds.ForEach(serialized.AddRange);
      serialized.AddRange(InclusionMerkleProof.HexToBytes());
      serialized.AddRange(BitConverter.GetBytes(NextPoWScore));
      serialized.AddRange(BitConverter.GetBytes(NextPoWScoreMilestoneIndex));

      serialized.Add((byte) PublicKeys.Count);
      publicKeys.ForEach(serialized.AddRange);

      if (this.Receipt != null)
      {
        serialized.AddRange(this.Receipt.Serialize());
      }
      else
      {
        serialized.AddRange(BitConverter.GetBytes(0));
      }

      serialized.Add((byte) Signatures.Count);
      signatures.ForEach(serialized.AddRange);

      return serialized.ToArray();
    }

    public static MilestonePayload Deserialize(byte[] payload)
    {
      var payloadType = BitConverter.ToInt32(payload.Take(4).ToArray(), 0);
      var pointer = 4;
      if (payloadType != MilestonePayloadType)
        throw new Exception($"Payload Type ({payloadType}) is not a milestone payload!");

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

      var publicKeyCount = payload.Skip(pointer).Take(1).First();
      pointer += 1;

      var publicKeys = new List<string>();
      for (var i = 0; i < publicKeyCount; i++)
      {
        publicKeys.Add(payload.Skip(pointer).Take(32).ToHex());
        pointer += 32;
      }

      var hasReceipt = BitConverter.ToInt32(payload.Skip(pointer).Take(4).ToArray(), 0) != 0;
      if (hasReceipt)
      {

      }
      else
      {
        pointer += 4;
      }

      var signatureCount = payload.Skip(pointer).Take(1).First();
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
        NextPoWScoreMilestoneIndex = nextPoWScoreMilestoneIndex,
        Receipt = null
      };
    }
  }
}