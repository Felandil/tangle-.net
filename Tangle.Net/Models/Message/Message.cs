namespace Tangle.Net.Models.Message
{
  using Newtonsoft.Json;

  using Tangle.Net.Models.Message.Payload;

  public class Message<T>
    where T : PayloadBase

  {
    [JsonProperty("networkId")]
    public string NetworkId { get; set; }

    [JsonProperty("nonce")]
    public string Nonce { get; set; }

    [JsonProperty("parent1MessageId")]
    public string Parent1MessageId { get; set; }

    [JsonProperty("parent2MessageId")]
    public string Parent2MessageId { get; set; }

    [JsonProperty("payload")]
    public T Payload { get; set; }
  }
}