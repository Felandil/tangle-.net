using System.Collections.Generic;
using Newtonsoft.Json;

namespace Tangle.Net.Api.Response.Utxo
{
  public class OutputsResponse
  {
    [JsonProperty("addressType")] 
    public int AddressType { get; set; }

    [JsonProperty("address")] 
    public string Address { get; set; }

    [JsonProperty("maxResults")] 
    public int MaxResults { get; set; }

    [JsonProperty("count")] 
    public int Count { get; set; }

    [JsonProperty("outputIds")] 
    public List<string> OutputIds { get; set; }
  }
}