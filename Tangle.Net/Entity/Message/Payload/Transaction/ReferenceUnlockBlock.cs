namespace Tangle.Net.Entity.Message.Payload.Transaction
{
  using Newtonsoft.Json;

  public class ReferenceUnlockBlock : PayloadType
  {
    [JsonProperty("reference")]
    public long Reference { get; set; }
  }
}