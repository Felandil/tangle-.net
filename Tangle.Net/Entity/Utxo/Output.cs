using Newtonsoft.Json;
using Tangle.Net.Entity.Ed25519;
using Tangle.Net.Entity.Message.Payload.Transaction;

namespace Tangle.Net.Entity.Utxo
{
  public class Output
  {
    [JsonProperty("type")] 
    public int Type { get; set; }

    [JsonProperty("address")] 
    public Ed25519Address Address { get; set; }

    [JsonProperty("amount")] 
    public long Amount { get; set; }
  }
}