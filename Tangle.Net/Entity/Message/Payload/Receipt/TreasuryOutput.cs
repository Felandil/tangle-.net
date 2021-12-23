using Newtonsoft.Json;

namespace Tangle.Net.Entity.Message.Payload.Receipt
{
  public class TreasuryOutput : PayloadType
  {
    [JsonProperty("amount")] 
    public long Amount { get; set; }
  }
}