namespace Tangle.Net.Models.MessagePayload
{
  using Newtonsoft.Json;

  public abstract class PayloadBase
  {
    [JsonProperty(PropertyName = "type")]
    public int Type { get; set; }
  }
}