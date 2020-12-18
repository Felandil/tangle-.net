namespace Tangle.Net.Entity.Message.Payload.Transaction
{
  using Newtonsoft.Json;

  public class ReferenceUnlockBlock : PayloadBase
  {
    [JsonProperty("reference")]
    public long Reference { get; set; }

    /// <inheritdoc />
    protected override byte[] SerializeImplementation()
    {
      return new byte[] { };
    }
  }
}