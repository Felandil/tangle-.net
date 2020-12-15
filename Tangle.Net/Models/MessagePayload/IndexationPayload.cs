namespace Tangle.Net.Models.MessagePayload
{
  using Newtonsoft.Json;

  public class IndexationPayload : PayloadBase
  {
    public IndexationPayload()
    {
      this.Type = 2;
    }

    [JsonProperty(PropertyName = "data")]
    public string Data { get; set; }

    [JsonProperty(PropertyName = "index")]
    public string Index { get; set; }
  }
}