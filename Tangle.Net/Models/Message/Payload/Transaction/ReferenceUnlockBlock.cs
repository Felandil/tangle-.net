namespace Tangle.Net.Models.Message.Payload.Transaction
{
  using Newtonsoft.Json;

  public class ReferenceUnlockBlock : PayloadBase
  {
    [JsonProperty("reference")]
    public long Reference { get; set; }
  }
}