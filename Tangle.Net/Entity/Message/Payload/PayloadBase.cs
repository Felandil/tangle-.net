namespace Tangle.Net.Entity.Message.Payload
{
  using System;
  using System.Collections.Generic;

  using Newtonsoft.Json;

  public abstract class PayloadBase
  {
    [JsonProperty("type")]
    public int Type { get; set; }

    protected abstract byte[] SerializeImplementation();

    public byte[] Serialize()
    {
      var result = new List<byte>();

      var serializedImplementation = this.SerializeImplementation();
      result.AddRange(BitConverter.GetBytes(serializedImplementation.Length));
      result.AddRange(serializedImplementation);

      return result.ToArray();
    }
  }
}