using System.Collections.Generic;
using Newtonsoft.Json;

namespace Tangle.Net.Api.Responses
{
  public class TipsResponse
  {
    [JsonProperty("tipMessageIds")]
    public List<string> TipMessageIds { get; set; }
  }
}