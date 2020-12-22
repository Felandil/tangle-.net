namespace Tangle.Net.Entity.Message.Payload.Transaction
{
  using System;
  using System.Collections.Generic;

  using Newtonsoft.Json;

  public class ReferenceUnlockBlock : UnlockBlock
  {
    [JsonProperty("reference")]
    public short Reference { get; set; }

    /// <inheritdoc />
    public override byte[] Serialize()
    {
      var serialized = new List<byte>();

      serialized.Add((byte)this.Type);
      serialized.AddRange(BitConverter.GetBytes(this.Reference));

      return serialized.ToArray();
    }
  }
}