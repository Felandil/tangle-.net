namespace Tangle.Net.Api.Responses
{
  using System.Collections.Generic;

  using Newtonsoft.Json;

  public class MessageIdsByIndexResponse
  {
    [JsonProperty("count")]
    public int Count { get; set; }

    [JsonProperty("index")]
    public string Index { get; set; }

    [JsonProperty("maxResults")]
    public int MaxResults { get; set; }

    [JsonProperty("messageIds")]
    public List<string> MessageIds { get; set; }
  }
}