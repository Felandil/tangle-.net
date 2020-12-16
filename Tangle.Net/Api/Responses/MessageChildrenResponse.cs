namespace Tangle.Net.Api.Responses
{
  using System.Collections.Generic;

  using Newtonsoft.Json;

  public class MessageChildrenResponse
  {
    [JsonProperty("childrenMessageIds")]
    public List<string> ChildrenMessageIds { get; set; }

    [JsonProperty("count")]
    public int Count { get; set; }

    [JsonProperty("maxResults")]
    public int MaxResults { get; set; }

    [JsonProperty("messageId")]
    public string MessageId { get; set; }
  }
}