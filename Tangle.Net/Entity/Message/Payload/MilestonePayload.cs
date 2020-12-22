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

    [JsonProperty("parent1MessageId")]
    public string Parent1MessageId { get; set; }

    [JsonProperty("parent2MessageId")]
    public string Parent2MessageId { get; set; }

    [JsonProperty("publicKeys")]
    public List<string> PublicKeys { get; set; }

    [JsonProperty("signatures")]
    public List<string> Signatures { get; set; }

    [JsonProperty("timestamp")]
    public long Timestamp { get; set; }

    /// <inheritdoc />
    protected override byte[] SerializeImplementation()
    {
      var result = new List<byte>();

      result.AddRange(BitConverter.GetBytes(this.Type));
      result.AddRange(BitConverter.GetBytes(this.Index));
      result.AddRange(BitConverter.GetBytes(this.Timestamp));
      result.AddRange(this.Parent1MessageId.HexToBytes());
      result.AddRange(this.Parent2MessageId.HexToBytes());
      result.AddRange(this.InclusionMerkleProof.HexToBytes());

      result.Add((byte)this.PublicKeys.Count);
      foreach (var publicKey in this.PublicKeys)
      {
        result.AddRange(publicKey.HexToBytes());
      }

      result.Add((byte)(this.Signatures.Count));
      foreach (var signature in this.Signatures)
      {
        result.AddRange(signature.HexToBytes());
      }

      return result.ToArray();
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

      var parent1MessageId = payload.Skip(pointer).Take(32).ToHex();
      pointer += 32;

      var parent2MessageId = payload.Skip(pointer).Take(32).ToHex();
      pointer += 32;

      var inclusionMerkleProof = payload.Skip(pointer).Take(32).ToHex();
      pointer += 32;

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
        signatures.Add(payload.Skip(pointer).Take(32).ToHex());
        pointer += 32;
      }


      return new MilestonePayload
               {
                 Index = index,
                 Timestamp = timestamp,
                 Parent1MessageId = parent1MessageId,
                 Parent2MessageId = parent2MessageId,
                 InclusionMerkleProof = inclusionMerkleProof,
                 PublicKeys = publicKeys,
                 Signatures = signatures
               };
    }
  }
}