namespace Tangle.Net.Models
{
  using System.Collections.Generic;

  using Newtonsoft.Json;

  public class NodeInfo
  {
    [JsonProperty("features")]
    public List<string> Features { get; set; }

    [JsonProperty("isHealthy")]
    public bool IsHealthy { get; set; }

    [JsonProperty("latestMilestoneIndex")]
    public int LatestMilestoneIndex { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("networkId")]
    public string NetworkId { get; set; }

    [JsonProperty("pruningIndex")]
    public int PruningIndex { get; set; }

    [JsonProperty("solidMilestoneIndex")]
    public int SolidMilestoneIndex { get; set; }

    [JsonProperty("version")]
    public string Version { get; set; }
  }
}