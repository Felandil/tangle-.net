namespace Tangle.Net.Entity.Message.Payload
{
  using Newtonsoft.Json;

  using Tangle.Net.Entity.Message.Payload.Transaction;

  public class TransactionPayload<T> : PayloadBase
    where T : PayloadBase
  {
    [JsonProperty("essence")]
    public TransactionEssence Essence { get; set; }

    [JsonProperty("unlockBlocks")]
    public T UnlockBlocks { get; set; }

    /// <inheritdoc />
    protected override byte[] SerializeImplementation()
    {
      return new byte[] { };
    }
  }
}