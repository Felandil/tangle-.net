using System.Collections.Generic;
using Newtonsoft.Json;

namespace Tangle.Net.Api.Response.Message
{
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