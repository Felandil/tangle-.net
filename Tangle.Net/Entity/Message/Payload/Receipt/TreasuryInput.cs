using Newtonsoft.Json;

namespace Tangle.Net.Entity.Message.Payload.Receipt
{
  public class TreasuryInput : PayloadType
  {
    [JsonProperty("milestoneId")] 
    public string MilestoneId { get; set; }
  }
}