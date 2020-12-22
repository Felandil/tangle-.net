namespace Tangle.Net.Entity.Message.Payload
{
  using Newtonsoft.Json;

  public abstract class PayloadType
  {
    [JsonProperty("type")]
    public int Type { get; set; }
  }
}