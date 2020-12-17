namespace Tangle.Net.Models.Message.Payload
{
  using Newtonsoft.Json;

  using Tangle.Net.Models.Message.Payload.Transaction;

  public class TransactionPayload<T> : PayloadBase
    where T : PayloadBase
  {
    [JsonProperty("essence")]
    public TransactionEssence Essence { get; set; }

    [JsonProperty("unlockBlocks")]
    public T UnlockBlocks { get; set; }
  }
}