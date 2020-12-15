namespace Tangle.Net.Api
{
  using Newtonsoft.Json;

  internal class ClientResponse<T>
  {
    [JsonProperty(PropertyName = "data")]
    public T Data { get; set; }
  }
}