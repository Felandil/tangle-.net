namespace Tangle.Net.Entity.Message.Payload
{
  using Newtonsoft.Json;

  using Tangle.Net.Entity.Message.Payload.Transaction;

  public class TransactionPayload<TBlock> : Payload
    where TBlock : Payload
  {
    [JsonProperty("essence")]
    public TransactionEssence Essence { get; set; }

    [JsonProperty("unlockBlocks")]
    public TBlock UnlockBlocks { get; set; }

    /// <inheritdoc />
    protected override byte[] SerializeImplementation()
    {
      return new byte[] { };
    }
  }
}