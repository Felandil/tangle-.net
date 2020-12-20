namespace Tangle.Net.Entity.Message.Payload
{
  using System;
  using System.Collections.Generic;

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
      //writeStream.writeUInt32("payloadMilestone.type", object.type);
      result.AddRange(BitConverter.GetBytes(this.Type));

      //writeStream.writeUInt32("payloadMilestone.index", object.index);
      result.AddRange(BitConverter.GetBytes(this.Index));

      //writeStream.writeUInt64("payloadMilestone.timestamp", BigInt(object.timestamp));
      result.AddRange(BitConverter.GetBytes(this.Timestamp));

      //writeStream.writeFixedHex("payloadMilestone.parent1MessageId", MESSAGE_ID_LENGTH, object.parent1MessageId);
      //writeStream.writeFixedHex("payloadMilestone.parent2MessageId", MESSAGE_ID_LENGTH, object.parent2MessageId);
      result.AddRange(this.Parent1MessageId.HexToBytes());
      result.AddRange(this.Parent2MessageId.HexToBytes());

      //writeStream.writeFixedHex("payloadMilestone.inclusionMerkleProof",
      //  MERKLE_PROOF_LENGTH, object.inclusionMerkleProof);
      result.AddRange(this.InclusionMerkleProof.HexToBytes());

      //writeStream.writeByte("payloadMilestone.publicKeysCount", object.publicKeys.length);
      //for (let i = 0; i < object.publicKeys.length; i++)
      //{
      //  writeStream.writeFixedHex("payloadMilestone.publicKey", Ed25519.PUBLIC_KEY_SIZE, object.publicKeys[i]);
      //}
      result.Add((byte)this.PublicKeys.Count);
      foreach (var publicKey in this.PublicKeys)
      {
        result.AddRange(publicKey.HexToBytes());
      }

      //writeStream.writeByte("payloadMilestone.signaturesCount", object.signatures.length);
      //for (let i = 0; i < object.signatures.length; i++)
      //{
      //  writeStream.writeFixedHex("payloadMilestone.signature", Ed25519.SIGNATURE_SIZE, object.signatures[i]);
      //}

      result.Add((byte)(this.Signatures.Count));
      foreach (var signature in this.Signatures)
      {
        result.AddRange(signature.HexToBytes());
      }

      return result.ToArray();
    }
  }
}