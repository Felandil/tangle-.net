namespace Tangle.Net.Entity
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Text;

  using Isopoh.Cryptography.Blake2b;

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

    [JsonProperty("minPoWScore")]
    public int MinPoWScore { get; set; }

    [JsonProperty("networkId")]
    public string NetworkId { get; set; }

    [JsonProperty("bech32HRP")]
    public string Bech32Hrp { get; set; }

    [JsonProperty("messagesPerSecond")]
    public int MessagesPerSecond { get; set; }

    [JsonProperty("pruningIndex")]
    public int PruningIndex { get; set; }

    [JsonProperty("solidMilestoneIndex")]
    public int SolidMilestoneIndex { get; set; }

    [JsonProperty("version")]
    public string Version { get; set; }

    public string CalculateMessageNetworkId()
    {
      var networkIdBytes = Blake2B.ComputeHash(Encoding.ASCII.GetBytes(this.NetworkId), new Blake2BConfig { OutputSizeInBytes = 32 }, null);
      return BitConverter.ToInt64(networkIdBytes.Take(8).ToArray(), 0).ToString();
    }
  }
}