using Newtonsoft.Json;
using Tangle.Net.Entity.Ed25519;
using Tangle.Net.Entity.Message.Payload.Transaction;

namespace Tangle.Net.Entity.Message.Payload.Receipt
{
  public class MigratedFunds
  {
    [JsonProperty("tailTransactionHash")] 
    public string TailTransactionHash { get; set; }

    [JsonProperty("address")] 
    public Ed25519Address Address { get; set; }

    [JsonProperty("deposit")] 
    public long Deposit { get; set; }
  }
}