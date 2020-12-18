namespace Tangle.Net.Entity.Message.Payload
{
  using System;
  using System.Collections.Generic;
  using System.Text;

  using Newtonsoft.Json;

  using Tangle.Net.Utils;

  public class IndexationPayload : PayloadBase
  {
    public IndexationPayload()
    {
      this.Type = 2;
    }

    [JsonProperty("data")]
    public string Data { get; set; }

    [JsonProperty("index")]
    public string Index { get; set; }

    /// <inheritdoc />
    protected override byte[] SerializeImplementation()
    {
      var bytes = new List<byte>();
      bytes.AddRange(BitConverter.GetBytes(this.Type));
      bytes.AddRange(BitConverter.GetBytes((short)this.Index.Length));
      bytes.AddRange(Encoding.ASCII.GetBytes(this.Index));
      bytes.AddRange(BitConverter.GetBytes(this.Data.Length / 2));
      bytes.AddRange(this.Data.HexToBytes());

      return bytes.ToArray();
    }
  }
}