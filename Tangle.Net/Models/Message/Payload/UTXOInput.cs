namespace Tangle.Net.Models.Message.Payload
{
  using Newtonsoft.Json;

  // ReSharper disable once InconsistentNaming
  public class UTXOInput
  {
    [JsonProperty("transactionId")]
    public string TransactionId { get; set; }

    [JsonProperty("transactionOutputIndex")]
    public int TransactionOutputIndex { get; set; }
  }
}