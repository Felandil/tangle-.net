namespace Tangle.Net.Models
{
  using Newtonsoft.Json;

  using Tangle.Net.Models.MessagePayload;

  public class Message<T>
    where T : PayloadBase

  {
    [JsonProperty(PropertyName = "networkId")]
    public string NetworkId { get; set; }

    [JsonProperty(PropertyName = "nonce")]
    public string Nonce { get; set; }

    [JsonProperty(PropertyName = "parent1MessageId")]
    public string Parent1MessageId { get; set; }

    [JsonProperty(PropertyName = "parent2MessageId")]
    public string Parent2MessageId { get; set; }

    [JsonProperty(PropertyName = "payload")]
    public T Payload { get; set; }
  }
}