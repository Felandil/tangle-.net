namespace Tangle.Net.Models.Message.MessagePayload
{
  using Newtonsoft.Json;

  public abstract class PayloadBase
  {
    [JsonProperty("type")]
    public int Type { get; set; }
  }
}