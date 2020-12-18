namespace Tangle.Net.Entity.Message.Payload
{
  using Newtonsoft.Json;

  public class PayloadType
  {
    [JsonProperty("type")]
    public int Type { get; set; }
  }
}