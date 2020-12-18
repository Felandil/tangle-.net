namespace Tangle.Net.Entity.Message.Payload.Transaction
{
  using Newtonsoft.Json;

  public class Ed25519Address : PayloadBase
  {
    [JsonProperty("address")]
    public string Address { get; set; }

    /// <inheritdoc />
    protected override byte[] SerializeImplementation()
    {
      return new byte[] { };
    }
  }
}