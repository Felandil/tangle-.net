namespace Tangle.Net.Models.Message.Payload
{
  using Newtonsoft.Json;

  public abstract class PayloadBase
  {
    [JsonProperty("type")]
    public int Type { get; set; }
  }
}