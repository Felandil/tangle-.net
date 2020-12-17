namespace Tangle.Net.Models.Message.Payload
{
  using Newtonsoft.Json;

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
  }
}