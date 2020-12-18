namespace Tangle.Net.Entity.Message.Payload.Transaction
{
  using Newtonsoft.Json;

  // ReSharper disable once InconsistentNaming
  public class UTXOInput : PayloadBase
  {
    [JsonProperty("transactionId")]
    public string TransactionId { get; set; }

    [JsonProperty("transactionOutputIndex")]
    public int TransactionOutputIndex { get; set; }

    /// <inheritdoc />
    protected override byte[] SerializeImplementation()
    {
      return new byte[] { };
    }
  }
}