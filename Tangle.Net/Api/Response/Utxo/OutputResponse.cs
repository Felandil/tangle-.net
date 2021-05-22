using Newtonsoft.Json;
using Tangle.Net.Entity.Utxo;

namespace Tangle.Net.Api.Response.Utxo
{
  public class OutputResponse
  {
    [JsonProperty("messageId")]
    public string MessageId { get; set; }

    [JsonProperty("transactionId")]
    public string TransactionId { get; set; }

    [JsonProperty("outputIndex")]
    public short OutputIndex { get; set; }

    [JsonProperty("isSpent")]
    public bool IsSpent { get; set; }

    [JsonProperty("output")]
    public Output Output { get; set; }
  }
}
